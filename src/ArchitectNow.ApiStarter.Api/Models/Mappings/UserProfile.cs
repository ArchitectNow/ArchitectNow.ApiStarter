using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Models;
using AutoMapper;

namespace ArchitectNow.ApiStarter.Api.Models.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserVm>().ReverseMap();

            //Mapper.CreateMap<Src, Dest>()
            //    .ForMember(d => d.UserName, opt => opt.MapFrom(/* ????? */));
        }
    }
}