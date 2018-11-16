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
            Project project = Mapper.Map<Project>(projectDTO);

            _unitOfWork.Projects.Update(project);
            _unitOfWork.Save();
        }
    }
}
