using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsManagementProject.Contracts.DomainEntities;

namespace TransactionsManagementProject.Contracts.ServiceContracts
{
    public interface ITransactionsManagementService
    {
        Task<List<Product>> GetAllProducts();
        Task<List<Transformation>> GetAllTransformations();
        Task<ProductTransactions> GetTransactionsById(string id);
    }
}
