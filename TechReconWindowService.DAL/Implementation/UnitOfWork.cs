using System.Threading.Tasks;
using TechReconWindowService.DAL;
using TechReconWindowService.DAL.Interfaces;



namespace TechReconWindowService.DAL.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private TechReconContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public TechReconContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }
        public async Task<int> Commit(int userid, string CompanyAccountNo)
        {
            return await DbContext.Commit(userid, CompanyAccountNo);
        }
        public int CommitNonAsync(int userid, string CompanyAccountNo)
        {
            return DbContext.CommitNonAsync(userid, CompanyAccountNo);
        }
    }
}
