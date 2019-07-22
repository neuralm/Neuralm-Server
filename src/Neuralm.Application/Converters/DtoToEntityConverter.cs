using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Neuralm.Application.Converters
{
    /// <summary>
    /// Represents the <see cref="DtoToEntityConverter"/> class.
    /// </summary>
    public static class DtoToEntityConverter
    {
        /// <summary>
        /// Converts the given Dto into the given Entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TDto">The dto type.</typeparam>
        /// <param name="dto">The dto.</param>
        /// <returns>Returns the converted dto as entity.</returns>
        public static TEntity Convert<TEntity, TDto>(TDto dto) where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            IList<PropertyInfo> dtoProperties = new List<PropertyInfo>(typeof(TDto).GetProperties());
            IList<PropertyInfo> entityProperties = new List<PropertyInfo>(typeof(TEntity).GetProperties());
            IList<PropertyInfo> joinedProperties =
                dtoProperties.Join(entityProperties,
                        dtoProperty => dtoProperty.Name,
                        entityProperty => entityProperty.Name,
                        (dtoProperty, entityProperty) => entityProperty)
                    .ToList();
            foreach (PropertyInfo property in joinedProperties)
            {
                object value = dto.GetType().GetProperty(property.Name).GetValue(dto);
                if (value == null) continue;
                property.SetValue(entity, value);
            }
            return entity;
        }
    }
}
