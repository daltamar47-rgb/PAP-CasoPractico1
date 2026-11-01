using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for employees (dbo.Employees).
    /// </summary>
    public class EmployeeManager : ManagerBase<Employee>
    {
        public EmployeeManager()
            : base(new EmployeeRepository())
        {
        }

        protected override void PrepareIncoming(Employee employee, bool isNew)
        {
            employee.Email = ContactRules.NormalizeEmail(employee.Email);
            employee.PasswordHash = ContactRules.HashTypedPassword(employee.PasswordHash, isNew);
        }
    }
}
