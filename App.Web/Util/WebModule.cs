using App.LogicLayer.Interfaces;
using App.LogicLayer.Services;
using Ninject.Modules;

namespace App.Web.Util
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEmployeeService>().To<EmployeeService>();
            Bind<IProjectService>().To<ProjectService>();
        }
    }
}