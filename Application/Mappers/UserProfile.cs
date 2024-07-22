using Application.Models.Identities;

using AutoMapper;

using Domain.Identities;

namespace Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterCommand, User>();

        CreateMap<User, UserDTO>()
            .ForMember(
            dest => dest.FullName,
            opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    }
}
