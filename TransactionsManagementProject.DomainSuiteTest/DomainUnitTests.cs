using Moq;
using TransactionsManagementProject.Contracts.DomainEntities;
using TransactionsManagementProject.Contracts.RepositoryContracts;
using TransactionsManagementProject.Implementations;

namespace TransactionsManagementProject.DomainSuiteTest
{
    public class DomainUnitTests
    {
        private readonly Mock<IRepositoryCache> _mockCache = new Mock<IRepositoryCache> ();
        private readonly Mock<IRepositoryProducts> _mockRepositoryProducts = new Mock<IRepositoryProducts> ();
        private readonly Mock<IRepositoryTransformations> _mockRepositoryTransformations = new Mock<IRepositoryTransformations> ();
        private readonly TransactionsManagementService _transactionsManagementService; 

        public DomainUnitTests()
        {
            _transactionsManagementService = new TransactionsManagementService(_mockCache.Object, _mockRepositoryProducts.Object, _mockRepositoryTransformations.Object);

        }
               
       [Fact]
        public async void Assert_ProductsItsWhatExpected_When_FoundInCache()
        {
            // Arrange
            List<Product> products = GetProducts();

            _mockCache.Setup(e => e.GetCacheList<Product>(It.IsAny<string>())).Returns(products);

            //Act

            List<Product> result =  await _transactionsManagementService.GetAllProducts();

            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(products.First().Id, result.First().Id);
        }
        [Fact]
        public async void Assert_TransformationsItsWhatExpected_When_FoundInCache()
        {
            // Arrange
            List<Transformation> transformations = GetTransformations();

            _mockCache.Setup(e => e.GetCacheList<Transformation>(It.IsAny<string>())).Returns(transformations);


            //Act

            List<Transformation> result = await _transactionsManagementService.GetAllTransformations();

            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(transformations.First().EntryType, result.First().EntryType);

        }



        [Fact]
        public async void Assert_ProductsItsExpectedOnes_When_FoundInApi()
        {
            // Arrange

            List<Product> products = GetProducts();

            _mockCache.Setup(e => e.GetCacheList<Product>(It.IsAny<string>())).Returns(new List<Product>());  

            _mockRepositoryProducts.Setup(e => e.GetProducts()).ReturnsAsync(products);


            //Act

            List<Product> result = await _transactionsManagementService.GetAllProducts();

            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(products.First().Id, result.First().Id);

        }
        [Fact]
        public async void Assert_TransformationsItsExpectedOnes_When_FoundInApi()
        {

            // Arrange
            List<Transformation> transformations = GetTransformations();

            _mockCache.Setup(e => e.GetCacheList<Transformation>(It.IsAny<string>())).Returns(new List<Transformation>());

            _mockRepositoryTransformations.Setup(e => e.GetTransformations()).ReturnsAsync(transformations);

            //Act

            List<Transformation> result = await _transactionsManagementService.GetAllTransformations();

            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(transformations.First().EntryType, result.First().EntryType);


        }

        [Fact]
        public async void Assert_ProductsItsExpectedOnes_When_FoundInFile()
        {
            // Arrange

            List<Product> products = GetProducts();

            _mockCache.Setup(e => e.GetCacheList<Product>(It.IsAny<string>())).Returns(new List<Product>());

            _mockRepositoryProducts.Setup(e => e.GetProducts()).ReturnsAsync(new List<Product>());

            _mockRepositoryProducts.Setup(e => e.GetProductsInFile()).Returns(products);


            //Act

            List<Product> result = await _transactionsManagementService.GetAllProducts();

            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(products.First().Id, result.First().Id);

        }
        [Fact]
        public async void Assert_TransformationsItsExpectedOnes_When_FoundInFile()
        {

            // Arrange
            List<Transformation> transformations = GetTransformations();

            _mockCache.Setup(e => e.GetCacheList<Transformation>(It.IsAny<string>())).Returns(new List<Transformation>());

            _mockRepositoryTransformations.Setup(e => e.GetTransformations()).ReturnsAsync(new List<Transformation>());

            _mockRepositoryTransformations.Setup(e => e.GetTransformationsInFile()).Returns(transformations);

            //Act

            List<Transformation> result = await _transactionsManagementService.GetAllTransformations();

            //Assert

            Assert.NotEmpty(result);
            Assert.Equal(transformations.First().EntryType, result.First().EntryType);


        }

        [Fact]
        public async void Assert_ProductTransactionCostEqualsWhatExpected_When_FoundInCache()
        {

            // Arrange
            ProductTransactions productTransactions = GetProductTransactions();

            _mockCache.Setup(e => e.GetCache<ProductTransactions>(It.IsAny<string>(),null)).Returns(productTransactions);


            //Act

            ProductTransactions result = await _transactionsManagementService.GetTransactionsById(productTransactions.Id);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(productTransactions.TotalCost, result.TotalCost);


        }
        [Fact]
        public async void Assert_ProductTransactionCostEqualsWhatExpected_When_NeedToTransform()
        {

            // Arrange
            List<Transformation> transformations = GetTransformations();
            List<Product> products = GetProducts();
            ProductTransactions productTransactions = GetProductTransactions();

            _mockCache.Setup(e => e.GetCache<ProductTransactions>(It.IsAny<string>(), null)).Returns(new ProductTransactions());

            _mockRepositoryTransformations.Setup(e => e.GetTransformations()).ReturnsAsync(transformations);

            _mockRepositoryProducts.Setup(e => e.GetProducts()).ReturnsAsync(products);

            //Act

            ProductTransactions result = await _transactionsManagementService.GetTransactionsById("002");

            //Assert

            Assert.NotNull(result);
            Assert.Equal(productTransactions.TotalCost, result.TotalCost);


        }
        [Fact]
        public async void Assert_ProductTransactionCostEqualsWhatExpected_When_NoNeedToTransform()
        {

            List<Transformation> transformations = GetTransformations();
            List<Product> products = GetProducts();

            _mockCache.Setup(e => e.GetCache<ProductTransactions>(It.IsAny<string>(), null)).Returns(new ProductTransactions());

            _mockRepositoryTransformations.Setup(e => e.GetTransformations()).ReturnsAsync(transformations);

            _mockRepositoryProducts.Setup(e => e.GetProducts()).ReturnsAsync(products);

            //Act

            ProductTransactions result = await _transactionsManagementService.GetTransactionsById("001");

            //Assert

            Assert.NotNull(result);
            Assert.Equal(1m, result.TotalCost);


        }
        [Fact]
        public async void Assert_ProductTransactionCostEqualsWhatExpected_When_NotHavePrincipalTransformation()
        {

            List<Transformation> transformations = GetTransformations();
            List<Product> products = GetProducts();
            ProductTransactions productTransactions = new ProductTransactions
            {
                Id = "003",
                TotalCost = 1.02m
            };

            _mockCache.Setup(e => e.GetCache<ProductTransactions>(It.IsAny<string>(), null)).Returns(new ProductTransactions());

            _mockRepositoryTransformations.Setup(e => e.GetTransformations()).ReturnsAsync(transformations);

            _mockRepositoryProducts.Setup(e => e.GetProducts()).ReturnsAsync(products);

            //Act

            ProductTransactions result = await _transactionsManagementService.GetTransactionsById(productTransactions.Id);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(productTransactions.TotalCost, result.TotalCost);


        }


        private List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = "001",
                    Quantity = 1,
                    Currency = "EUR"
                },
                new Product
                {
                    Id = "002",
                    Quantity = 1,
                    Currency = "USD"
                },               
                new Product
                {
                    Id = "003",
                    Quantity = 1,
                    Currency = "YEN"
                }
            };
        }

        private List<Transformation> GetTransformations()
        {
            return new List<Transformation>
            {
                new Transformation
                {
                    EntryType = "USD",
                    ExitType = "EUR",
                    Ratio = 0.736m
                },                
                new Transformation
                {
                    EntryType = "YEN",
                    ExitType = "USD",
                    Ratio = 1.392m
                }
            };
        }

        private ProductTransactions GetProductTransactions()
        {
            return new ProductTransactions
            {
                Id = "002",
                IndivualCosts = new List<decimal>
                {
                    0.736m
                },
                TotalCost = 0.74m
            };
        }
    }
}