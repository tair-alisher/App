using App.DataLayer.Entities;
using App.DataLayer.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.LogicLayer.Tests.Repositories
{
    public class MockProjectRepository
    {
        public Mock<IRepository<Project>> repository;
        public List<Project> Projects { get; }

        public MockProjectRepository(List<Employee> Employees)
        {
            repository = new Mock<IRepository<Project>>();
            Projects = new List<Project>
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Title = "Постройка многоэтажного дома",
                    Customer = "Люди",
                    Performer = "МногоЭтажДомСтрой",
                    Priority = 3,
                    Comment = "Построить",
                    DateStart = DateTime.Today.AddYears(-1),
                    DateEnd = DateTime.Today.AddYears(2),
                    CreatedAt = DateTime.Today,
                    ManagerId = Employees.First().Id
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Title = "Недельный субботник",
                    Customer = "Домком",
                    Performer = "Жильцы",
                    Priority = 8,
                    Comment = "Убраться на территории",
                    DateStart = DateTime.Today.AddDays(-3),
                    DateEnd = DateTime.Today.AddDays(4),
                    CreatedAt = DateTime.Today.AddDays(-1),
                    ManagerId = null
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Title = "Ремонт авто",
                    Customer = "Клиент",
                    Performer = "СТО",
                    Priority = 3,
                    Comment = "В кратчайшие сроки",
                    DateStart = DateTime.Today.AddDays(1),
                    DateEnd = DateTime.Today.AddDays(2),
                    CreatedAt = DateTime.Now,
                    ManagerId = null
                }
            };

            repository
                .Setup(r => r.Create(It.IsAny<Project>()))
                .Callback<Project>(Add);
            repository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Get(id));
            repository
                .Setup(r => r.GetAll())
                .Returns(GetAll());
            repository
                .Setup(r => r.Find(It.IsAny<Func<Project, bool>>()))
                .Returns((Func<Project, bool> predicate) => Find(predicate));
            repository
                .Setup(r => r.Update(It.IsAny<Project>()))
                .Callback((Project project) => Update(project));
            repository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
        }

        public void Add(Project employee)
        {
            Projects.Add(employee);
        }

        public Project Get(Guid id)
        {
            return Projects.Where(e => e.Id == id).FirstOrDefault();
        }

        public IQueryable<Project> GetAll()
        {
            return Projects.AsQueryable();
        }

        public IEnumerable<Project> Find(Func<Project, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public void Update(Project data)
        {
            var project = Get(data.Id);
            project.Title = data.Title;
            project.Customer = data.Customer;
            project.Performer = data.Performer;
            project.Priority = data.Priority;
            project.Comment = data.Comment;
            project.DateStart = data.DateStart;
            project.DateEnd = data.DateEnd;
            project.CreatedAt = data.CreatedAt;
            project.ManagerId = data.ManagerId;

            project.Manager = data.Manager;

            project.Employees = data.Employees;
        }

        public void Delete(Guid id)
        {
            Project project = Get(id);
            if (project != null)
                Projects.Remove(project);
        }

        public void RemoveLastProject()
        {
            Projects.Remove(Projects.Last());
        }
    }
}
