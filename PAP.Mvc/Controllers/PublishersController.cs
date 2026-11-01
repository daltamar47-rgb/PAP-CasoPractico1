using PAP.Business;
using PAP.DataAccess;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Publishers.
    /// </summary>
    public class PublishersController : CrudController<Publisher>
    {
        public PublishersController()
            : base(new PublisherManager(), "Editoriales", "editorial")
        {
        }
    }
}
