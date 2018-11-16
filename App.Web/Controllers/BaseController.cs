using App.LogicLayer.Interfaces;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class BaseController : Controller
    {
        public IEmployeeService EmployeeService;
        public IProjectService ProjectService;

        public BaseController(IEmployeeService employeeService, IProjectService projectService)
        {
            EmployeeService = employeeService;
            ProjectService = projectService;
        }

        public BaseController(IEmployeeService employeeService)
        {
            EmployeeService = employeeService;
        }

        public BaseController(IProjectService projectService)
        {
            ProjectService = projectService;
        }
    }
}