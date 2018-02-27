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
    public class VostroMT940950TransRepository : Repository<VostroMT940950Trans>, IVostroMT940950TransRepository
    {
        public VostroMT940950TransRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IVostroMT940950TransRepository : IRepository<VostroMT940950Trans>
    {

    }
}
