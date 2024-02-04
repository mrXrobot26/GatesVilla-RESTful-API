using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.DataAcess.Repository.IRepo
{
    public interface IRepository<T> where T : class
    {
        T Get(Expression<Func<T, bool>> filter = null, string? includes = null);
        IEnumerable<T> GetAll(Expression<Func<T,bool>>filter=null , string? includes =null );
        void Add(T entity);
        void Delete(T entity);


    }
}
