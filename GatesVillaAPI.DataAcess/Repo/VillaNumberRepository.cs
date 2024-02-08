using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.DataAcess.Repository;
using GatesVillaAPI.Models.Models.MyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.DataAcess.Repo
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext db;

        public VillaNumberRepository(ApplicationDbContext db) :base(db) 
        {
            this.db = db;
        }
        public async Task UpdateAsync(VillaNumber villaNumber)
        {
            db.villaNumbers.Update(villaNumber);
            await db.SaveChangesAsync();
        }
    }
}
