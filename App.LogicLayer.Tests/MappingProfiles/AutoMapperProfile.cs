using App.DataLayer.Entities;
using App.LogicLayer.DTO;
using AutoMapper;

namespace App.LogicLayer.Tests
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>(MemberList.None).ReverseMap();
            CreateMap<Project, ProjectDTO>(MemberList.None).ReverseMap();
        }
    }
}
