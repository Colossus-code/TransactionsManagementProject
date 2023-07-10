using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsManagementProject.Contracts.DomainEntities;
using TransactionsManagementProject.Contracts.RepositoryContracts;
using TransactionsManagementProject.InfrastructureData.DTOs;
using TransactionsManagementProject.InfrastructureData.RepositoryHelpers;

namespace TransactionsManagementProject.InfrastructureData
{
    public class RepositoryProducts : IRepositoryProducts
    {
        private readonly string _pathRoot;
        
        private readonly string _pathFile;

        public RepositoryProducts(IConfiguration configuration)
        {
            _pathRoot = configuration.GetSection("ApiCalls:Products").Value;
            
            _pathFile = configuration.GetSection("PathFiles:Products").Value;

        }

        public async Task<List<Product>> GetProducts()
        {
            List<ProductsDto> productsDto = await RepositoryHelper.GetList<ProductsDto>(_pathRoot);

            if (productsDto == null || productsDto.Count == 0) return new List<Product>();

            return TransformDtoToDomain(productsDto);

        }

        public List<Product> GetProductsInFile()
        {
            return RepositoryHelper.GetListFromFile<Product>(_pathFile);
        }

        public void PersistProductsInFile(List<Product> products)
        {
            RepositoryHelper.PersistList<Product>(products, _pathFile);
        }

        private List<Product> TransformDtoToDomain(List<ProductsDto> productsDtos)
        {
            List<Product> products = new List<Product>();   

            foreach(var productDto in productsDtos)
            {
                products.Add(new Product
                {
                    Id = productDto.Id,
                    Currency = productDto.Currency,
                    Quantity = decimal.Parse(productDto.Quantity, CultureInfo.InvariantCulture)
                });
            }

            return products;

        }
    }
}
