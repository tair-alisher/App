using App.LogicLayer.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace App.LogicLayer.Tests.Services.Tests
{
    [TestClass]
    public class ProjectServiceTests : BaseServiceTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestMethod]
        public void AddProject()
        {
            // Arrange
            int projectsCount = mockProjectRepository.Projects.Count();

            ProjectDTO projectDTO = new ProjectDTO
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

            // Act
            ProjectService.Add(projectDTO);
            var addedProject = ProjectService.Get(projectDTO.Id) as ProjectDTO;

            // Assert
            Assert.IsTrue(mockProjectRepository.Projects.Count() > projectsCount);
            Assert.IsNotNull(mockProjectRepository.Projects.Last().CreatedAt);
            Assert.IsTrue(mockProjectRepository.Projects.Any(p => p.Title == projectDTO.Title));
        }
    }
}
