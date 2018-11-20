using App.DataLayer.Entities;
using App.DataLayer.Interfaces;
using App.LogicLayer.DTO;
using App.LogicLayer.Infrastructure.Exceptions;
using App.LogicLayer.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.LogicLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public EmployeeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Add(EmployeeDTO employeeDTO)
        {
            Employee employee = Mapper.Map<Employee>(employeeDTO);

            _unitOfWork.Employees.Create(employee);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            Employee employee = _unitOfWork.Employees.Get(id);
            if (employee == null)
                throw new NotFoundException();

            RemoveManagerFromProjects(employee.Id);
            employee.Projects.Clear();

            _unitOfWork.Employees.Delete(id);
            _unitOfWork.Save();
        }

        private void RemoveManagerFromProjects(Guid employeeId)
        {
            List<Project> projects = _unitOfWork
                .Projects
                .Find(p => p.ManagerId == employeeId)
                .ToList();

            foreach (var project in projects)
                project.ManagerId = null;

            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EmployeeDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            Employee employee = _unitOfWork.Employees.Get(id);
            if (employee == null)
                throw new NotFoundException();

            return Mapper.Map<EmployeeDTO>(employee);
        }

        public IEnumerable<EmployeeDTO> GetAll()
        {
            List<Employee> employees = _unitOfWork.Employees.GetAll().ToList();

            return Mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }

        public void Update(EmployeeDTO employeeDTO)
        {
            Employee employee = Mapper.Map<Employee>(employeeDTO);

            _unitOfWork.Employees.Update(employee);
            _unitOfWork.Save();
        }

        public IEnumerable<EmployeeDTO> GetEmployeesByName(string inputName)
        {
            IEnumerable<Employee> foundEmployees = Enumerable.Empty<Employee>();

            string name = inputName.Trim();
            if (name.Length <= 0)
                return Mapper.Map<IEnumerable<EmployeeDTO>>(foundEmployees);

            NameParts nameParts = new NameParts(name);

            while (nameParts.PartsCount > 0)
            {
                foundEmployees = GetEmployeesByName(foundEmployees, nameParts.GetFirstElement());
                nameParts.RemoveFirstElement();
            }

            return Mapper.Map<IEnumerable<EmployeeDTO>>(foundEmployees);
        }

        private IEnumerable<Employee> GetEmployeesByName(IEnumerable<Employee> employees, string name)
        {
            List<Employee> foundEmployees = (
                from
                    emp in employees.Count() > 0 ? employees : _unitOfWork.Employees.GetAll()
                where
                    emp.LastName.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    emp.FirstName.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) >= 0
                select emp
                ).ToList();

            return foundEmployees;
        }
    }

    class NameParts
    {
        const int MaxNumberOfPartsInFullName = 3;

        List<string> Parts { get; }
        public int PartsCount
        {
            get { return Parts.Count(); }
        }

        public NameParts(string name)
        {
            Parts = name.Split(' ').ToList();
            if (Parts.Count() > MaxNumberOfPartsInFullName)
                Parts = Parts.Take(MaxNumberOfPartsInFullName).ToList();
        }

        public string GetFirstElement()
        {
            return Parts.First();
        }

        public void RemoveFirstElement()
        {
            Parts.Remove(Parts.First());
        }
    }
}
