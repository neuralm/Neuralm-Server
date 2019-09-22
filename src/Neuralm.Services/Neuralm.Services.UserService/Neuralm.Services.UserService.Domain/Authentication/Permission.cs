using Neuralm.Services.Common.Domain;
using System;

namespace Neuralm.Services.UserService.Domain.Authentication
{
    /// <summary>
    /// Represents the <see cref="Permission"/> class.
    /// </summary>
    public class Permission : IEntity
    {
        /// <summary>
        /// Gets and sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets and sets the code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets and sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the position.
        /// </summary>
        public int? Position { get; set; }
    }
}
