using System.Text.Json.Serialization;

namespace TransactionsManagementProject.WebApi.Models
{
    public class TransformationModel
    {
        public string EntryType { get; set; }

        public string ExitType { get; set; }

        public decimal Ratio { get; set; }
    }
}
