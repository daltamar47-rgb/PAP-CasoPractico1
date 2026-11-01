using PAP.Business;
using PAP.DataAccess;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Roles.
    /// </summary>
    public class RolesController : CrudController<Role>
    {
        public RolesController()
            : base(new RoleManager(), "Roles", "rol")
        {
        }
    }
}
