using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for roles (dbo.Roles).
    /// </summary>
    public class RoleManager : ManagerBase<Role>
    {
        public RoleManager()
            : base(new RoleRepository())
        {
        }
    }
}
