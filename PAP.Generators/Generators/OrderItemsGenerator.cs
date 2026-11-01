using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.OrderItems — depends on dbo.Orders and dbo.Books. Each order gets 1 to 4 different books,
    /// then the subtotal, tax (13%), shipping and total of every order are recalculated.
    /// </summary>
    public class OrderItemsGenerator : GeneratorBase<OrderItem>
    {
        private const decimal TaxRate = 0.13m;
        private const decimal ShippingCost = 3.50m;
        private const decimal DiscountRate = 0.10m;

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Orders, Tables.Books };

        public OrderItemsGenerator()
            : base(new OrderItemRepository())
        {
        }

        public override string TableName => Tables.OrderItems;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        public override int Generate()
        {
            var count = base.Generate();
            UpdateOrderTotals();
            return count;
        }

        protected override IEnumerable<OrderItem> Build()
        {
            var orderIds = IdsOf(new OrderRepository(), order => order.Id);
            var books = new BookRepository().GetAll().ToList();

            foreach (var orderId in orderIds)
            {
                foreach (var book in Data.PickMany(books, Data.Between(1, 4)))
                {
                    var quantity = Data.Between(1, 3);
                    var hasDiscount = Data.Chance(20);
                    yield return new OrderItem
                    {
                        OrderId = orderId,
                        BookId = book.Id,
                        Quantity = quantity,
                        UnitPrice = book.Price,
                        Discount = hasDiscount ? decimal.Round(book.Price * quantity * DiscountRate, 2) : 0
                    };
                }
            }
        }

        private void UpdateOrderTotals()
        {
            var itemsByOrder = Repository.GetAll().ToLookup(item => item.OrderId);
            var orderRepository = new OrderRepository();

            foreach (var order in orderRepository.GetAll())
            {
                order.Subtotal = itemsByOrder[order.Id].Sum(item => item.UnitPrice * item.Quantity - item.Discount);
                order.TaxAmount = decimal.Round(order.Subtotal * TaxRate, 2);
                order.ShippingCost = ShippingCost;
                order.Total = order.Subtotal + order.TaxAmount + order.ShippingCost;
            }

            orderRepository.Save();
        }
    }
}
