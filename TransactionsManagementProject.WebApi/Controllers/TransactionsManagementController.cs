using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using TransactionsManagementProject.Contracts.DomainEntities;
using TransactionsManagementProject.Contracts.ServiceContracts;
using TransactionsManagementProject.WebApi.Models;

namespace TransactionsManagementProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsManagementController : ControllerBase
    {

        private readonly ITransactionsManagementService _transactionsManagementService;

        private readonly ILogger<TransactionsManagementController> _logger; 

        public TransactionsManagementController(ITransactionsManagementService transactionService, ILogger<TransactionsManagementController> logger)
        {
         
            _logger = logger; 

            _transactionsManagementService = transactionService;
        }

        [HttpGet]
        [Route("GetAllTransactions")]
        public async Task<IActionResult> GetAllTransformations()
        {
            try
            {
                List<TransformationModel> response = new List<TransformationModel>();

                List<Transformation> transformations = await _transactionsManagementService.GetAllTransformations();

                foreach(Transformation transformation in transformations) 
                {

                    response.Add(new TransformationModel
                    {
                        EntryType = transformation.EntryType,
                        ExitType = transformation.ExitType,
                        Ratio = transformation.Ratio,
                    }); 
                
                };

                return Ok(JsonSerializer.Serialize(response));

            }catch(Exception ex)
            {
                _logger.LogError($"Error happened: {ex.Message}");
                return StatusCode(500, "Unexpected Error");
            }

        }        
        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                List<ProductModel> response = new List<ProductModel>();

                List<Product> products = await _transactionsManagementService.GetAllProducts();

                foreach (Product product in products)
                {

                    response.Add(new ProductModel
                    {
                        Id = product.Id,
                        Currency = product.Currency,
                        Quantity = product.Quantity,    
                    });

                };

                return Ok(JsonSerializer.Serialize(response));

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error happened: {ex.Message}");
                return StatusCode(500, "Unexpected Error");
            }
        }

        [HttpPost]
        [Route("GetTransactionsById")]
        public async Task<IActionResult> GetTransactionsById([Required] string id)
        {
            try
            {
                

                ProductTransactions productTransactions = await _transactionsManagementService.GetTransactionsById(id);

                if(productTransactions == null)
                {
                    return BadRequest($"Not found {id}");

                }

                if(productTransactions.TotalCost == 0)
                {
                    return BadRequest($"Not found transactions for the ID {id}");
                }

                ProductTransactionsModel response = new ProductTransactionsModel
                {
                    Id = productTransactions.Id,
                    IndivualCost = productTransactions.IndivualCosts,
                    TotalCost = productTransactions.TotalCost
                };

                return Ok(JsonSerializer.Serialize(response));

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error happened: {ex.Message}");
                return StatusCode(500, "Unexpected Error");
            }
        }
    }
}
