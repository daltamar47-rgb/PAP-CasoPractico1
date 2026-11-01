using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Core
{
    /// <summary>
    /// Base for the generators of tables that have an entity class.
    /// The rows are built in memory and inserted in bulk with the entity repository.
    /// </summary>
    public abstract class GeneratorBase<T> : IDataGenerator where T : class
    {
        private static readonly IReadOnlyList<string> NoDependencies = new string[0];

        protected readonly IRepositoryBase<T> Repository;
        protected readonly SampleData Data;

        protected GeneratorBase(IRepositoryBase<T> repository)
        {
            Repository = repository;
            Data = new SampleData();
        }

        public abstract string TableName { get; }

        public virtual IReadOnlyList<string> DependsOn => NoDependencies;

        public bool HasData()
        {
            return Repository.HasData();
        }

        public virtual int Generate()
        {
            var rows = Build().ToList();
            Repository.AddRange(rows);
            return rows.Count;
        }

        /// <summary>Creates the rows to insert.</summary>
        protected abstract IEnumerable<T> Build();

        /// <summary>Ids of the rows of another table, to use them as foreign keys.</summary>
        protected static IList<int> IdsOf<TOther>(IRepositoryBase<TOther> repository, Func<TOther, int> getId) where TOther : class
        {
            return repository.GetAll().Select(getId).ToList();
        }
    }
}
