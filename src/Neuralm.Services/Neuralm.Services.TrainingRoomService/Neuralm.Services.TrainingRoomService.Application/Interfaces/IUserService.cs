using System;
using System.Threading.Tasks;
using Neuralm.Services.TrainingRoomService.Messages.Dtos;

namespace Neuralm.Services.TrainingRoomService.Application.Interfaces
{
    /// <summary>
    /// Represents the <see cref="IUserService"/> interface.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Finds the user by id asynchronously.
        /// </summary>
        /// <param name="id">The id,</param>
        /// <returns>Returns the user if found, otherwise; returns null.</returns>
        Task<UserDto> FindUserAsync(Guid id);
    }
}