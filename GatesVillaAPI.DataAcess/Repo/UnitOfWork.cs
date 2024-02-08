using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.DataAcess.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        public IVillaRepository Villa { get; private set; }

        public IVillaNumberRepository VillaNumber { get; private set; }

        private readonly ApplicationDbContext db;
        public UnitOfWork(ApplicationDbContext db )
        {
            this.db = db;
            Villa = new VillaRepository( db );
            VillaNumber = new VillaNumberRepository( db );
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

    }
}
