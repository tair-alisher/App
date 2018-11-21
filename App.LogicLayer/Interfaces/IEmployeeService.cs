using App.LogicLayer.DTO;
using System.Collections.Generic;

namespace App.LogicLayer.Interfaces
{
    public interface IEmployeeService : ICrudService<EmployeeDTO>
    {
        IEnumerable<EmployeeDTO> GetEmployeesByName(string name);
    }
}
