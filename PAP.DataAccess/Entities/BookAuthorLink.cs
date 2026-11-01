namespace PAP.DataAccess.Entities
{
    /// <summary>
    /// One row of dbo.BookAuthors.
    /// Entity Framework maps that table as a many-to-many relation (Book.Authors / Author.Books)
    /// and does not generate a class for it, so this class represents each relation.
    /// </summary>
    public class BookAuthorLink
    {
        public int BookId { get; set; }

        public int AuthorId { get; set; }

        public string BookTitle { get; set; }

        public string AuthorName { get; set; }
    }
}
