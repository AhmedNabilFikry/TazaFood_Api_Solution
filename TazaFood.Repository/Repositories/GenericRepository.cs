using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Core.Specification;
using TazaFood.Repository.Context;

namespace TazaFood.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly TazaDbContext _dbcontext;

        // Asking CLR To Create Obj From TazaDbContext Implicitly
        public GenericRepository(TazaDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllASync()
        {
            //if (typeof(T) == typeof(Product))
            //    return (IEnumerable<T>)await _dbcontext.Products.Include(P => P.Category).ToListAsync();
            //else
            return await _dbcontext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIDAsync(int ID)
            // searching Remotely 
            //=> await _dbcontext.Set<T>().Where(X => X.ID == ID).FirstOrDefaultAsync();
            => await _dbcontext.Set<T>().FindAsync(ID);


        public async Task<IReadOnlyList<T>> GetAllWithSpecASync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIDWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> Spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbcontext.Set<T>(), Spec);
        }

        public async Task<int> GetCountwithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task Add(T entity)
        {
            await _dbcontext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbcontext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
        }
    }
}
