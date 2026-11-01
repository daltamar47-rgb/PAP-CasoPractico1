using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Authors — no dependencies.
    /// </summary>
    public class AuthorsGenerator : GeneratorBase<Author>
    {
        private const int Count = 30;

        public AuthorsGenerator()
            : base(new AuthorRepository())
        {
        }

        public override string TableName => Tables.Authors;

        protected override IEnumerable<Author> Build()
        {
            for (var index = 1; index <= Count; index++)
            {
                yield return new Author
                {
                    FirstName = Data.FirstName(),
                    LastName = Data.LastName(),
                    Biography = Data.Sentence(18),
                    BirthDate = Data.DateBetween(new DateTime(1940, 1, 1), new DateTime(1995, 12, 31)).Date,
                    Country = Data.Pick(SampleData.Countries),
                    CreatedAt = DateTime.Now
                };
            }
        }
    }
}
