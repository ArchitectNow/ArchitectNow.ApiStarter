using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Models;
using AutoMapper;

namespace ArchitectNow.ApiStarter.Api.Models.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressVm>().ReverseMap();
        }
    }
}