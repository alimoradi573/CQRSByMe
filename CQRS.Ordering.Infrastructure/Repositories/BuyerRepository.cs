using CQRS.Ordering.Domain.AggregatesModel.BuyerAggregate;
using CQRS.Ordering.Domain.SeedWork;
using CQRS.Ordering.Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Infrastructure.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly OrderingContext _context;
        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }
        public BuyerRepository(OrderingContext context)
        {
            _context = context ?? throw new
            ArgumentNullException(nameof(context));
        }
        public Buyer Add(Buyer buyer)
        {
            if (buyer.IsTransient())
            {
                return _context.Buyers
                .Add(buyer)
                .Entity;
            }
            else
            {
                return buyer;
            }
        }
        public Buyer Update(Buyer buyer)
        {
            return _context.Buyers
            .Update(buyer)
            .Entity;
        }
        public async Task<Buyer> FindAsync(string identity)
        {
            var buyer = await _context.Buyers
            .Where(b => b.IdentityGuid == identity)
            .SingleOrDefaultAsync();
            return buyer;
        }
        public async Task<Buyer> FindByIdAsync(string id)
        {
            var buyer = await _context.Buyers
            .Where(b => b.Id == int.Parse(id))
            .SingleOrDefaultAsync();
            return buyer;
        }
    }
}
