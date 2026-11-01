using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.CartItems — depends on dbo.ShoppingCarts and dbo.Books.
    /// A book appears only once per cart (unique CartId + BookId).
    /// </summary>
    public class CartItemsGenerator : GeneratorBase<CartItem>
    {
        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.ShoppingCarts, Tables.Books };

        public CartItemsGenerator()
            : base(new CartItemRepository())
        {
        }

        public override string TableName => Tables.CartItems;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<CartItem> Build()
        {
            var cartIds = IdsOf(new ShoppingCartRepository(), cart => cart.Id);
            var bookIds = IdsOf(new BookRepository(), book => book.Id);

            foreach (var cartId in cartIds)
            {
                foreach (var bookId in Data.PickMany(bookIds, Data.Between(1, 3)))
                {
                    yield return new CartItem
                    {
                        CartId = cartId,
                        BookId = bookId,
                        Quantity = Data.Between(1, 2),
                        AddedAt = Data.RecentDate(30)
                    };
                }
            }
        }
    }
}
