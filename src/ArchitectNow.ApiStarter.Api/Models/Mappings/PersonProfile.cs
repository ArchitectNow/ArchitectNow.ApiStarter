using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Models;
using AutoMapper;

namespace ArchitectNow.ApiStarter.Api.Models.Mappings
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonProfile>().ReverseMap();
        }
    }
}