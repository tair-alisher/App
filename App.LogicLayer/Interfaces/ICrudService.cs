using System;
using System.Collections.Generic;

namespace App.LogicLayer.Interfaces
{
    public interface ICrudService<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Add(T item);
        T Get(Guid? id);
        void Update(T item);
        void Delete(Guid id);
        void Dispose();
    }
}
