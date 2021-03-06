﻿using Neuralm.Services.Common.Messages.Interfaces;
using System;

namespace Neuralm.Services.Common.Messages.Abstractions
{
    /// <summary>
    /// Represents the <see cref="Request"/> class.
    /// </summary>
    public abstract class Request : IMessage
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="Request"/> class.
        /// </summary>
        protected Request()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
        }
    }
}
