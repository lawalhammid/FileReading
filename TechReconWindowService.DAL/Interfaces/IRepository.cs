using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TechReconWindowService.DAL.Interfaces
{
    public interface IRepository<T>  where T : class
    {
        void Add(T entity);
        void  AddAsync(Task<T> entity);
        void Update(T entity);
       // void UpdateAsync(Task<T> entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        Task<T> GetById(object id);
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        Task<T> GetAsync(Expression<Func<T, bool>> where);     
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);
        //Task<int> SaveAsync(T entity, string userid);
       
    }
}
