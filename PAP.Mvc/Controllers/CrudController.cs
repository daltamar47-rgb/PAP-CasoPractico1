using PAP.Business.Core;
using PAP.Mvc.Infrastructure;
using PAP.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Base controller for the maintenance pages: one Index view with the table,
    /// and the Save / Delete actions used by its modals and buttons.
    /// </summary>
    public abstract class CrudController<T> : Controller where T : class
    {
        private const string SuccessKey = "Success";
        private const string ErrorKey = "Error";

        private readonly IManager<T> _manager;
        private readonly string _title;
        private readonly string _singular;

        protected CrudController(IManager<T> manager, string title, string singular)
        {
            _manager = manager;
            _title = title;
            _singular = singular;
            Lookups = new LookupProvider();
        }

        protected LookupProvider Lookups { get; }

        public ActionResult Index()
        {
            var fields = FormFieldFactory.Create(_manager.GetFields(), GetLookups());
            foreach (var field in fields)
            {
                ConfigureField(field);
            }

            var items = _manager.GetAll().ToList();
            var controllerName = GetType().Name.Replace("Controller", string.Empty);
            return View(new CrudPageModel<T>(controllerName, _title, _singular, items, fields));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(T entity)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorKey] = "Revise los datos: " + string.Join(" ", ModelStateErrors());
                return RedirectToAction("Index");
            }

            var isNew = _manager.GetId(entity) == 0;
            Run(
                () =>
                {
                    if (isNew)
                    {
                        _manager.Create(entity);
                    }
                    else
                    {
                        _manager.Update(entity);
                    }
                },
                isNew ? "Registro creado correctamente." : "Registro actualizado correctamente.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Foreign keys shown as dropdowns: column name → options.
        /// </summary>
        protected virtual IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>();
        }

        /// <summary>
        /// Lets a controller adjust a field (hide it, change its label...).
        /// </summary>
        protected virtual void ConfigureField(FormField field)
        {
        }

        private void Run(Action operation, string successMessage)
        {
            try
            {
                operation();
                TempData[SuccessKey] = successMessage;
            }
            catch (BusinessException ex)
            {
                TempData[ErrorKey] = ex.Message;
            }
        }

        private IEnumerable<string> ModelStateErrors()
        {
            return ModelState.Values
                .SelectMany(state => state.Errors)
                .Select(error => string.IsNullOrEmpty(error.ErrorMessage) ? "Hay un valor con formato inválido." : error.ErrorMessage);
        }
    }
}
