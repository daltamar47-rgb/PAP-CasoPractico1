using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for proveedores (dbo.Suppliers).
    /// </summary>
    public class SupplierManager : ManagerBase<Supplier>
    {
        public SupplierManager()
            : base(new SupplierRepository())
        {
        }
    }
}
