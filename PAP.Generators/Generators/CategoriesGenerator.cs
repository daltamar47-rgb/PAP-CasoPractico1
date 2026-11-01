using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Categories — no dependencies. Names are unique in the table, so a fixed list is used.
    /// </summary>
    public class CategoriesGenerator : GeneratorBase<Category>
    {
        private static readonly string[] Names =
        {
            "Ficción", "No ficción", "Infantil", "Juvenil", "Ciencia", "Tecnología",
            "Historia", "Negocios", "Arte", "Autoayuda", "Educación", "Viajes"
        };

        public CategoriesGenerator()
            : base(new CategoryRepository())
        {
        }

        public override string TableName => Tables.Categories;

        protected override IEnumerable<Category> Build()
        {
            return Names.Select(name => new Category
            {
                Name = name,
                Description = "Libros de la categoría " + name.ToLowerInvariant() + "."
            });
        }
    }
}
