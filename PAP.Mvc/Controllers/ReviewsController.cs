using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Reviews.
    /// </summary>
    public class ReviewsController : CrudController<Review>
    {
        public ReviewsController()
            : base(new ReviewManager(), "Reseñas", "reseña")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "BookId", Lookups.Books() },
                { "CustomerId", Lookups.Customers() }
            };
        }
    }
}
