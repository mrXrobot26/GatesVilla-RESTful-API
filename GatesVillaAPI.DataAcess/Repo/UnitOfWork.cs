using AutoMapper;
using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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

        public IUserRepository User { get; private set; }


        private readonly ApplicationDbContext db;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public UnitOfWork(ApplicationDbContext db , IConfiguration configuration, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.configuration = configuration;
            this.mapper = mapper;
            this.userManager = userManager;
            Villa = new VillaRepository( db );
            VillaNumber = new VillaNumberRepository( db );
            User = new UserRepository(db , configuration,userManager,mapper );
        }

        public async Task SaveChangesAsync()
        {
            await db.SaveChangesAsync();
        }

    }
}
