using PAP.DataAccess.Entities;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace PAP.Business.Core
{
    /// <summary>
    /// Business operations shared by every entity: list, find, create, update and delete.
    /// Each entity manager inherits from it and only adds its own rules.
    /// </summary>
    public abstract class ManagerBase<T> : IManager<T> where T : class
    {
        protected readonly IRepositoryBase<T> Repository;

        protected ManagerBase(IRepositoryBase<T> repository)
        {
            Repository = repository;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public T GetById(int id)
        {
            return Repository.GetById(id);
        }

        public IReadOnlyList<EntityField> GetFields()
        {
            return Repository.GetFields();
        }

        public int GetId(T entity)
        {
            var key = GetFields().First(field => field.IsKey);
            return (int)key.GetValue(entity);
        }

        public void Create(T entity)
        {
            PrepareIncoming(entity, isNew: true);
            AuditStamp.OnCreate(entity, GetFields());
            ApplyDefaults(entity);
            Validate(entity);
            Execute(() => Repository.Add(entity));
        }

        public void Update(T incoming)
        {
            var existing = Repository.GetById(GetId(incoming));
            if (existing == null)
            {
                throw new BusinessException("El registro que intenta editar ya no existe.");
            }

            PrepareIncoming(incoming, isNew: false);
            CopyEditableValues(incoming, existing);
            AuditStamp.OnUpdate(existing, GetFields());
            ApplyDefaults(existing);
            Validate(existing);
            Execute(() => Repository.Update(existing));
        }

        public void Delete(int id)
        {
            if (Repository.GetById(id) == null)
            {
                throw new BusinessException("El registro que intenta eliminar ya no existe.");
            }

            Execute(() => Repository.Delete(id));
        }

        /// <summary>
        /// Hook to transform the values typed by the user before they are saved (e.g. hash passwords).
        /// </summary>
        protected virtual void PrepareIncoming(T entity, bool isNew)
        {
        }

        /// <summary>
        /// Hook to fill values the database requires and the user may leave empty.
        /// </summary>
        protected virtual void ApplyDefaults(T entity)
        {
        }

        /// <summary>
        /// Hook for business rules. Throw a <see cref="BusinessException"/> to reject the operation.
        /// </summary>
        protected virtual void Validate(T entity)
        {
        }

        private void CopyEditableValues(T source, T target)
        {
            foreach (var field in GetFields().Where(field => field.IsEditable))
            {
                var value = field.GetValue(source);
                if (IsKeptValue(field, value))
                {
                    continue;
                }

                field.SetValue(target, value);
            }
        }

        /// <summary>
        /// Values left empty that must not overwrite the stored ones:
        /// a blank password, or a required date that was not sent.
        /// </summary>
        private static bool IsKeptValue(EntityField field, object value)
        {
            if (field.IsSecret)
            {
                return string.IsNullOrWhiteSpace(value as string);
            }

            return field.ClrType == typeof(DateTime) && !field.IsNullable && (DateTime)value == default(DateTime);
        }

        private static void Execute(Action operation)
        {
            try
            {
                operation();
            }
            catch (DbEntityValidationException ex)
            {
                throw new BusinessException(DataErrorTranslator.Translate(ex), ex);
            }
            catch (DbUpdateException ex)
            {
                throw new BusinessException(DataErrorTranslator.Translate(ex), ex);
            }
        }
    }
}
