namespace PAP.Mvc.Models
{
    /// <summary>
    /// Values posted by the BookAuthors form. The "Original" ids identify the relation being edited
    /// (both are 0 when a new relation is created).
    /// </summary>
    public class BookAuthorForm
    {
        public int OriginalBookId { get; set; }

        public int OriginalAuthorId { get; set; }

        public int BookId { get; set; }

        public int AuthorId { get; set; }

        public bool IsNew => OriginalBookId == 0 && OriginalAuthorId == 0;
    }
}
