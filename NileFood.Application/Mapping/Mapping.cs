using NileFood.Application.Contracts.Authentication;
using NileFood.Application.Contracts.Users;
using NileFood.Domain.Entities.Identity;

namespace NileFood.Application.Mapping;
internal class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(des => des.UserName, src => src.Email)
            .Map(des => des.ImageUrl, src => "Profiles/Default-Image.jpg");


        config.NewConfig<CreateUserRequest, ApplicationUser>()
            .Map(des => des.UserName, src => src.Email)
            .Map(des => des.ImageUrl, src => "Profiles/Default-Image.jpg");

        TypeAdapterConfig<(ApplicationUser user, List<string> roles), UserResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.user.Id)
            .Map(dest => dest.Email, src => src.user.Email)
            .Map(dest => dest.Roles, src => src.roles);

    }
}
