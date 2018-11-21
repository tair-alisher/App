using System;
using System.Collections.Generic;
using System.Linq;

namespace App.DataLayer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(Guid? id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(Guid id);
    }
}
