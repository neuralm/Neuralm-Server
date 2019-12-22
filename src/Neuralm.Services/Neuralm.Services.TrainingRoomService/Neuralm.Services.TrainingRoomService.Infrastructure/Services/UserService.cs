using System;
using System.Threading.Tasks;
using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Infrastructure.Services
{
    /// <summary>
    /// Represents the <see cref="UserService"/> class.
    /// </summary>
    public class UserService : IUserService
    {
        //INetworkConnector networkConnector
        public UserService()
        {
//            networkConnector.s
        }
        
        /// <inheritdoc cref="IUserService.FindUserAsync(Guid)"/>
        public Task<UserDto> FindUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}