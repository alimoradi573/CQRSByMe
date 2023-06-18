using CQRS.Ordering.Application.Commands;
using CQRS.Ordering.Application.Queries;
using CQRS.Ordering.Infrastructure.DTOs;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Ordering.API.v1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new
            ArgumentNullException(nameof(mediator));
        }
        [
        Route("Order")]
        [HttpPost]
        public async Task<ActionResult<Result>>
        CreateOrderFromBasketDataAsync([FromBody] OrderDTO orderDTO)
        {
            var createOrderCommand = new CreateOrderCommand(orderDTO.Items,
            orderDTO.UserId, orderDTO.UserName, orderDTO.City,
            orderDTO.Street, orderDTO.Country, orderDTO.ZipCode);
            var result = await _mediator.Send(createOrderCommand);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetOrderByIdAsync([FromRoute] int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(order.Value);
        }
    }

}