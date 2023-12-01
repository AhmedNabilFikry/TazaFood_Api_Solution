using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazaFood.Core.Models;

namespace TazaFood.Core.Specification
{
    public class CategorySpecification :BaseSpecification<Category>
    {
        public CategorySpecification( int ID) :base(Cr => Cr.ID == ID)
        {
        }
    }
}
