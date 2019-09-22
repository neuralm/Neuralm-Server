﻿using System;

namespace Neuralm.Services.Common.Messaging.Messages
{
    /// <summary>
    /// Represents the <see cref="IEvent"/> interface.
    /// </summary>
    public interface IEvent
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