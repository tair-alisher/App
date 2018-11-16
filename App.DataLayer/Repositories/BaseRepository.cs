using App.DataLayer.EF;
using App.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace App.DataLayer.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private ProjectContext Context;
        private DbSet<T> DbSet;

        public BaseRepository(ProjectContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public void Create(T item)
        {
            DbSet.Add(item);
        }

        public void Delete(Guid id)
        {
            T entity = DbSet.Find(id);
            if (entity != null)
                DbSet.Remove(entity);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T Get(Guid? id)
        {
            return DbSet.Find(id);
        }

        public void Update(T item)
        {
            var entityEntry = Context.Entry(item);
            if (entityEntry.State == EntityState.Detached)
                DbSet.Attach(item);
            entityEntry.State = EntityState.Modified;
        }
    }
}
