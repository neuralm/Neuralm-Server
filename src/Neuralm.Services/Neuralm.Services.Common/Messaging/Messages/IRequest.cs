﻿using System;

namespace Neuralm.Services.Common.Messaging.Messages
{
    /// <summary>
    /// Represents the <see cref="IRequest"/> interface.
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        DateTime DateTime { get; }
    }
}