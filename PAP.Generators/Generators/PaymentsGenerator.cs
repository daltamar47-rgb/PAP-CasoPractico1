using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Payments — depends on dbo.Orders and dbo.OrderItems (so the order totals are already calculated).
    /// Every order that is not cancelled gets one payment for its total.
    /// </summary>
    public class PaymentsGenerator : GeneratorBase<Payment>
    {
        private const string CancelledStatus = "Cancelled";

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Orders, Tables.OrderItems };

        private static readonly string[] Methods = { "Tarjeta", "SINPE Móvil", "Transferencia", "Efectivo" };

        public PaymentsGenerator()
            : base(new PaymentRepository())
        {
        }

        public override string TableName => Tables.Payments;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<Payment> Build()
        {
            var orders = new OrderRepository().GetAll()
                .Where(order => order.Status != CancelledStatus && order.Total > 0)
                .ToList();

            foreach (var order in orders)
            {
                yield return new Payment
                {
                    OrderId = order.Id,
                    PaymentMethod = Data.Pick(Methods),
                    Amount = order.Total,
                    PaymentDate = order.OrderDate.AddMinutes(Data.Between(1, 120)),
                    Status = "Completed",
                    TransactionRef = "TRX-" + order.Id.ToString("00000") + "-" + Data.Between(1000, 9999)
                };
            }
        }
    }
}
