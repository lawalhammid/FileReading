﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReconWindowService.DAL;
using TechReconWindowService.DAL.Implementation;
using TechReconWindowService.DAL.Interfaces;

namespace TechReconWindowService.Repository.Repositories
{
    public class admDataPoollingControlRepository : Repository<admDataPullingControl>, IadmDataPoollingControlRepository
    {
        public admDataPoollingControlRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IadmDataPoollingControlRepository : IRepository<admDataPullingControl>
    {

    }
}