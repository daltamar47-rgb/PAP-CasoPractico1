using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for editoriales (dbo.Publishers).
    /// </summary>
    public class PublisherManager : ManagerBase<Publisher>
    {
        public PublisherManager()
            : base(new PublisherRepository())
        {
        }
    }
}
