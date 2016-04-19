using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Repository
{
    public class CRUDRepository<T,TDb> : ICRUDRepository<T> 
        where T : class
        where TDb : DbContext, new()
    {
        protected TDb Db;
        public CRUDRepository(TDb entities)
        {
            Db = entities;
        }

        public void Create(T item)
        {
            Db.Entry(item).State = EntityState.Added;
            Db.SaveChanges();
        }

        public void Update(T item)
        {
            Db.Entry(item).State = EntityState.Modified;
            Db.SaveChanges();
        }

        public void Delete(T item)
        {
            Db.Entry(item).State = EntityState.Deleted;
            Db.SaveChanges();
        }

        //public Type GetTypeObject()
        //{
        //    return typeof (T);
        //}

        public IQueryable<T> GetSortList(Expression<Func<T, bool>> predicate)
        {
            return Db.Set<T>().Where(predicate);
        }

        public IEnumerable<T> GetAllList()
        {
            return Db.Set<T>();
        }

        public T GetItem(params object[] keyValue)
        {
            return Db.Set<T>().Find(keyValue);
        }

        public T GetItem(Expression<Func<T, bool>> predicate)
        {
            return Db.Set<T>().SingleOrDefault(predicate);
        }
    }
}
