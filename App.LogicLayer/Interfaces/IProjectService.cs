using App.LogicLayer.DTO;
using System;
using System.Collections.Generic;

namespace App.LogicLayer.Interfaces
{
    public interface IProjectService : ICrudService<ProjectDTO>
    {
        void AttachEmployee(Guid projectId, Guid employeeId);
        void DetachEmployee(Guid projectId, Guid employeeId);
        List<EmployeeDTO> GetManagerList();
        IEnumerable<ProjectDTO> GetFilteredAndSortedProjectList(SortAndFilterParamsDTO parameters);
    }
}
