using PAP.Business;
using PAP.DataAccess;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Categories.
    /// </summary>
    public class CategoriesController : CrudController<Category>
    {
        public CategoriesController()
            : base(new CategoryManager(), "Categorías", "categoría")
        {
        }
    }
}
