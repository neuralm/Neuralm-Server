using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Neuralm.Application.Interfaces;
using Neuralm.Domain.Entities.NEAT;
using Neuralm.Persistence.Abstractions;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Persistence.Repositories
{
    public class TrainingRoomRepository : RepositoryBase<TrainingRoom, NeuralmDbContext>
    {
        public TrainingRoomRepository(NeuralmDbContext dbContext, IEntityValidator<TrainingRoom> entityValidator) : base(dbContext, entityValidator)
        {

        }

        public override Task<bool> CreateAsync(TrainingRoom entity)
        {
            DbContext.Entry(entity.Owner).State = EntityState.Unchanged;
            return base.CreateAsync(entity);
        }
    }
}
