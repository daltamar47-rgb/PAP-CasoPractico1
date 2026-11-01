using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Core
{
    /// <summary>
    /// State of one table on the Admin page: whether it has data and whether it can be generated now.
    /// </summary>
    public class GeneratorStatus
    {
        public GeneratorStatus(string tableName, bool hasData, IReadOnlyList<string> missingDependencies)
        {
            TableName = tableName;
            HasData = hasData;
            MissingDependencies = missingDependencies;
        }

        public string TableName { get; }

        public bool HasData { get; }

        /// <summary>Tables this one depends on that are still empty.</summary>
        public IReadOnlyList<string> MissingDependencies { get; }

        public bool CanGenerate => !HasData && !MissingDependencies.Any();

        public string Description
        {
            get
            {
                if (HasData)
                {
                    return "Ya tiene datos";
                }

                return MissingDependencies.Any()
                    ? "Primero genere: " + string.Join(", ", MissingDependencies)
                    : "Lista para generar";
            }
        }
    }
}
