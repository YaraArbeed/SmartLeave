using AutoMapper;
using SmartLeave.API.DTOs;
using SmartLeave.DAL.Entities;

namespace SmartLeave.API.Mapping
{
    public class MappingProfile : Profile
    {
        //When you are mapping an object of type
        //UserForRegistrationDto to an object of type User,
        //take the value from the Email property of the UserForRegistrationDto object
        //and use it to populate the UserName property of the User object
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
    .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
