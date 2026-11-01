using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Employees.
    /// </summary>
    public class EmployeesController : CrudController<Employee>
    {
        public EmployeesController()
            : base(new EmployeeManager(), "Empleados", "empleado")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "RoleId", Lookups.Roles() }
            };
        }
    }
}
