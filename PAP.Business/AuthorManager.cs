using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for autores (dbo.Authors).
    /// </summary>
    public class AuthorManager : ManagerBase<Author>
    {
        public AuthorManager()
            : base(new AuthorRepository())
        {
        }
    }
}
