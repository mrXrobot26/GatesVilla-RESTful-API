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
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext db;
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            db.villas.Update(entity);
        }
    }
}
