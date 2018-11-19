using App.LogicLayer.DTO;
using System;
using System.Collections.Generic;

namespace App.LogicLayer.Interfaces
{
    public interface IProjectService : ICrudService<ProjectDTO>
    {
        void AttachEmployee(Guid projectId, Guid employeeId);
        void DetachEmployee(Guid projectId, Guid employeeId);
        IEnumerable<ProjectDTO> GetFilteredAndSortedProjectList(string startDate, string priority, string managerId, string sortProperty);
    }
}
