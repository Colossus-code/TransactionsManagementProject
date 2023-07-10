using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TransactionsManagementProject.InfrastructureData.DTOs
{
    public class ProductsDto
    {
        [JsonPropertyName("sku")]
        public string Id { get; set; }
        [JsonPropertyName("amount")]
        public string Quantity { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
