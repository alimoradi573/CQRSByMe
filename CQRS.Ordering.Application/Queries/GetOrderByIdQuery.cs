using CQRS.Ordering.Infrastructure.DTOs;
using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Application.Queries
{
    public class GetOrderByIdQuery : IRequest<Result<OrderQueryDTO>>
    {
        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; }
    }
}
