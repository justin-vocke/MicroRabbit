using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {
        private readonly ITransferRepository _repository;
        public TransferEventHandler(ITransferRepository transferRepository)
        {
            _repository = transferRepository;
        }
        public Task Handle(TransferCreatedEvent @event)
        {
            var transferLog = new TransferLog(){
                FromAccount = @event.From,
                ToAccount = @event.To,
                TransferAmount = @event.Amount
            };
            _repository.Add(transferLog);
            return Task.CompletedTask;
        }
    }
}
