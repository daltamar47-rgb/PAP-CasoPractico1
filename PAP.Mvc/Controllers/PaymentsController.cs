using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Payments.
    /// </summary>
    public class PaymentsController : CrudController<Payment>
    {
        public PaymentsController()
            : base(new PaymentManager(), "Pagos", "pago")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "OrderId", Lookups.Orders() }
            };
        }
    }
}
