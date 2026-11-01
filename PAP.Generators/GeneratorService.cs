using PAP.Generators.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    /// <summary>
    /// Entry point used by the Admin page: tells which tables can be generated
    /// and generates them while protecting the integrity of the foreign keys.
    /// Rules:
    /// 1. A table that already has data is never generated again.
    /// 2. A table can only be generated when every table it depends on has data,
    ///    so tables without dependencies are enabled first and the rest are enabled as data is added.
    /// </summary>
    public class GeneratorService
    {
        private readonly IReadOnlyList<IDataGenerator> _generators;

        public GeneratorService()
        {
            _generators = GeneratorCatalog.CreateAll();
        }

        public IReadOnlyList<GeneratorStatus> GetStatuses()
        {
            var tablesWithData = new HashSet<string>(_generators.Where(generator => generator.HasData()).Select(generator => generator.TableName));

            return _generators
                .Select(generator => new GeneratorStatus(
                    generator.TableName,
                    tablesWithData.Contains(generator.TableName),
                    generator.DependsOn.Where(table => !tablesWithData.Contains(table)).ToList()))
                .ToList();
        }

        public GenerationResult Generate(string tableName)
        {
            var generator = Find(tableName);
            if (generator == null)
            {
                return GenerationResult.Failure("La tabla indicada no existe.");
            }

            if (generator.HasData())
            {
                return GenerationResult.Failure(string.Format("La tabla {0} ya tiene datos. No se generó ningún registro.", tableName));
            }

            var missing = generator.DependsOn.Where(table => !Find(table).HasData()).ToList();
            if (missing.Any())
            {
                return GenerationResult.Failure(string.Format(
                    "No se puede generar {0} todavía. Primero genere: {1}.", tableName, string.Join(", ", missing)));
            }

            try
            {
                var count = generator.Generate();
                return GenerationResult.Success(string.Format("Se generaron {0} registros en {1}.", count, tableName));
            }
            catch (Exception ex)
            {
                return GenerationResult.Failure(string.Format("No se pudieron generar los datos de {0}: {1}", tableName, InnermostMessage(ex)));
            }
        }

        private IDataGenerator Find(string tableName)
        {
            return _generators.FirstOrDefault(generator => generator.TableName == tableName);
        }

        private static string InnermostMessage(Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            return exception.Message;
        }
    }
}
