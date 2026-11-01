using PAP.Business;
using PAP.Business.Core;
using PAP.DataAccess.Entities;
using PAP.Mvc.Infrastructure;
using PAP.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.BookAuthors (which author wrote which book).
    /// The table has a composite key (BookId + AuthorId), so it does not use CrudController.
    /// </summary>
    public class BookAuthorsController : Controller
    {
        private const string ControllerName = "BookAuthors";

        private readonly BookAuthorManager _manager;
        private readonly LookupProvider _lookups;

        public BookAuthorsController()
        {
            _manager = new BookAuthorManager();
            _lookups = new LookupProvider();
        }

        public ActionResult Index()
        {
            var links = _manager.GetAll().ToList();
            return View(new CrudPageModel<BookAuthorLink>(ControllerName, "Autores por libro", "autor a un libro", links, BuildFields()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(BookAuthorForm form)
        {
            Run(
                () =>
                {
                    if (form.IsNew)
                    {
                        _manager.Create(form.BookId, form.AuthorId);
                    }
                    else
                    {
                        _manager.Update(form.OriginalBookId, form.OriginalAuthorId, form.BookId, form.AuthorId);
                    }
                },
                form.IsNew ? "Autor asignado correctamente." : "Relación actualizada correctamente.");

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int bookId, int authorId)
        {
            Run(() => _manager.Delete(bookId, authorId), "Relación eliminada correctamente.");
            return RedirectToAction("Index");
        }

        private IList<FormField> BuildFields()
        {
            return new List<FormField>
            {
                new FormField { Name = "OriginalBookId", Kind = FieldKind.Hidden, DefaultValue = "0", ShowInDetails = false },
                new FormField { Name = "OriginalAuthorId", Kind = FieldKind.Hidden, DefaultValue = "0", ShowInDetails = false },
                new FormField { Name = "BookId", Label = FieldLabels.For("BookId"), Kind = FieldKind.Select, Required = true, Options = _lookups.Books() },
                new FormField { Name = "AuthorId", Label = FieldLabels.For("AuthorId"), Kind = FieldKind.Select, Required = true, Options = _lookups.Authors() }
            };
        }

        private void Run(Action operation, string successMessage)
        {
            try
            {
                operation();
                TempData["Success"] = successMessage;
            }
            catch (BusinessException ex)
            {
                TempData["Error"] = ex.Message;
            }
        }
    }
}
