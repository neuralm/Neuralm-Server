using Neuralm.Services.TrainingRoomService.Application.Interfaces;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;
using System;
using System.Threading.Tasks;

namespace Neuralm.Services.TrainingRoomService.Tests.Mocks
{
    public class UserServiceMock : IUserService
    {
        private readonly Guid _userId;
        private readonly string _username;

        public UserServiceMock(Guid userId, string username)
        {
            _userId = userId;
            _username = username;
        }

        public Task<UserDto> FindUserAsync(Guid id)
        {
            return Task.FromResult(_userId.Equals(id) ? new UserDto() {Id = _userId, Username = _username } : null);
        }
    }
}
