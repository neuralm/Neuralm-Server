using AutoMapper;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Mapping.Profiles
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