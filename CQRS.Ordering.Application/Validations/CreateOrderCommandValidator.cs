using CQRS.Ordering.Infrastructure.DTOs;
using CQRS.Ordering.Infrastructure.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Application.Validations
{
    public class CreateOrderCommandValidator : AbstractValidator<OrderDTO>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.City).NotEmpty();
            RuleFor(command => command.Street).NotEmpty(); 
            RuleFor(command => command.Country).NotEmpty();
            RuleFor(command => command.ZipCode).NotEmpty();
            RuleFor(command =>
            command.Items).Must(ContainOrderItems).WithMessage("No order items found");
        }
        private bool ContainOrderItems(IEnumerable<BasketItemModel> orderItems)
        {
            return orderItems.Any();
        }
    }
}
