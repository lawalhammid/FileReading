using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReconWindowService.DAL;
using TechReconWindowService.DAL.Implementation;
using TechReconWindowService.DAL.Interfaces;

namespace TechReconWindowService.Repository.Repositories
{
    public class VostroMT940950TransHistoryRepository : Repository<VostroMT940950TransHistory>, IVostroMT940950TransHistoryRepository
    {
        public VostroMT940950TransHistoryRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IVostroMT940950TransHistoryRepository : IRepository<VostroMT940950TransHistory>
    {

    }
}
