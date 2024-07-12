using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infrastructure.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //Domain Bus
            //services.AddTransient<IMediator, Mediator>(); 
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetRequiredService<IMediator>(), scopeFactory);
            });

            
            
            //Domain Banking Services
            services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();
           
        }
        public static void RegisterBankingServices(IServiceCollection services)
        {
            //Application Services
            services.AddTransient<IAccountService, AccountService>();

            //Data
            services.AddTransient<BankingDbContext>();

            services.AddTransient<IAccountRepository, AccountRepository>();
        }
        public static void RegisterTransferServices(IServiceCollection services)
        {
            services.AddTransient<ITransferService, TransferService> ();
            services.AddTransient<TransferDbContext>();
            services.AddTransient<ITransferRepository, TransferRepository>();

            //Domain events
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();
            //Subscriptions
            services.AddTransient<TransferEventHandler>();
        }
    }
}
