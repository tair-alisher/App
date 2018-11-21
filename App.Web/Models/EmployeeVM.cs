using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Web.Models
{
    public class EmployeeVM
    {
        public Guid Id { get; set; }
        
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        public ICollection<ProjectVM> Projects { get; set; }

        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName} {MiddleName}";
            }
        }
    }
}