using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repository.IRepo;
using Microsoft.EntityFrameworkCore;

namespace GatesVillaAPI.DataAcess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.DbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>> filter = null, string? includes = null)
        {
            IQueryable<T> quary = DbSet.Where(filter);
            if (includes!=null)
            {
                foreach (var include in includes.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    quary=quary.Include(include);
                }
            }
            return quary.FirstOrDefault();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string? includes = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includes!= null)
            {
                foreach (var include in includes.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }
            return query.ToList();
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }
}
