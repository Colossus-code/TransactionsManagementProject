using TransactionsManagementProject.Contracts.DomainEntities;
using TransactionsManagementProject.Contracts.RepositoryContracts;
using TransactionsManagementProject.Contracts.ServiceContracts;

namespace TransactionsManagementProject.Implementations
{
    public class TransactionsManagementService : ITransactionsManagementService
    {
        private readonly IRepositoryCache _repositoryCache;
        private readonly IRepositoryProducts _repositoryProducts;
        private readonly IRepositoryTransformations _repositoryTransformations; 


        public TransactionsManagementService(IRepositoryCache repoCache, IRepositoryProducts repoProducts, IRepositoryTransformations repoTransformations)
        {
            _repositoryCache = repoCache;
            _repositoryProducts = repoProducts;
            _repositoryTransformations = repoTransformations;   

        }
        public async Task<List<Product>> GetAllProducts()
        {
            List<Product> productsInCache = _repositoryCache.GetCacheList<Product>("products");

            if(ValidateInformationRecived<Product>(productsInCache)) return productsInCache;

            List<Product> products = await _repositoryProducts.GetProducts();

            if (ValidateInformationRecived<Product>(products))
            {
                _repositoryCache.SetCacheList<Product>("products" , products );

                _repositoryProducts.PersistProductsInFile(products);

                return products;
            }

            List<Product> productsInFile = _repositoryProducts.GetProductsInFile();

            if (ValidateInformationRecived<Product>(productsInFile))
            {
                _repositoryCache.SetCacheList<Product>("products", productsInFile);

                return productsInFile;
            }

            return null; 
        }

        public async Task<List<Transformation>> GetAllTransformations()
        {
            List<Transformation> transformationsInCache = _repositoryCache.GetCacheList<Transformation>("transformations");

            if (ValidateInformationRecived<Transformation>(transformationsInCache)) return transformationsInCache;

            List<Transformation> transformations = await _repositoryTransformations.GetTransformations();

            if (ValidateInformationRecived<Transformation>(transformations))
            {
                _repositoryCache.SetCacheList<Transformation>("transformations", transformations);

                _repositoryTransformations.PersistTransformationInFile(transformations);

                return transformations;

            }

            List<Transformation> transformationsInFile = _repositoryTransformations.GetTransformationsInFile();

            if (ValidateInformationRecived<Transformation>(transformationsInFile))
            {
                _repositoryCache.SetCacheList<Transformation>("transformations", transformations);

                return transformationsInFile;
            }

            return null;
        }

        public async Task<ProductTransactions> GetTransactionsById(string id)
        {
            ProductTransactions productTransactioninCache = _repositoryCache.GetCache<ProductTransactions>(id, null);

            if (productTransactioninCache != null && productTransactioninCache.Id != null) return productTransactioninCache; 

            List<Product> products = await GetAllProducts();

            List<Transformation> transformations = await GetAllTransformations();

            List<Product> filteredProducts = products.Where(e => e.Id == id).ToList();

            ProductTransactions productTransaction =  GetProductTransactions(filteredProducts, transformations);

            _repositoryCache.SetCache<ProductTransactions>(id, productTransaction);

            return productTransaction;

        }

        private ProductTransactions GetProductTransactions(List<Product> products, List<Transformation> transformations) 
        {

            ProductTransactions transactions = new ProductTransactions();

            transactions.IndivualCosts = new List<decimal>();

            transactions.Id = products.FirstOrDefault().Id; 

            foreach(Product product in products) { 

                if(product.Currency != "EUR")
                {
                    List<Transformation> actualTransformations = transformations.Where(e => e.EntryType == product.Currency).ToList();

                    List<Transformation> requiredTransformations = transformations.Where(e => e.ExitType == "EUR").ToList();

                    if(actualTransformations.FirstOrDefault().ExitType != "EUR") 
                    {
                        if (actualTransformations.FirstOrDefault().EntryType != requiredTransformations.FirstOrDefault().EntryType)
                        {
                            Transformation requiredTransformation = actualTransformations.FirstOrDefault(e => e.ExitType == requiredTransformations.FirstOrDefault(i => i.EntryType == e.ExitType).EntryType);

                            product.Quantity = requiredTransformation.Ratio * product.Quantity;

                            if (!(requiredTransformation.ExitType == "EUR"))
                            {
                                Transformation endTransformation = requiredTransformations.Where(e => e.EntryType == requiredTransformation.ExitType && e.ExitType == "EUR").FirstOrDefault();

                                product.Quantity = endTransformation.Ratio * product.Quantity;
                            }
                        }
                        else
                        {
                            product.Quantity = requiredTransformations.FirstOrDefault().Ratio * product.Quantity;
                        }

                    }
                    else
                    {
                        product.Quantity = actualTransformations.FirstOrDefault().Ratio * product.Quantity;
                    }
                                  
                    transactions.IndivualCosts.Add(Math.Round(product.Quantity, 2));

                    transactions.TotalCost += Math.Round(product.Quantity, 2);
                }
                else
                {
                    transactions.IndivualCosts.Add(product.Quantity);

                    transactions.TotalCost += product.Quantity; 
                }
            
            
            }

            return transactions; 
              
        }

        private bool ValidateInformationRecived<T>(List<T> genericList)
        {
            if(genericList != null && genericList.Count > 0) { return true; }

            return false;
        }
    }
}