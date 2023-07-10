using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionsManagementProject.Contracts.DomainEntities
{
    public class Transformation
    {
        public string EntryType { get; set; }

        public string ExitType { get; set; }

        public decimal Ratio { get; set; }
    }
}
