using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TazaFood.Core.Services
{
    public interface IFileService
    {
        Task DeleteIamge(string imagePath);
    }
}
