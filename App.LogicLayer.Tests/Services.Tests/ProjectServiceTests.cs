using App.DataLayer.Entities;
using App.LogicLayer.DTO;
using App.LogicLayer.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.LogicLayer.Tests.Services.Tests
{
    [TestClass]
    public class ProjectServiceTests : BaseServiceTests
    {
        ProjectDTO project;

        [TestInitialize]
        public void TestInitialize()
        {
            base.Init();

            project = new ProjectDTO
            {
                Id = Guid.NewGuid(),
                Title = "ProjectName",
                Customer = "ProjectCustomer",
                Performer = "ProjectPerformer",
                Priority = 3,
                Comment = "ProjectMessage",
                DateStart = DateTime.Today.AddYears(-1),
                DateEnd = null,
                ManagerId = null
            };

        }

        [TestMethod]
        public void AddProject()
        {
            // Arrange
            int projectsCount = mockProjectRepository.Projects.Count();

            ProjectDTO projectDTO = project;

            // Act
            ProjectService.Add(projectDTO);
            var addedProject = ProjectService.Get(projectDTO.Id) as ProjectDTO;

            // Assert
            Assert.IsTrue(mockProjectRepository.Projects.Count() > projectsCount);
            Assert.IsNotNull(mockProjectRepository.Projects.Last().CreatedAt);
            Assert.IsTrue(mockProjectRepository.Projects.Any(p => p.Title == projectDTO.Title));
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void DeleteProjectWithInvalidIdReturnsNotFoundException()
        {
            // Act
            ProjectService.Delete(Guid.NewGuid());
        }

        [TestMethod]
        public void DeleteProject()
        {
            // Arrange
            int projectsCount = mockProjectRepository.Projects.Count();
            ProjectDTO projectDTO = project;
            ProjectService.Add(projectDTO);
            int incrementedProjectCount = mockProjectRepository.Projects.Count();

            // Act
            ProjectService.Delete(projectDTO.Id);

            // Assert
            Assert.IsTrue(incrementedProjectCount > projectsCount);
            Assert.AreEqual(projectsCount, mockProjectRepository.Projects.Count());
            Assert.IsFalse(mockProjectRepository.Projects.Any(p => p.Title == projectDTO.Title));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetMethodWithNullIdReturnsArgumentNullException()
        {
            // Act
            ProjectService.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void GetMethodWithInvalidIdReturnsNotFoundException()
        {
            // Act
            ProjectService.Get(Guid.NewGuid());
        }

        [TestMethod]
        public void GetMethod()
        {
            // Arrange
            ProjectDTO projectDTO = project;
            ProjectService.Add(projectDTO);

            // Act
            var returnedProject = ProjectService.Get(projectDTO.Id) as ProjectDTO;

            // Assert
            Assert.IsTrue(mockProjectRepository.Projects.Any(p => p.Id == projectDTO.Id));
            Assert.AreEqual(returnedProject.Id, projectDTO.Id);
            Assert.AreEqual(returnedProject.Title, projectDTO.Title);
        }

        [TestMethod]
        public void GetProjectListOrderedByTitleReturnsOrderedProjectList()
        {
            // Arrange
            Project firstProjectInOrderedList = mockProjectRepository.Projects.OrderBy(p => p.Title).First();
            Project lastProjectInOrderedList = mockProjectRepository.Projects.OrderBy(p => p.Title).Last();

            // Act
            var projectList = ProjectService.GetProjectListOrderedByTitle() as IEnumerable<ProjectDTO>;

            // Assert
            Assert.AreEqual(firstProjectInOrderedList.Id, projectList.First().Id);
            Assert.AreEqual(projectList.Count(), mockProjectRepository.Projects.Count());
            Assert.AreEqual(lastProjectInOrderedList.Id, projectList.Last().Id);
        }

        [TestMethod]
        public void GetAllReturnsProjectDTOList()
        {
            // Act
            var projectList = ProjectService.GetAll() as IEnumerable<ProjectDTO>;

            // Assert
            Assert.AreEqual(projectList.Count(), mockProjectRepository.Projects.Count());
        }

        [TestMethod]
        public void UpdateMethodUpdatesSomeFields()
        {
            // Arrange
            ProjectDTO projectDTO = project;
            ProjectService.Add(projectDTO);
            ProjectDTO newData = new ProjectDTO
            {
                Id = projectDTO.Id,
                Title = "new udpated title",
                Customer = "new updated customer",
                Performer = "new updated performer",
                Priority = projectDTO.Priority,
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now,
                ManagerId = Guid.NewGuid()
            };

            // Act
            ProjectService.Update(newData);
            ProjectDTO updatedProject = ProjectService.Get(projectDTO.Id);

            // Assert
            Assert.AreEqual(projectDTO.Id, updatedProject.Id);
            Assert.AreEqual(projectDTO.Priority, updatedProject.Priority);
            Assert.AreNotEqual(projectDTO.Title, updatedProject.Title);
            Assert.AreNotEqual(projectDTO.Customer, updatedProject.Customer);
            Assert.AreNotEqual(projectDTO.Performer, updatedProject.Performer);
            Assert.AreNotEqual(projectDTO.DateStart, updatedProject.DateStart);
            Assert.AreNotEqual(projectDTO.DateEnd, updatedProject.DateEnd);
            Assert.AreNotEqual(projectDTO.ManagerId, updatedProject.ManagerId);
        }

        [TestMethod]
        public void AttachEmployeeAddsEmployeeToProjectEmployeeList()
        {
            // Arrange
            ProjectDTO projectDTO = project;
            projectDTO.Employees = new List<EmployeeDTO>();
            ProjectService.Add(projectDTO);
            Employee employeeToAttach = mockEmployeeRepository.Employees.First();

            // Act
            ProjectService.AttachEmployee(projectDTO.Id, employeeToAttach.Id);
            var projectWithEmployees = ProjectService.Get(projectDTO.Id) as ProjectDTO;

            // Assert
            Assert.IsTrue(projectWithEmployees.Employees.Count() > 0);
            Assert.IsTrue(projectWithEmployees.Employees.Any(e => e.Id == employeeToAttach.Id));
            Assert.IsTrue(projectWithEmployees.Employees.First().Id == employeeToAttach.Id);
            Assert.IsTrue(projectWithEmployees.Employees.Count() == 1);
        }

        [TestMethod]
        public void DetachEmployeeDetachesEmployeeFromProjectEmployeeList()
        {
            // Arrange
            ProjectDTO projectDTO = project;
            projectDTO.Employees = new List<EmployeeDTO>();
            ProjectService.Add(projectDTO);

            Employee firstEmployeeToAttach = mockEmployeeRepository.Employees.First();
            ProjectService.AttachEmployee(projectDTO.Id, firstEmployeeToAttach.Id);

            Employee secondEmployeeToAttach = mockEmployeeRepository.Employees.Last();
            ProjectService.AttachEmployee(projectDTO.Id, secondEmployeeToAttach.Id);

            // Act
            ProjectService.DetachEmployee(projectDTO.Id, firstEmployeeToAttach.Id);
            var projectWithEmployees = ProjectService.Get(projectDTO.Id) as ProjectDTO;

            // Assert
            Assert.IsTrue(projectWithEmployees.Employees.Count() == 1);
            Assert.IsFalse(projectWithEmployees.Employees.Any(e => e.Id == firstEmployeeToAttach.Id));
            Assert.IsTrue(projectWithEmployees.Employees.First().Id == secondEmployeeToAttach.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void DetachEmployeeWithInvalidProjectIdOrEmployeeIdThrowsNotFoundException()
        {
            // Act
            ProjectService.DetachEmployee(Guid.NewGuid(), Guid.NewGuid());
        }

        [TestMethod]
        public void GetManagerListReturnsEmployeeManagerList()
        {
            // Arrange
            List<Guid?> managerIdList = mockProjectRepository.Projects.Where(p => p.ManagerId != null).Select(m => m.ManagerId).ToList();
            List<Employee> originalManagerList = mockEmployeeRepository.Employees.Where(e => managerIdList.Contains(e.Id)).ToList();

            // Act
            var managerList = ProjectService.GetManagerList() as List<EmployeeDTO>;

            // Assert
            Assert.AreEqual(originalManagerList.Count(), managerIdList.Count());
            Assert.IsTrue(managerList.Any(m => m.Id == originalManagerList.First().Id));
        }

        [TestMethod]
        public void GetFilteredAndSortedProjectListReturnsFilteredByPriorityProjectList()
        {
            // Arrange
            SortAndFilterParamsDTO parameters = new SortAndFilterParamsDTO
            {
                PriorityFilter = mockProjectRepository.Projects.First().Priority.ToString()
            };
            List<Project> projecstWithGivenPriority = mockProjectRepository.Projects.Where(p => p.Priority == int.Parse(parameters.PriorityFilter)).ToList();

            // Act
            var projectList = ProjectService.GetFilteredAndSortedProjectList(parameters) as IEnumerable<ProjectDTO>;

            // Assert
            Assert.AreEqual(projecstWithGivenPriority.Count(), projectList.Count());
            Assert.IsTrue(projectList.Any(p => p.Id == projecstWithGivenPriority.First().Id));
        }
    }
}
