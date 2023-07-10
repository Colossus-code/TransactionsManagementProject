using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionsManagementProject.Contracts.DomainEntities
{
    public class Product
    {

        public string Id { get; set; }

        public decimal Quantity { get; set; }

        public string Currency { get; set; }
    }
}
