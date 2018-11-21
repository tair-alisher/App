using System;
using System.Collections.Generic;
using System.Linq;
using App.DataLayer.Entities;
using App.LogicLayer.DTO;
using App.LogicLayer.Infrastructure.Exceptions;
using App.LogicLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.LogicLayer.Tests.Services.Tests
{
    [TestClass]
    public class EmployeeServiceTests : BaseServiceTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            base.Init();
        }

        [TestMethod]
        public void AddEmployee()
        {
            // Arrange
            int elementsCountBeforeAct = mockEmployeeRepository.Employees.Count();

            EmployeeDTO employeeDTO = new EmployeeDTO();

            // Act
            EmployeeService.Add(employeeDTO);

            // Assert
            Assert.AreNotEqual(elementsCountBeforeAct, mockEmployeeRepository.Employees.Count());
            Assert.IsTrue(mockEmployeeRepository.Employees.Count() > elementsCountBeforeAct);
        }

        [TestMethod]
        public void GetReturnsEmployee()
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
            mockEmployeeRepository.Add(addedEmployee);

            // Act
            var employeeDTO = EmployeeService.Get(addedEmployeeId) as EmployeeDTO;

            // Assert
            Assert.IsNotNull(employeeDTO);
            Assert.AreEqual(addedEmployee.Id, employeeDTO.Id);
            Assert.AreEqual(addedEmployee.FirstName, employeeDTO.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetWithInvalidIdReturnsNotFoundException()
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
            mockEmployeeRepository.Add(addedEmployee);

            // Act
            EmployeeDTO employeeDTO = EmployeeService.Get(Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetWithNullIdReturnsArgumentNullException()
        {
            // Act
            EmployeeDTO employeeDTO = EmployeeService.Get(null);
        }

        [TestMethod]
        public void GetAllReturnsAllEmployees()
        {
            // Arrange
            int allEmployeeCount = mockEmployeeRepository.Employees.Count();
            string firstEmployeeName = mockEmployeeRepository.Employees.First().FirstName;

            // Act
            var employees = EmployeeService.GetAll() as IEnumerable<EmployeeDTO>;

            // Assert
            Assert.AreEqual(allEmployeeCount, employees.Count());
            Assert.AreEqual(firstEmployeeName, employees.First().FirstName);
        }

        [TestMethod]
        public void DeleteRemovesEmployee()
        {
            // Arrange
            int OriginalEmployeesCount = mockEmployeeRepository.Employees.Count();
            Employee addedEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "TestUserFirstName",
                LastName = "TestUserLastName",
                MiddleName = "TestUserMiddleName",
                Email = "TestUserEmail@mail.com"
            };
            mockEmployeeRepository.Add(addedEmployee);
            int EmployeesCountWithOneNewEmployee = mockEmployeeRepository.Employees.Count();

            // Act
            EmployeeService.Delete(addedEmployee.Id);

            // Assert
            Assert.AreNotEqual(OriginalEmployeesCount, EmployeesCountWithOneNewEmployee);
            Assert.IsFalse(mockEmployeeRepository.Employees.Any(e => e.Id == addedEmployee.Id));
            Assert.IsFalse(mockEmployeeRepository.Employees.Any(e => e.FirstName == addedEmployee.FirstName));
            Assert.AreEqual(OriginalEmployeesCount, mockEmployeeRepository.Employees.Count());
        }

        [TestMethod]
        public void DeleteRemovesEmployeeAsManagerFromHisProjects ()
        {
            // arrange
            Employee employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "EmployeeManager",
                LastName = "Manager",
                MiddleName = "Employee",
                Email = "manager@mail.com"
            };
            Project managerProjectOne = new Project
            {
                Title = "ManagerProjectOne",
                Customer = "CustomerOne",
                Performer = "PerformerOne",
                Priority = 0,
                Comment = "",
                DateStart = DateTime.Today.AddDays(-1),
                DateEnd = null,
                CreatedAt = DateTime.Today.AddDays(-1),
                ManagerId = employee.Id,
                Employees = new List<Employee>
                {
                    mockEmployeeRepository.Employees[0],
                    mockEmployeeRepository.Employees[1]
                }
            };
            Project managerProjectTwo = new Project
            {
                Title = "ManagerProjectTwo",
                Customer = "CustomerTwo",
                Performer = "PerformerTwo",
                Priority = 4,
                Comment = "comment",
                DateStart = DateTime.Today,
                DateEnd = DateTime.Today.AddDays(3),
                CreatedAt = DateTime.Today,
                ManagerId = employee.Id
            };

            mockEmployeeRepository.Add(employee);
            mockProjectRepository.Projects.Add(managerProjectOne);
            mockProjectRepository.Projects.Add(managerProjectTwo);

            int employeeProjectsCount = mockProjectRepository.Projects.Where(p => p.ManagerId == employee.Id).Count();
            Assert.IsTrue(mockProjectRepository.Projects.Any(p => p.ManagerId == employee.Id));

            // Act
            EmployeeService.Delete(employee.Id);

            // Assert
            int employeeProjectsCountAfterDelete = mockProjectRepository.Projects.Where(p => p.ManagerId == employee.Id).Count();
            Assert.IsTrue(employeeProjectsCount > employeeProjectsCountAfterDelete);
            Assert.AreEqual(employeeProjectsCountAfterDelete, 0);
            Assert.IsFalse(mockProjectRepository.Projects.Any(p => p.ManagerId == employee.Id));
            Assert.IsFalse(mockEmployeeRepository.Employees.Any(e => e.Id == employee.Id));
        }

        [TestMethod]
        public void GetEmployeesByNameReturnsEmployeeList()
        {
            // Arrange
            int originalemployeesCount = mockEmployeeRepository.Employees.Count();
            Employee employeeOne = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Алина",
                LastName = "Чазова",
                MiddleName = "Святославовна"
            };
            Employee employeeTwo = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Алина",
                LastName = "Дябина",
                MiddleName = "Игнатиевна"
            };
            Employee employeeThree = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "Филимон",
                LastName = "Чазов",
                MiddleName = "Архипович"
            };

            mockEmployeeRepository.Add(employeeOne);
            mockEmployeeRepository.Add(employeeTwo);
            mockEmployeeRepository.Add(employeeThree);

            // Act
            var employeesByName = EmployeeService.GetEmployeesByName(employeeOne.FirstName) as IEnumerable<EmployeeDTO>;
            var employeesByLowerName = EmployeeService.GetEmployeesByName(employeeOne.FirstName.ToLower()) as IEnumerable<EmployeeDTO>;
            var employeesByLastName = EmployeeService.GetEmployeesByName(employeeThree.LastName) as IEnumerable<EmployeeDTO>;

            // Assert
            Assert.IsTrue(mockEmployeeRepository.Employees.Count() > originalemployeesCount);
            Assert.IsTrue(employeesByName.Count() >= 2);
            Assert.IsTrue(employeesByLowerName.Count() >= 2);
            Assert.IsTrue(employeesByLastName.Count() >= 2);
        }
    }
}
