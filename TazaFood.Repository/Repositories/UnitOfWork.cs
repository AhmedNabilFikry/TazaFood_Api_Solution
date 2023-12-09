using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Repository.Context;

namespace TazaFood.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TazaDbContext _dbContext;
        private Hashtable repositories;
        public UnitOfWork( TazaDbContext dbContext)
        {
            _dbContext = dbContext;
            repositories = new Hashtable();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel
        {
            var Type = typeof(TEntity).Name; //Product
            if (!repositories.ContainsKey(Type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                repositories.Add(Type,repository);
            }
            return repositories[Type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
    }
}
