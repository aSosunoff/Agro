using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Model.Engine.Repository.Interface
{
    public interface ICRUDRepository <T> where T : class
    {
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        IQueryable<T> GetSortList(Expression<Func<T, bool>> predicate);
        //http://oxozle.com/2015/04/01/kollekcii-v-net-ienumerable-iqueryable-icollection-ilist
        //http://metanit.com/sharp/entityframework/1.4.php
        IEnumerable<T> GetAllList();
        T GetItem(params object[] keyValye);
        T GetItem(Expression<Func<T, bool>> predicate);
    }
}