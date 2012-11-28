using System;
using System.Collections.Generic;
using System.Linq;

namespace porder.data.contract
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
        IEnumerable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> expression);
    }
}