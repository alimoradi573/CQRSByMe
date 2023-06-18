﻿
using CQRS.Ordering.Domain.AggregatesModel.OrderAggregate;
using MediatR;

namespace CQRS.Ordering.Domain.Events
{
    public class OrderStartedDomainEvent : INotification
    {
        public string UserId { get; }
        public string UserName { get; }
        public Order Order { get; }
        public OrderStartedDomainEvent(Order order, string userId, string userName)
        {
            Order = order;
            UserId = userId;
            UserName = userName;
        }
    }
}
