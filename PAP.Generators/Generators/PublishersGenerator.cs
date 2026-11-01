using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Publishers — no dependencies.
    /// </summary>
    public class PublishersGenerator : GeneratorBase<Publisher>
    {
        private const int Count = 15;

        public PublishersGenerator()
            : base(new PublisherRepository())
        {
        }

        public override string TableName => Tables.Publishers;

        protected override IEnumerable<Publisher> Build()
        {
            for (var index = 1; index <= Count; index++)
            {
                var name = Data.CompanyName() + " " + index;
                var slug = "editorial" + index;
                yield return new Publisher
                {
                    Name = name,
                    Country = Data.Pick(SampleData.Countries),
                    Website = "https://www." + slug + ".com",
                    Email = "contacto@" + slug + ".com",
                    CreatedAt = DateTime.Now
                };
            }
        }
    }
}
