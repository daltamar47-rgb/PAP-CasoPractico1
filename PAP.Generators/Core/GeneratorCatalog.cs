using PAP.Generators.Generators;
using System.Collections.Generic;

namespace PAP.Generators.Core
{
    /// <summary>
    /// All the generators, one per table, in the order shown on the Admin page.
    /// </summary>
    public static class GeneratorCatalog
    {
        public static IReadOnlyList<IDataGenerator> CreateAll()
        {
            return new List<IDataGenerator>
            {
                new AddressesGenerator(),
                new AuthorsGenerator(),
                new BookAuthorsGenerator(),
                new BooksGenerator(),
                new CartItemsGenerator(),
                new CategoriesGenerator(),
                new CustomersGenerator(),
                new EmployeesGenerator(),
                new GenresGenerator(),
                new InventoryGenerator(),
                new OrderItemsGenerator(),
                new OrdersGenerator(),
                new PaymentsGenerator(),
                new PublishersGenerator(),
                new PurchaseOrderItemsGenerator(),
                new PurchaseOrdersGenerator(),
                new ReviewsGenerator(),
                new RolesGenerator(),
                new ShoppingCartsGenerator(),
                new SuppliersGenerator()
            };
        }
    }
}
