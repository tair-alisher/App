using System;
using System.Collections.Generic;

namespace App.DataLayer.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public Employee()
        {
            Projects = new List<Project>();
        }
    }
}
