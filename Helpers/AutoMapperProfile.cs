using AutoMapper;
using PhinaMart.Models;
using PhinaMart.ViewModels;

namespace PhinaMart.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVm, User>();
            //	.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen)).ReverseMap();
        }
    }
}
