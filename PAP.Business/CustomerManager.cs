using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for customers (dbo.Customers).
    /// </summary>
    public class CustomerManager : ManagerBase<Customer>
    {
        public CustomerManager()
            : base(new CustomerRepository())
        {
        }

        protected override void PrepareIncoming(Customer customer, bool isNew)
        {
            customer.Email = ContactRules.NormalizeEmail(customer.Email);
            customer.PasswordHash = ContactRules.HashTypedPassword(customer.PasswordHash, isNew);
        }
    }
}
