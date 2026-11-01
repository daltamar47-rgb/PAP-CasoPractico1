using PAP.Business;
using PAP.DataAccess;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Suppliers.
    /// </summary>
    public class SuppliersController : CrudController<Supplier>
    {
        public SuppliersController()
            : base(new SupplierManager(), "Proveedores", "proveedor")
        {
        }
    }
}
