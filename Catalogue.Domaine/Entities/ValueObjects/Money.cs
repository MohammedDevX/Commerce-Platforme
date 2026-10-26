using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domaine.Entities.ValueObjects
{
    public record Money
    {
        public decimal? Price { get; private set; }

        public Money(decimal? price)
        {
            if (price is not null)
            {
                if (price <= 0)
                    throw new ArgumentException("Price cannot be <= 0", nameof(price));
            }
        }
    }
}
