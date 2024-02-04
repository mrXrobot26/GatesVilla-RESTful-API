using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.DataAcess.Repo.IRepo
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        void SaveChanges();
    }
}
