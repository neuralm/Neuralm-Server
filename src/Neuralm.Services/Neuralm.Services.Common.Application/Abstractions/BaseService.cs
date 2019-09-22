using AutoMapper;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neuralm.Services.Common.Application.Abstractions
{
    /// <summary>
    /// Represents the <see cref="BaseService{TEntity,TDto}"/> class.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TDto">The dto type.</typeparam>
    public abstract class BaseService<TEntity, TDto> : IService<TDto> where TDto : class where TEntity : class, IEntity
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository<TEntity> EntityRepository;

        /// <summary>
        /// Initializes an instance of the <see cref="BaseService{TEntity,TDto}"/> class.
        /// </summary>
        /// <param name="entityRepository">The entity repository.</param>
        /// <param name="mapper">The mapper.</param>
        protected BaseService(IRepository<TEntity> entityRepository, IMapper mapper)
        {
            Mapper = mapper;
            EntityRepository = entityRepository;
        }

        /// <inheritdoc cref="IService{TEntity}.FindSingleOrDefaultAsync(Guid)"/>
        public virtual Task<TDto> FindSingleOrDefaultAsync(Guid id)
        {
            return EntityRepository.FindSingleOrDefaultAsync(tr => tr.Id == id)
                .ContinueWith(trainingRoom => Mapper.Map<TDto>(trainingRoom.Result));
        }

        /// <inheritdoc cref="IService{TEntity}.GetAllAsync()"/>
        public virtual Task<IEnumerable<TDto>> GetAllAsync()
        {
            return EntityRepository.GetAllAsync()
                .ContinueWith(trainingRooms => Mapper.Map<IEnumerable<TDto>>(trainingRooms.Result));
        }

        /// <inheritdoc cref="IService{TEntity}.DeleteAsync(TEntity)"/>
        public virtual async Task<(bool success, bool found)> DeleteAsync(TDto dto)
        {
            TEntity trainingRoom = Mapper.Map<TEntity>(dto);
            bool found = await EntityRepository.ExistsAsync(u => u.Id.Equals(trainingRoom.Id));
            return !found ? (false, false) : (await EntityRepository.DeleteAsync(trainingRoom), true);
        }

        /// <inheritdoc cref="IService{TEntity}.UpdateAsync(TEntity)"/>
        public virtual Task<(bool success, Guid id, bool updated)> UpdateAsync(TDto dto)
        {
            TEntity trainingRoom = Mapper.Map<TEntity>(dto);
            return EntityRepository.UpdateAsync(trainingRoom);
        }

        /// <inheritdoc cref="IService{TEntity}.CreateAsync(TEntity)"/>
        public virtual Task<(bool success, Guid id)> CreateAsync(TDto dto)
        {
            TEntity trainingRoom = Mapper.Map<TEntity>(dto);
            return EntityRepository.CreateAsync(trainingRoom);
        }
    }
}
