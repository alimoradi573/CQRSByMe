﻿using CQRS.Ordering.Infrastructure.DTOs;
using CQRS.Ordering.Infrastructure.Models;
using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;

namespace CQRS.Ordering.Application.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderQueryDTO>>
    {
        private readonly ConnectionStringModel _connectionString;
        public GetOrderByIdQueryHandler(ConnectionStringModel connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<Result<OrderQueryDTO>> Handle(GetOrderByIdQuery
        request, CancellationToken cancellationToken)
        {
            using (var connection = new
            SqlConnection(_connectionString.Value))
            {
                connection.Open();
                var result = await connection.QueryAsync<dynamic>(
                @"select o.[Id] as ordernumber,o.OrderDate as date,
o.Description as description,
o.Address_City as city, o.Address_Country as
country, o.Address_Street as street,
o.Address_ZipCode as zipcode,os.Name as status,
oi.ProductName as productname, oi.Units as units,
oi.UnitPrice as unitprice, oi.PictureUrl as
pictureurl
FROM ordering.Orders o
LEFT JOIN ordering.Orderitems oi ON o.Id = oi.orderid
LEFT JOIN ordering.orderstatus os on o.OrderStatusId
= os.Id
WHERE o.Id=@id"
                , new { request.Id }
                );
                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();
                return MapOrderItems(result);
            }
        }
        private Result<OrderQueryDTO> MapOrderItems(dynamic result)
        {
            var order = new OrderQueryDTO
            {
                Ordernumber = result[0].ordernumber,
                Date = result[0].date,
                Status = result[0].status,
                Description = result[0].description,
                Street = result[0].street,
                City = result[0].city,
                Zipcode = result[0].zipcode,
                Country = result[0].country,
                Orderitems = new List<Orderitem>(),
                Total = 0
            };
            foreach (dynamic item in result)
            {
                var orderitem = new Orderitem
                {
                    Productname = item.productname,
                    Units = item.units,
                    Unitprice = (double)item.unitprice,
                    Pictureurl = item.pictureurl
                };
                order.Total += item.units * item.unitprice;
                order.Orderitems.Add(orderitem);
            }
            return Result.Success(order);
        }
    }
}
