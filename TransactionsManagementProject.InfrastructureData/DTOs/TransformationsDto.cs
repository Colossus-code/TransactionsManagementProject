using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TransactionsManagementProject.InfrastructureData.DTOs
{
    public class TransformationsDto
    {
        [JsonPropertyName("from")]
        public string EntryType { get; set; }
        [JsonPropertyName("to")]
        public string ExitType { get; set; }
        [JsonPropertyName("rate")]
        public string Ratio { get; set; }
    }
}
