using System;
using System.Collections.Generic;
using System.Linq;
using App.DataLayer.Entities;
using App.DataLayer.Interfaces;
using App.LogicLayer.DTO;
using App.LogicLayer.Services;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace App.LogicLayer.Tests.Services.Tests
{
    [TestClass]
    public class EmployeeServiceTests
    {
        Mock<IRepository<Employee>> mockEmployeeRepository;
        Mock<IUnitOfWork> mockUnitOfWork;

        EmployeeService EmployeeService;
        List<Employee> Employees;

        [TestInitialize]
        public void TestInitialize()
        {
            ResetAndInitializeMapper();
            SetupMockEmployeeRepository();
            SetupMockUnitOfWork();

            Employees = new List<Employee>();

            EmployeeService = new EmployeeService(mockUnitOfWork.Object);
        }

        private void ResetAndInitializeMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }

        private void SetupMockEmployeeRepository()
        {
            mockEmployeeRepository = new Mock<IRepository<Employee>>();

            mockEmployeeRepository
                .Setup(r => r.Create(It.IsAny<Employee>()))
                .Callback<Employee>(Add);
            mockEmployeeRepository
                .Setup(r => r.Delete(It.IsAny<Guid>()))
                .Callback<Guid>(Delete);
            mockEmployeeRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns((Guid id) => Employees.Where(e => e.Id == id).First());
        }

        private void SetupMockUnitOfWork()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(u => u.Employees)
                .Returns(mockEmployeeRepository.Object);
        }

        private void Add(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            Employees.Add(employee);
        }

        private void Delete(Guid id)
        {
            Employees.Remove(FindEmployee(id));
        }

        private Employee FindEmployee(Guid id)
        {
            return Employees.Where(e => e.Id == id).First();
        }

        [TestMethod]
        public void AddMethod_Test()
        {
            // Arrange
            int elementsCountBeforeAct = Employees.Count();

            EmployeeDTO employeeDTO = new EmployeeDTO();

            // Act
            EmployeeService.Add(employeeDTO);

            // Assert
            Assert.AreNotEqual(elementsCountBeforeAct, Employees.Count());
            Assert.IsTrue(Employees.Count() > elementsCountBeforeAct);
        }

        [TestMethod]
        public void DeleteMethod_Test()
        {
            // Arrange
            int OriginalEmployeesCount = Employees.Count();
            Guid addedEmployeeId = Guid.NewGuid();
            Employee addedEmployee = new Employee
            {
                Id = addedEmployeeId,
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                MiddleName = "TestUserMiddleName",
                Email = "TestUserEmail@mail.com"
            };
            Employees.Add(addedEmployee);
            int EmployeesCountWithOneNewEmployee = Employees.Count();

            // Act
            EmployeeService.Delete(addedEmployeeId);

            // Assert
            Assert.AreNotEqual(OriginalEmployeesCount, EmployeesCountWithOneNewEmployee);
            Assert.IsFalse(Employees.Any(e => e.Id == addedEmployeeId));
            Assert.AreEqual(OriginalEmployeesCount, Employees.Count());
        }

        [TestMethod]
        public void GetMehod_Test()
        {
            // Arrange
            Guid addedEmployeeId = Guid.NewGuid();
            Employee addedEmployee = new Employee
            {
                Id = addedEmployeeId,
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                MiddleName = "TestUserMiddleName",
                Email = "TestUserEmail@mail.com"
            };
            Employees.Add(addedEmployee);

            // Act
            EmployeeDTO employeeDTO = EmployeeService.Get(addedEmployeeId);

            // Assert
            Assert.AreEqual(addedEmployee.Id, employeeDTO.Id);
            Assert.AreEqual(addedEmployee.FirstName, employeeDTO.FirstName);
        }
    }
}
