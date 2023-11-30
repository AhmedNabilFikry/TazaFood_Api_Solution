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
        Task<IEnumerable<T>> GetAllASync();
        // GetByID Method 
        Task<T> GetByIDAsync(int ID);
        Task<IEnumerable<T>> GetAllWithSpecASync(ISpecification<T> spec);
        Task<T> GetByIDWithSpecAsync(ISpecification<T> spec);
    }
}
