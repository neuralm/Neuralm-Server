﻿using Microsoft.EntityFrameworkCore;
using Neuralm.Services.Common.Exceptions;
using Neuralm.Services.Common.Persistence;
using Neuralm.Services.Common.Persistence.EFCore;
using Neuralm.Services.Common.Persistence.EFCore.Abstractions;
using Neuralm.Services.TrainingRoomService.Domain;
using Neuralm.Services.TrainingRoomService.Persistence.Contexts;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Neuralm.Services.TrainingRoomService.Persistence.Repositories
{
    /// <summary>
    /// Represents the <see cref="TrainingRoomRepository"/> class.
    /// </summary>
    public sealed class TrainingRoomRepository : RepositoryBase<TrainingRoom, TrainingRoomDbContext>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TrainingRoomRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="entityValidator">The entity validator.</param>
        /// <param name="logger">The logger.</param>
        public TrainingRoomRepository(
            TrainingRoomDbContext dbContext,
            IEntityValidator<TrainingRoom> entityValidator,
            ILogger<TrainingRoomDbContext> logger) : base(dbContext, entityValidator, logger)
        {

        }

       /// <inheritdoc cref="RepositoryBase{TEntity,TDbContext}.CreateAsync(TEntity)"/>
       public override async Task<(bool success, Guid id)> CreateAsync(TrainingRoom entity)
       {
           bool saveSuccess = false;
           using EntityLoadLock.Releaser loadLock = EntityLoadLock.Shared.Lock();
           try
           {
               EntityValidator.Validate(entity);
               DbContext.Entry(entity.Owner).State = EntityState.Unchanged;
               DbContext.Set<TrainingRoom>().Add(entity);
               int saveResult = await DbContext.SaveChangesAsync();
               saveSuccess = Convert.ToBoolean(saveResult);
           }
           catch (Exception ex)
           {
               CreatingEntityFailedException creatingEntityFailedException = new CreatingEntityFailedException($"The entity of type {typeof(TrainingRoom).Name} could not be created.", ex);
               Logger.LogError(creatingEntityFailedException, creatingEntityFailedException.Message);
           }
           return (saveSuccess, entity.Id);
       }
    }
}
