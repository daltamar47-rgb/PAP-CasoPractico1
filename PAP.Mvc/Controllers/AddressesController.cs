using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Addresses.
    /// </summary>
    public class AddressesController : CrudController<Address>
    {
        public AddressesController()
            : base(new AddressManager(), "Direcciones", "dirección")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "CustomerId", Lookups.Customers() }
            };
        }
    }
}
