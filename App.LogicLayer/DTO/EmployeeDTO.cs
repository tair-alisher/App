using System;
using System.Collections.Generic;

namespace App.LogicLayer.DTO
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public ICollection<ProjectDTO> Projects { get; set; }
    }
}
