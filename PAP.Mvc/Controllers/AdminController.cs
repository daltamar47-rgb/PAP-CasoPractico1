using PAP.Generators;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Admin page: generates sample data for each table of BookStoreDB.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly GeneratorService _generatorService;

        public AdminController()
        {
            _generatorService = new GeneratorService();
        }

        public ActionResult Index()
        {
            return View(_generatorService.GetStatuses());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate(string tableName)
        {
            var result = _generatorService.Generate(tableName);
            TempData[result.Succeeded ? "Success" : "Error"] = result.Message;
            return RedirectToAction("Index");
        }
    }
}
