using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Genres — no dependencies. Names are unique in the table, so a fixed list is used.
    /// </summary>
    public class GenresGenerator : GeneratorBase<Genre>
    {
        private static readonly string[] Names =
        {
            "Misterio", "Romance", "Fantasía", "Ciencia ficción", "Terror", "Drama",
            "Aventura", "Biografía", "Poesía", "Humor", "Suspenso", "Histórica"
        };

        public GenresGenerator()
            : base(new GenreRepository())
        {
        }

        public override string TableName => Tables.Genres;

        protected override IEnumerable<Genre> Build()
        {
            return Names.Select(name => new Genre
            {
                Name = name,
                Description = "Obras del género " + name.ToLowerInvariant() + "."
            });
        }
    }
}
