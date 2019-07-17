﻿using System;

namespace Neuralm.Application.Messages
{
    /// <summary>
    /// Represents the <see cref="Command"/> class.
    /// </summary>
    public abstract class Command : ICommand
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="Command"/> class.
        /// </summary>
        protected Command()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
