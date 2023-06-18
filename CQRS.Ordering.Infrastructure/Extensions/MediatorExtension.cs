﻿using CQRS.Ordering.Domain.SeedWork.CQRS.Ordering.Domain.SeedWork;
using CQRS.Ordering.Infrastructure.DataContexts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Infrastructure.Extensions
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator
        mediator, OrderingContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null &&
            x.Entity.DomainEvents.Any());
            var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
            domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents) 
        await mediator.Publish(domainEvent);
        }
    }
}
