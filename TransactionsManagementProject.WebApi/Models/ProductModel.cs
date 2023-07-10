using System.Text.Json.Serialization;

namespace TransactionsManagementProject.WebApi.Models
{
    public class ProductModel
    {

        public string Id { get; set; }

        public decimal Quantity { get; set; }

        public string Currency { get; set; }
    }
}
