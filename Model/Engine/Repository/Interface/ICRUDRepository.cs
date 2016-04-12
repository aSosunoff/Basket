using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Model.Engine.Repository.Interface
{
    public interface ICRUDRepository<T> where T : class
    {
        void Create(T item);
        IEnumerable<T> GetList();
        T GetItem(Expression<Func<T, bool>> predicate);
        void Update(T item);
        void Delete(T item);
    }
}