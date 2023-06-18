using CQRS.Ordering.Domain.SeedWork;
using CQRS.Ordering.Domain.SeedWork.CQRS.Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class Buyer : Entity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }
        public string Name { get; private set; }
        protected Buyer()
        { }
        public Buyer(string identity, string name) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity :
            throw new ArgumentNullException(nameof(identity));
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new
            ArgumentNullException(nameof(name));
        }
    }
}
