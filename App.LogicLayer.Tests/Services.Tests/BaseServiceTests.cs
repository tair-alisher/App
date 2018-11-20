using App.DataLayer.Interfaces;
using App.LogicLayer.Services;
using App.LogicLayer.Tests.Repositories;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace App.LogicLayer.Tests.Services.Tests
{
    public class BaseServiceTests
    {
        public MockEmployeeRepository mockEmployeeRepository;
        public MockProjectRepository mockProjectRepository;
        public Mock<IUnitOfWork> mockUnitOfWork;

        public EmployeeService EmployeeService;
        public ProjectService ProjectService;

        public void TestInitialize()
        {
            ResetAndInitializeMapper();
            SetupMockEmployeeAndProjectRepositories();
            SetupMockUnitOfWork();

            EmployeeService = new EmployeeService(mockUnitOfWork.Object);
            ProjectService = new ProjectService(mockUnitOfWork.Object);
        }

        private void ResetAndInitializeMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }

        private void SetupMockEmployeeAndProjectRepositories()
        {
            mockEmployeeRepository = new MockEmployeeRepository();
            mockProjectRepository = new MockProjectRepository(mockEmployeeRepository.Employees);
        }

        private void SetupMockUnitOfWork()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(u => u.Employees)
                .Returns(mockEmployeeRepository.repository.Object);
            mockUnitOfWork
                .Setup(u => u.Projects)
                .Returns(mockProjectRepository.repository.Object);
        }
    }
}
