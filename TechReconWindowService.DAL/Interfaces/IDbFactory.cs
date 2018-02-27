using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechReconWindowService.DAL;

namespace TechReconWindowService.DAL.Interfaces
{
    public interface IDbFactory : IDisposable
    {
        TechReconContext Init();
    }
}
