﻿using System;

namespace Neuralm.Application.Messages.Responses
{
    public class CreateRoomResponse : IResponse
    {
        public Guid Id { get; }
        public Guid RequestId { get; }
    }
}