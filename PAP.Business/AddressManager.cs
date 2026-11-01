using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for customer addresses (dbo.Addresses).
    /// </summary>
    public class AddressManager : ManagerBase<Address>
    {
        private const string DefaultAddressType = "Shipping";

        public AddressManager()
            : base(new AddressRepository())
        {
        }

        protected override void ApplyDefaults(Address address)
        {
            if (string.IsNullOrWhiteSpace(address.AddressType))
            {
                address.AddressType = DefaultAddressType;
            }
        }
    }
}
