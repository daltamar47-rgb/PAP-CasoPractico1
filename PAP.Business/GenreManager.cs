using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for géneros (dbo.Genres).
    /// </summary>
    public class GenreManager : ManagerBase<Genre>
    {
        public GenreManager()
            : base(new GenreRepository())
        {
        }
    }
}
