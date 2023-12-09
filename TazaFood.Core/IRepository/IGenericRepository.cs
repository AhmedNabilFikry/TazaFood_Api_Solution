using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;
using TazaFood.Core.Specification;

namespace TazaFood.Core.IRepository
{
    public interface IGenericRepository<T> where T :BaseModel
    {
        //GetAll Products Method 
        Task<IReadOnlyList<T>> GetAllASync();
        // GetByID Method 
        Task<T> GetByIDAsync(int ID);
        Task<IReadOnlyList<T>> GetAllWithSpecASync(ISpecification<T> spec);
        Task<T> GetByIDWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountwithSpecAsync(ISpecification<T> spec);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
