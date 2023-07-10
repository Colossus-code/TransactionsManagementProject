using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionsManagementProject.Contracts.DomainEntities
{
    public class ProductTransactions
    {
        public string Id { get; set; }

        public List<decimal> IndivualCosts { get; set; }

        public decimal TotalCost { get; set; }
    }
}
