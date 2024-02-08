using GatesVillaAPI.DataAcess.Repository.IRepo;
using GatesVillaAPI.Models.Models.MyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.DataAcess.Repo.IRepo
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        void Update (VillaNumber villaNumber);
    }
}
