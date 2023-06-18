using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Infrastructure.Models
{
    public sealed class ConnectionStringModel
    {
        public ConnectionStringModel(string value)
        {
            Value = value;
        }
        public string Value { get; }
    }
}
