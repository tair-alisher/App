using App.DataLayer.EF;
using App.DataLayer.Entities;
using App.DataLayer.Interfaces;
using System;
using System.Threading.Tasks;

namespace App.DataLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private BaseRepository<Employee> employeeRepository;
        private BaseRepository<Project> projectRepository;
        private ProjectContext context;

        public UnitOfWork(string connectionString)
        {
            context = new ProjectContext(connectionString);
        }

        public IRepository<Employee> Employees
        {
            get
            {
                if (employeeRepository == null)
                    employeeRepository = new BaseRepository<Employee>(context);
                return employeeRepository;
            }
        }

        public IRepository<Project> Projects
        {
            get
            {
                if (projectRepository == null)
                    projectRepository = new BaseRepository<Project>(context);
                return projectRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
