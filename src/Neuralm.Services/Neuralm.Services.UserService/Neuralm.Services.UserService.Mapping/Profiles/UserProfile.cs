using AutoMapper;
using Neuralm.Services.UserService.Application.Dtos;
using Neuralm.Services.UserService.Domain;

namespace Neuralm.Services.UserService.Mapping.Profiles
{
    /// <summary>
    /// Represents the <see cref="UserProfile"/> class.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Maps the profile.
        /// </summary>
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
