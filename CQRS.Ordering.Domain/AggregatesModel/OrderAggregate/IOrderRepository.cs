using CQRS.Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Order Add(Order order);
        void Update(Order order);
        Task<Order> GetAsync(int orderId);
    }
}
