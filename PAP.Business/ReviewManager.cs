using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for book reviews (dbo.Reviews).
    /// </summary>
    public class ReviewManager : ManagerBase<Review>
    {
        private const int MinRating = 1;
        private const int MaxRating = 5;

        public ReviewManager()
            : base(new ReviewRepository())
        {
        }

        protected override void Validate(Review review)
        {
            if (review.Rating < MinRating || review.Rating > MaxRating)
            {
                throw new BusinessException("La calificación debe estar entre 1 y 5.");
            }
        }
    }
}
