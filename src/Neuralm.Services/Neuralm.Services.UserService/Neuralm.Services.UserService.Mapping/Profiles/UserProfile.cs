using AutoMapper;
using Neuralm.Services.UserService.Domain;
using Neuralm.Services.UserService.Messages.Dtos;

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
