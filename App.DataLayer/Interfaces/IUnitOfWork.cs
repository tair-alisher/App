using App.DataLayer.Entities;
using System;
using System.Threading.Tasks;

namespace App.DataLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Employee> Employees { get; }
        IRepository<Project> Projects { get; }

        Task SaveAsync();
        void Save();
    }
}
