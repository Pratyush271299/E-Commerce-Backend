using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<SignUpDTO, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
            CreateMap<User, UserDTO>();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CartProduct, UpdateDTO>().ReverseMap();
            CreateMap<CartProduct, CartDTO>().ReverseMap();
        }
    }
}
