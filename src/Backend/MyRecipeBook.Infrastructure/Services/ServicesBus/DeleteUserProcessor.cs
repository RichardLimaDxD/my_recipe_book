﻿using Azure.Messaging.ServiceBus;

namespace MyRecipeBook.Infrastructure.Services.ServicesBus
{
    public class DeleteUserProcessor
    {
        private readonly ServiceBusProcessor _processor;

        public DeleteUserProcessor(ServiceBusProcessor processor)
        {
            _processor = processor;
        }

        public ServiceBusProcessor GetProcessor() => _processor;
    }
}
