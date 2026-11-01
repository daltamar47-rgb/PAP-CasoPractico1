using System.Collections.Generic;

namespace PAP.Generators.Core
{
    /// <summary>
    /// Generates sample rows for one table.
    /// </summary>
    public interface IDataGenerator
    {
        /// <summary>Table filled by this generator, e.g. "dbo.Books".</summary>
        string TableName { get; }

        /// <summary>Tables that must already have data (foreign keys of this table).</summary>
        IReadOnlyList<string> DependsOn { get; }

        bool HasData();

        /// <summary>Inserts the rows and returns how many were created.</summary>
        int Generate();
    }
}
