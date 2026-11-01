using PAP.Business;
using PAP.DataAccess;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Customers.
    /// </summary>
    public class CustomersController : CrudController<Customer>
    {
        public CustomersController()
            : base(new CustomerManager(), "Clientes", "cliente")
        {
        }
    }
}
