using App.DataLayer.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace App.DataLayer.EF
{
    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<ProjectContext>
    {
        protected override void Seed(ProjectContext context)
        {
            context.Employees.Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Ярослава",
                LastName = "Ермишина",
                MiddleName = "Филипповна",
                Email = "ermishina@mail.com"
            });
            context.Employees.Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Владилен",
                LastName = "Пашин",
                MiddleName = "Елизарович",
                Email = "pashin@mail.com"
            });
            context.Employees.Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Артем",
                LastName = "Яманов",
                MiddleName = "Прохорович ",
                Email = "yamanov@mail.com"
            });
            context.Employees.Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Оксана",
                LastName = "Лютова",
                MiddleName = "Родионовна",
                Email = "lutova@mail.com"
            });
            context.Employees.Add(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Адам",
                LastName = "Абабков",
                MiddleName = "Назарович",
                Email = "ababkov@mail.com"
            });

            context.SaveChanges();

            context.Projects.Add(new Project
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
                ManagerId = context.Employees.First().Id
            });
            context.Projects.Add(new Project
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
            });
            context.Projects.Add(new Project
            {
                Id = Guid.NewGuid(),
                Title = "Ремонт авто",
                Customer = "Клиент",
                Performer = "СТО",
                Priority = 5,
                Comment = "В кратчайшие сроки",
                DateStart = DateTime.Today.AddDays(1),
                DateEnd = DateTime.Today.AddDays(2),
                CreatedAt = DateTime.Now,
                ManagerId = context.Employees.Last().Id
            });

            context.SaveChanges();
        }
    }
}
