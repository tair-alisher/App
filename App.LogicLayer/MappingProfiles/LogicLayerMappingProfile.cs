using App.DataLayer.Entities;
using App.LogicLayer.DTO;
using AutoMapper;

namespace App.LogicLayer.MappingProfiles
{
    public class LogicLayerMappingProfile : Profile
    {
        public LogicLayerMappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>(MemberList.None).ReverseMap();
            CreateMap<Project, ProjectDTO>(MemberList.None).ReverseMap();
        }
    }
}
