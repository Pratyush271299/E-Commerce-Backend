using AutoMapper;
using E_Commerce.Data;
using E_Commerce.Models;

namespace E_Commerce.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, SignUpDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CartProduct, UpdateDTO>().ReverseMap();
            CreateMap<CartProduct, CartDTO>().ReverseMap();
        }
    }
}
