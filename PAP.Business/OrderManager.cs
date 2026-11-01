using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for customer orders (dbo.Orders).
    /// </summary>
    public class OrderManager : ManagerBase<Order>
    {
        private const string DefaultStatus = "Pending";

        private readonly IRepositoryBase<Address> _addressRepository;

        public OrderManager()
            : base(new OrderRepository())
        {
            _addressRepository = new AddressRepository();
        }

        protected override void ApplyDefaults(Order order)
        {
            if (string.IsNullOrWhiteSpace(order.Status))
            {
                order.Status = DefaultStatus;
            }

            order.Total = order.Subtotal + order.TaxAmount + order.ShippingCost;
        }

        protected override void Validate(Order order)
        {
            AmountRules.RequireNotNegative(order.Subtotal, "El subtotal");
            AmountRules.RequireNotNegative(order.TaxAmount, "El impuesto");
            AmountRules.RequireNotNegative(order.ShippingCost, "El costo de envío");

            if (order.ShippingAddressId.HasValue)
            {
                var address = _addressRepository.GetById(order.ShippingAddressId.Value);
                if (address == null || address.CustomerId != order.CustomerId)
                {
                    throw new BusinessException("La dirección de envío debe pertenecer al cliente de la orden.");
                }
            }
        }
    }
}
