﻿using CQRS.Ordering.Domain.AggregatesModel.OrderAggregate;
using CSharpFunctionalExtensions;
using MediatR;

namespace CQRS.Ordering.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result>
    {
        private readonly IOrderRepository _orderRepository;
        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result> Handle(CreateOrderCommand message,
          CancellationToken cancellationToken)
        {
            var address = new Address(message.Street, message.City,
            message.Country, message.ZipCode);
            var order = new Order(message.UserId, message.UserName, address);
            foreach (var item in message.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName,
                item.UnitPrice, item.Discount, item.PictureUrl, item.Units);
            }
            _orderRepository.Add(order);
            await
            _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken
            );
            return Result.Success();
        }
    }
}
