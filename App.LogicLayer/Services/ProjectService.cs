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
    public class ProjectService : IProjectService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public ProjectService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Add(ProjectDTO projectDTO)
        {
            Project project = Mapper.Map<Project>(projectDTO);
            project.CreatedAt = DateTime.Now;

            _unitOfWork.Projects.Create(project);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            Project project = _unitOfWork.Projects.Get(id);
            if (project == null)
                throw new NotFoundException();

            project.Employees.Clear();
            _unitOfWork.Projects.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public ProjectDTO Get(Guid id)
        {
            Project project = _unitOfWork.Projects.Get(id);

            return Mapper.Map<ProjectDTO>(project);
        }

        public IEnumerable<ProjectDTO> GetAll()
        {
            List<Project> projects = _unitOfWork.Projects.GetAll().ToList();

            return Mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public void Update(ProjectDTO projectDTO)
        {
            Project project = _unitOfWork.Projects.Get(projectDTO.Id);
            project.Title = projectDTO.Title;
            project.Customer = projectDTO.Customer;
            project.Performer = projectDTO.Performer;
            project.Priority = projectDTO.Priority;
            project.DateStart = projectDTO.DateStart;
            project.DateEnd = projectDTO.DateEnd;
            project.ManagerId = projectDTO.ManagerId;

            _unitOfWork.Projects.Update(project);
            _unitOfWork.Save();
        }

        public void AttachEmployee(Guid projectId, Guid employeeId)
        {
            Project project = _unitOfWork.Projects.Get(projectId);
            Employee employee = _unitOfWork.Employees.Get(employeeId);
            if (project == null || employee == null)
                throw new NotFoundException();

            project.Employees.Add(employee);
            _unitOfWork.Save();
        }

        public void DetachEmployee(Guid projectId, Guid employeeId)
        {
            Project project = _unitOfWork.Projects.Get(projectId);
            Employee employee = _unitOfWork.Employees.Get(employeeId);
            if (project == null || employee == null)
                throw new NotFoundException();

            project.Employees.Remove(employee);
            _unitOfWork.Save();
        }

        private IEnumerable<Project> SortProjectListByProperty(IQueryable<Project> filteredProjectList = null, string property = "title")
        {
            filteredProjectList = filteredProjectList ?? _unitOfWork.Projects.GetAll();

            switch (property)
            {
                case "title":
                    filteredProjectList = filteredProjectList.OrderBy(p => p.Title);
                    break;
                case "customer":
                    filteredProjectList = filteredProjectList.OrderBy(p => p.Customer);
                    break;
                case "performer":
                    filteredProjectList = filteredProjectList.OrderBy(p => p.Performer);
                    break;
                case "priority":
                    filteredProjectList = filteredProjectList.OrderBy(p => p.Priority);
                    break;
                case "dateStart":
                    filteredProjectList = filteredProjectList.OrderBy(p => p.DateStart);
                    break;
                case "dateEnd":
                    filteredProjectList = filteredProjectList.OrderBy(p => p.DateEnd);
                    break;
            }

            return filteredProjectList.ToList();
        }

        public IEnumerable<ProjectDTO> GetFilteredAndSortedProjectList(
            string startDateValue, string priorityValue, string managerId, string sortProperty
            )
        {
            IQueryable<Project> filteredProjectList = _unitOfWork
                .Projects
                .Find(p => p.Priority == int.Parse(priorityValue) && p.ManagerId == Guid.Parse(managerId))
                .AsQueryable();

            IEnumerable<Project> filteredAndSortedList = SortProjectListByProperty(filteredProjectList, sortProperty);

            return Mapper.Map<IEnumerable<ProjectDTO>>(filteredAndSortedList);
        }
    }
}
