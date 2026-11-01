using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

namespace PAP.Business.Core
{
    /// <summary>
    /// Converts Entity Framework and SQL Server errors into messages a user can understand.
    /// </summary>
    public static class DataErrorTranslator
    {
        private const int ConstraintConflict = 547;
        private const int UniqueConstraintViolation = 2627;
        private const int UniqueIndexViolation = 2601;
        private const int NullNotAllowed = 515;
        private const int TextTooLong = 8152;
        private const int TextTooLongDetailed = 2628;

        public static string Translate(DbEntityValidationException exception)
        {
            var messages = exception.EntityValidationErrors
                .SelectMany(result => result.ValidationErrors)
                .Select(error => string.Format("{0}: {1}", error.PropertyName, error.ErrorMessage));

            return "Revise los datos. " + string.Join(" ", messages);
        }

        public static string Translate(DbUpdateException exception)
        {
            var sqlError = FindSqlException(exception);
            if (sqlError == null)
            {
                return "No se pudo guardar la información en la base de datos.";
            }

            switch (sqlError.Number)
            {
                case ConstraintConflict:
                    return sqlError.Message.Contains("DELETE")
                        ? "No se puede eliminar: hay otros registros que dependen de este."
                        : "Un valor no es válido: el registro relacionado no existe o el valor está fuera del rango permitido.";
                case UniqueConstraintViolation:
                case UniqueIndexViolation:
                    return "Ya existe un registro con ese valor; este dato debe ser único.";
                case NullNotAllowed:
                    return "Falta un dato obligatorio.";
                case TextTooLong:
                case TextTooLongDetailed:
                    return "Uno de los textos supera el largo permitido.";
                default:
                    return sqlError.Message;
            }
        }

        private static SqlException FindSqlException(Exception exception)
        {
            var current = exception;
            while (current != null)
            {
                var sqlException = current as SqlException;
                if (sqlException != null)
                {
                    return sqlException;
                }

                current = current.InnerException;
            }

            return null;
        }
    }
}
