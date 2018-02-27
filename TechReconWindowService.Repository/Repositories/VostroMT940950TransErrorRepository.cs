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
    public class VostroMT940950TransErrorRepository : Repository<VostroMT940950TransError>, IVostroMT940950TransErrorRepository
    {
        public VostroMT940950TransErrorRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IVostroMT940950TransErrorRepository : IRepository<VostroMT940950TransError>
    {

    }
}
