using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.IRepository;
using TazaFood.Core.Models;
using TazaFood.Repository.Context;

namespace TazaFood.Repository.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TazaDbContext _context;

        public CategoryRepository(TazaDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetByName(string Name)
        {
            return await _context.Set<Category>().Where(C => C.Name.Contains(Name)).ToListAsync();
        }
    }
}
