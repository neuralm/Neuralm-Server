using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Neuralm.Application.Messages.Dtos;
using Neuralm.Domain;
using Neuralm.Domain.Entities;

namespace Neuralm.Application.Converters
{
    /// <summary>
    /// Represents the <see cref="EntityToDtoConverter"/> class.
    /// </summary>
    public static class EntityToDtoConverter
    {
        /// <summary>
        /// Converts the given Entity into the given Dto type.
        /// </summary>
        /// <typeparam name="TDto">The dto type.</typeparam>
        /// <typeparam name="TEntity">The Entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns the converted entity as dto.</returns>
        public static TDto Convert<TDto, TEntity>(TEntity entity) where TDto : class, new()
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            TDto dto = new TDto();
            IList<PropertyInfo> joinedProperties =
                typeof(TDto).GetProperties().Join(typeof(TEntity).GetProperties(),
                dtoProperty => dtoProperty.Name,
                entityProperty => entityProperty.Name,
                (dtoProperty, entityProperty) => dtoProperty)
                .ToList();
            foreach (PropertyInfo property in joinedProperties)
            {
                Type propertyType = property.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    Type dtoItemType = propertyType.GetGenericArguments()[0];
                    IList genericListInstance = CreateGenericListOfType(dtoItemType);
                    dynamic list = typeof(TEntity).GetProperty(property.Name).GetValue(entity);
                    if (list == null)
                    {
                        property.SetValue(dto, genericListInstance);
                        continue;
                    }
                    foreach (object item in list)
                    {
                        if (item == null)
                            continue;
                        
                        Type actualType = ((IProxyTargetAccessor)item).DynProxyGetTarget().GetType().BaseType;
                        genericListInstance.Add(Convert(dtoItemType, actualType, item));
                    }
                    property.SetValue(dto, genericListInstance);
                    continue;
                }

                Type entityType = entity.GetType();
                if (GetValue(entityType, entity, property, dto))
                    continue;
                object value = entityType.GetProperty(property.Name).GetValue(entity);
                property.SetValue(dto, value);
            }
            return dto;
        }

        private static IList CreateGenericListOfType(Type itemType)
        {
            Type listGenericType = typeof(List<>);
            dynamic listOfItemType = listGenericType.MakeGenericType(itemType);
            return (IList)Activator.CreateInstance(listOfItemType);
        }

        private static object Convert(Type dtoItemType, Type entityType, object entity)
        {
            dynamic dto = Activator.CreateInstance(dtoItemType);
            IList<PropertyInfo> dtoProperties = new List<PropertyInfo>(dtoItemType.GetProperties());
            foreach (PropertyInfo property in dtoProperties)
            {
                Type type = property.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition().GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    Type newDtoItemType = type.GetGenericArguments()[0];
                    IList genericListInstance = CreateGenericListOfType(newDtoItemType);
                    dynamic list = entityType.GetProperty(property.Name).GetValue(entity);
                    if (list == null)
                    {
                        property.SetValue(dto, genericListInstance);
                        continue;
                    }
                    foreach (dynamic item in list)
                    {
                        Type actualType = ((IProxyTargetAccessor)item).DynProxyGetTarget().GetType();
                        genericListInstance.Add(Convert(newDtoItemType, actualType, item));
                    }
                    property.SetValue(dto, genericListInstance);
                    continue;
                }

                if (GetValue(entityType, entity, property, dto))
                    continue;
                object value = entityType.GetProperty(property.Name).GetValue(entity);
                property.SetValue(dto, value);
            }

            return dto;
        }

        private static bool GetValue(Type entityType, object entity, PropertyInfo property, dynamic dto)
        {
            string propertyTypeName = property.PropertyType.Name.Replace("Proxy", "").Replace("Dto", "");

            // TODO: Verify if the current "Entity" type has a DTO type. if not throw!
            // TODO: also check if the property name is not equal to an Entity by chance... so maybe try to check for standard types?
            if (!typeof(UserDto).Assembly.GetTypes().Any(t => t.Name.Equals(propertyTypeName + "Dto")))
                return false;
            Type newEntityType = typeof(User).Assembly.GetTypes().First(t => t.Name.Equals(propertyTypeName));
            object entityObject;
            // NOTE: Because of lazy loading the database will need to be locked to ensure no parallel queries are running (which will throw errors).
            // This will use the shared globally shared LoadLock.
            if (property.PropertyType.Name.Contains("Proxy"))
            {
                using EntityLoadLock.Releaser lazyLoadLock = EntityLoadLock.Shared.Lock();
                entityObject = entityType.GetProperty(property.Name).GetValue(entity, null);
            }
            else
            {
                entityObject = entityType.GetProperty(property.Name).GetValue(entity, null);
            }

            if (entityObject == null)
                return false;

            object result = Convert(property.PropertyType, newEntityType, entityObject);
            property.SetValue(dto, result);
            return true;
        }
    }
}
