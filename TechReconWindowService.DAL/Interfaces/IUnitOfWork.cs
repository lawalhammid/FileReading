using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> Commit(int userid, string CompanyAccountNo);
        int CommitNonAsync(int userid, string CompanyAccountNo);

    }
}
