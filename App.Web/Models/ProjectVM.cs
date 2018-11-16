using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Web.Models
{
    public class ProjectVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Заказчик")]
        public string Customer { get; set; }

        [Display(Name = "Исполнитель")]
        public string Performer { get; set; }

        [Display(Name = "Приоритет")]
        public int Priority { get; set; }

        [Display(Name = "Комментарий")]
        public string Comment { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата начала проекта")]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата окончания проекта")]
        public DateTime? DateEnd { get; set; }

        [Display(Name = "Руководитель проекта")]
        public Guid? ManagerId { get; set; }

        public EmployeeVM Manager { get; set; }
        public ICollection<EmployeeVM> Employees { get; set; }
    }
}