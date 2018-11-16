using System;
using System.Collections.Generic;

namespace App.DataLayer.Entities
{
    public class Project
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Customer { get; set; }
        public string Performer { get; set; }

        public int Priority { get; set; }
        public string Comment { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? ManagerId { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public Project() {
            Employees = new List<Employee>();
        }
    }
}
