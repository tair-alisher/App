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

            _unitOfWork.Employees.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EmployeeDTO Get(Guid id)
        {
            Employee employee = _unitOfWork.Employees.Get(id);

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
    }
}
