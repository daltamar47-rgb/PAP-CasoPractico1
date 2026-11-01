using PAP.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business.Core
{
    /// <summary>
    /// Fills the date columns the user does not type (CreatedAt, UpdatedAt, AddedAt...).
    /// Entity Framework does not apply the SQL DEFAULT values, so they are set here.
    /// </summary>
    public static class AuditStamp
    {
        public static void OnCreate(object entity, IEnumerable<EntityField> fields)
        {
            var now = DateTime.Now;
            foreach (var field in fields.Where(field => field.ClrType == typeof(DateTime) && !field.IsNullable))
            {
                var isEmpty = (DateTime)field.GetValue(entity) == default(DateTime);
                if (field.IsAudit || isEmpty)
                {
                    field.SetValue(entity, now);
                }
            }
        }

        public static void OnUpdate(object entity, IEnumerable<EntityField> fields)
        {
            var updatedAt = fields.FirstOrDefault(field => field.Name == "UpdatedAt");
            if (updatedAt != null)
            {
                updatedAt.SetValue(entity, DateTime.Now);
            }
        }
    }
}
