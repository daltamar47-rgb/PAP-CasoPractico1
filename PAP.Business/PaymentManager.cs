using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for order payments (dbo.Payments).
    /// </summary>
    public class PaymentManager : ManagerBase<Payment>
    {
        private const string DefaultStatus = "Completed";

        public PaymentManager()
            : base(new PaymentRepository())
        {
        }

        protected override void ApplyDefaults(Payment payment)
        {
            if (string.IsNullOrWhiteSpace(payment.Status))
            {
                payment.Status = DefaultStatus;
            }
        }

        protected override void Validate(Payment payment)
        {
            AmountRules.RequirePositive(payment.Amount, "El monto");
        }
    }
}
