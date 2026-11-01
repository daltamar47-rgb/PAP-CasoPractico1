using PAP.DataAccess;
using PAP.DataAccess.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace PAP.Repositories
{
    /// <summary>
    /// Generic repository interface that defines the contract for data access operations.
    /// </summary>
    /// <typeparam name="T">The entity type that this repository manages</typeparam>
    public interface IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities of type T from the database.
        /// </summary>
        /// <returns>A collection of all entities</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Retrieves a single entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve</param>
        /// <returns>The entity with the specified ID, or null if not found</returns>
        T GetById(int id);

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        void Add(T entity);

        /// <summary>
        /// Adds many entities in a single save (bulk insert used by the data generators).
        /// </summary>
        /// <param name="entities">The entities to add</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an entity from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete</param>
        void Delete(int id);

        /// <summary>
        /// Indicates whether the table already has at least one row.
        /// </summary>
        bool HasData();

        /// <summary>
        /// Describes the scalar columns of the entity, read from the data model (.edmx).
        /// </summary>
        IReadOnlyList<EntityField> GetFields();

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        void Save();
    }

    /// <summary>
    /// Generic repository base class that provides common data access operations for entities.
    /// Implements the Repository pattern to encapsulate data access logic.
    /// </summary>
    /// <typeparam name="T">The entity type that this repository manages</typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyList<EntityField>> FieldsCache =
            new ConcurrentDictionary<Type, IReadOnlyList<EntityField>>();

        /// <summary>
        /// The Entity Framework database context used for data access.
        /// </summary>
        protected readonly BookStoreDBEntities _context;

        /// <summary>
        /// The Entity Framework DbSet for the entity type T.
        /// </summary>
        protected readonly DbSet<T> _set;

        /// <summary>
        /// Initializes a new instance of the RepositoryBase class.
        /// </summary>
        public RepositoryBase()
        {
            _context = new BookStoreDBEntities();
            _set = _context.Set<T>();
        }

        /// <summary>
        /// Retrieves all entities of type T from the database.
        /// </summary>
        /// <returns>A collection of all entities</returns>
        public IEnumerable<T> GetAll()
        {
            return _set.ToList();
        }

        /// <summary>
        /// Retrieves a single entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve</param>
        /// <returns>The entity with the specified ID, or null if not found</returns>
        public T GetById(int id)
        {
            return _set.Find(id);
        }

        /// <summary>
        /// Adds a new entity to the database and saves changes immediately.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        public void Add(T entity)
        {
            _set.Add(entity);
            Save();
        }

        /// <summary>
        /// Adds many entities and saves them with a single SaveChanges call.
        /// </summary>
        /// <param name="entities">The entities to add</param>
        public void AddRange(IEnumerable<T> entities)
        {
            _set.AddRange(entities);
            Save();
        }

        /// <summary>
        /// Updates an existing entity in the database and saves changes immediately.
        /// </summary>
        /// <param name="entity">The entity to update</param>
        public void Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _set.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;
            Save();
        }

        /// <summary>
        /// Deletes an entity from the database by its unique identifier and saves changes immediately.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete</param>
        public void Delete(int id)
        {
            T entityToDelete = _set.Find(id);
            if (entityToDelete != null)
            {
                _set.Remove(entityToDelete);
                Save();
            }
        }

        /// <summary>
        /// Indicates whether the table already has at least one row.
        /// </summary>
        public bool HasData()
        {
            return _set.Any();
        }

        /// <summary>
        /// Reads the column definitions of T from the conceptual model of the .edmx
        /// (name, type, nullability, max length, key and DATE columns).
        /// </summary>
        public IReadOnlyList<EntityField> GetFields()
        {
            return FieldsCache.GetOrAdd(typeof(T), _ => ReadFieldsFromModel());
        }

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        private IReadOnlyList<EntityField> ReadFieldsFromModel()
        {
            var workspace = ((IObjectContextAdapter)_context).ObjectContext.MetadataWorkspace;
            var entityType = workspace
                .GetItems<EntityType>(DataSpace.CSpace)
                .Single(type => type.Name == typeof(T).Name);

            var keyNames = new HashSet<string>(entityType.KeyMembers.Select(key => key.Name));

            return entityType.Properties
                .Select(property => new EntityField(
                    property.Name,
                    typeof(T).GetProperty(property.Name).PropertyType,
                    property.Nullable,
                    property.IsMaxLength ? null : property.MaxLength,
                    keyNames.Contains(property.Name),
                    property.TypeName == "DateTime" && property.Precision == 0))
                .ToList();
        }
    }
}
