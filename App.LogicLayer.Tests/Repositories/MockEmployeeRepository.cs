using App.DataLayer.Entities;
using App.DataLayer.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.LogicLayer.Tests.Repositories
{
    public class MockEmployeeRepository
    {
        public Mock<IRepository<Employee>> repository;
        public List<Employee> Employees { get; }

        public MockEmployeeRepository()
        {
            repository = new Mock<IRepository<Employee>>();
            Employees = new List<Employee>
            {
                new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Ярослава",
                    LastName = "Ермишина",
                    MiddleName = "Филипповна",
                    Email = "ermishina@mail.com"
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Владилен",
                    LastName = "Пашин",
                    MiddleName = "Елизарович",
                    Email = "pashin@mail.com"
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Артем",
                    LastName = "Яманов",
                    MiddleName = "Прохорович ",
                    Email = "yamanov@mail.com"
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Оксана",
                    LastName = "Лютова",
                    MiddleName = "Родионовна",
                    Email = "lutova@mail.com"
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Адам",
                    LastName = "Абабков",
                    MiddleName = "Назарович",
                    Email = "ababkov@mail.com"
                }
            };

            repository
                .Setup(r => r.Create(It.IsAny<Employee>()))
                .Callback<Employee>(Add);
            repository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<Employee, bool>>()))
                .Returns((Func<Employee, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.Update(It.IsAny<Employee>()))
                .Callback((Employee employee) => Update(employee));
            repository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
        }

        public void Add(Employee employee)
        {
            Employees.Add(employee);
        }

        public Employee Get(Guid id)
        {
            return Employees.Where(e => e.Id == id).FirstOrDefault();
        }

        public IQueryable<Employee> GetAll()
        {
            return Employees.AsQueryable();
        }

        public IEnumerable<Employee> Find(Func<Employee, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public void Update(Employee data)
        {
            var employee = Get(data.Id);
            employee.FirstName = data.FirstName;
            employee.LastName = data.LastName;
            employee.MiddleName = data.MiddleName;
            employee.Email = data.Email;

            employee.Projects = data.Projects;
        }

        public void Delete(Guid id)
        {
            Employee employee = Get(id);
            if (employee != null)
                Employees.Remove(employee);
        }

        public void RemoveLastEmployee()
        {
            Employees.Remove(Employees.Last());
        }
    }
}
