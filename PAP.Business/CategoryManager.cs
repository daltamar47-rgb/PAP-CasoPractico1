using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for categorías (dbo.Categories).
    /// </summary>
    public class CategoryManager : ManagerBase<Category>
    {
        public CategoryManager()
            : base(new CategoryRepository())
        {
        }
    }
}
