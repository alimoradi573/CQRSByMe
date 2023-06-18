using CQRS.Ordering.Infrastructure.DTOs;
using CQRS.Ordering.Infrastructure.Models;
using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Application.Commands
{
    public sealed partial class CreateOrderCommand : IRequest<Result>
    {
        private readonly List<OrderItemDTO> _orderItems;
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }
        public IEnumerable<OrderItemDTO> OrderItems => _orderItems;
        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemDTO>();
        }
        public CreateOrderCommand(List<BasketItemModel> basketItems, string
        userId, string userName, string city, string street, string
        country, string zipcode) : this()
        {
            _orderItems = basketItems.Select(item => new OrderItemDTO()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl,
                UnitPrice = item.UnitPrice,
                Units = item.Quantity
            }).ToList();
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            Country = country;
            ZipCode = zipcode;
        }
    }

}
