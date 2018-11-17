using App.LogicLayer.DTO;
using System;

namespace App.LogicLayer.Interfaces
{
    public interface IProjectService : ICrudService<ProjectDTO>
    {
        void AttachEmployee(Guid projectId, Guid employeeId);
        void DetachEmployee(Guid projectId, Guid employeeId);
    }
}
