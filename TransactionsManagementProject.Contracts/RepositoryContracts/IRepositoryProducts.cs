using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsManagementProject.Contracts.DomainEntities;

namespace TransactionsManagementProject.Contracts.RepositoryContracts
{
    public interface IRepositoryProducts
    {
        Task<List<Product>> GetProducts();
        List<Product> GetProductsInFile();
        void PersistProductsInFile(List<Product> products);
    }
}
