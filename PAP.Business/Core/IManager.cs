using PAP.DataAccess.Entities;
using System.Collections.Generic;

namespace PAP.Business.Core
{
    /// <summary>
    /// Contract used by the controllers to work with any entity.
    /// </summary>
    public interface IManager<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        IReadOnlyList<EntityField> GetFields();

        int GetId(T entity);

        void Create(T entity);

        void Update(T entity);

        void Delete(int id);
    }
}
