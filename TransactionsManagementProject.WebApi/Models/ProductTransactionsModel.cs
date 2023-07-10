namespace TransactionsManagementProject.WebApi.Models
{
    public class ProductTransactionsModel
    {
        public string Id { get; set; }

        public List<decimal> IndivualCost { get; set; }   

        public decimal TotalCost { get; set; }  
    }
}
