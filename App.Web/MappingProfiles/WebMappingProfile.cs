using App.LogicLayer.DTO;
using App.Web.Models;
using AutoMapper;

namespace App.Web.MappingProfiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<EmployeeDTO, EmployeeVM>(MemberList.None).ReverseMap();
            CreateMap<ProjectDTO, ProjectVM>(MemberList.None).ReverseMap();
        }
    }
}