using CQRS.Ordering.Domain.AggregatesModel.BuyerAggregate;
using CQRS.Ordering.Domain.AggregatesModel.OrderAggregate;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CQRS.Ordering.Domain.SeedWork;
using CQRS.Ordering.Infrastructure.EntityConfigurations;
using MediatR;
using CQRS.Ordering.Infrastructure.Extensions;

namespace CQRS.Ordering.Infrastructure.DataContexts
{
    public class OrderingContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "Ordering";
        private IDbContextTransaction _currentTransaction;
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        private IMediator _mediator;
        public OrderingContext(DbContextOptions<OrderingContext> options, IMediator
        mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new
            OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new
            OrderItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new
            OrderStatusEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new
            BuyerEntityTypeConfiguration());
        }

        public IDbContextTransaction GetCurrentTransaction() =>
        _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;
            _currentTransaction = await
            Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            return _currentTransaction;
        }
        public async Task CommitTransactionAsync(IDbContextTransaction
        transaction)
        {
            if (transaction == null) throw new
            ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new
            InvalidOperationException($"Transaction   {transaction.TransactionId} is not current");
            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken
        cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
