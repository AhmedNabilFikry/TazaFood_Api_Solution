using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Core.IRepository
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetByName(string Name);

    }
}
