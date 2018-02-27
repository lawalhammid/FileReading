using TechReconWindowService.DAL;
using TechReconWindowService.DAL.Interfaces;

namespace TechReconWindowService.DAL.Implementation
{
    public class DbFactory : Disposable, IDbFactory
    {
        TechReconContext dbContext;

        public TechReconContext Init()
        {
            return dbContext ?? (dbContext = new TechReconContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }


    }
}
