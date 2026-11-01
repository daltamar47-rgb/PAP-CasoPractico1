using PAP.Business;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PAP.Mvc.Infrastructure
{
    /// <summary>
    /// Option lists for the foreign key dropdowns (shows names instead of ids).
    /// </summary>
    public class LookupProvider
    {
        public IList<SelectListItem> Addresses()
        {
            return ToOptions(new AddressManager().GetAll(), item => item.Id, item => string.Format("{0}, {1} (cliente #{2})", item.Street, item.City, item.CustomerId));
        }

        public IList<SelectListItem> Authors()
        {
            return ToOptions(new AuthorManager().GetAll(), item => item.Id, item => item.FirstName + " " + item.LastName);
        }

        public IList<SelectListItem> Books()
        {
            return ToOptions(new BookManager().GetAll(), item => item.Id, item => item.Title);
        }

        public IList<SelectListItem> Categories()
        {
            return ToOptions(new CategoryManager().GetAll(), item => item.Id, item => item.Name);
        }

        public IList<SelectListItem> Customers()
        {
            return ToOptions(new CustomerManager().GetAll(), item => item.Id, item => item.FirstName + " " + item.LastName);
        }

        public IList<SelectListItem> Genres()
        {
            return ToOptions(new GenreManager().GetAll(), item => item.Id, item => item.Name);
        }

        public IList<SelectListItem> Orders()
        {
            return ToOptions(new OrderManager().GetAll(), item => item.Id, item => "Orden #" + item.Id);
        }

        public IList<SelectListItem> Publishers()
        {
            return ToOptions(new PublisherManager().GetAll(), item => item.Id, item => item.Name);
        }

        public IList<SelectListItem> PurchaseOrders()
        {
            return ToOptions(new PurchaseOrderManager().GetAll(), item => item.Id, item => "OC #" + item.Id);
        }

        public IList<SelectListItem> Roles()
        {
            return ToOptions(new RoleManager().GetAll(), item => item.Id, item => item.Name);
        }

        public IList<SelectListItem> ShoppingCarts()
        {
            return ToOptions(new ShoppingCartManager().GetAll(), item => item.Id, item => string.Format("Carrito #{0} (cliente #{1})", item.Id, item.CustomerId));
        }

        public IList<SelectListItem> Suppliers()
        {
            return ToOptions(new SupplierManager().GetAll(), item => item.Id, item => item.Name);
        }

        private static IList<SelectListItem> ToOptions<T>(IEnumerable<T> items, System.Func<T, int> getId, System.Func<T, string> getText)
        {
            return items
                .Select(item => new SelectListItem { Value = getId(item).ToString(), Text = getText(item) })
                .OrderBy(option => option.Text)
                .ToList();
        }
    }
}
