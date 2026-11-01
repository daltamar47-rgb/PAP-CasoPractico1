using PAP.Business.Core;
using PAP.DataAccess.Entities;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for the book-author relation (dbo.BookAuthors).
    /// </summary>
    public class BookAuthorManager
    {
        private readonly IBookAuthorRepository _repository;

        public BookAuthorManager()
        {
            _repository = new BookAuthorRepository();
        }

        public IEnumerable<BookAuthorLink> GetAll()
        {
            return _repository.GetAll();
        }

        public void Create(int bookId, int authorId)
        {
            EnsureNotAssigned(bookId, authorId);
            Execute(() => _repository.Add(bookId, authorId));
        }

        public void Update(int originalBookId, int originalAuthorId, int bookId, int authorId)
        {
            if (originalBookId == bookId && originalAuthorId == authorId)
            {
                return;
            }

            EnsureNotAssigned(bookId, authorId);
            Execute(() =>
            {
                _repository.Add(bookId, authorId);
                _repository.Delete(originalBookId, originalAuthorId);
            });
        }

        public void Delete(int bookId, int authorId)
        {
            Execute(() => _repository.Delete(bookId, authorId));
        }

        private void EnsureNotAssigned(int bookId, int authorId)
        {
            if (bookId <= 0 || authorId <= 0)
            {
                throw new BusinessException("Debe seleccionar un libro y un autor.");
            }

            if (_repository.Exists(bookId, authorId))
            {
                throw new BusinessException("Ese autor ya está asignado a ese libro.");
            }
        }

        private static void Execute(Action operation)
        {
            try
            {
                operation();
            }
            catch (KeyNotFoundException ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
            catch (DbUpdateException ex)
            {
                throw new BusinessException(DataErrorTranslator.Translate(ex), ex);
            }
        }
    }
}
