namespace Microservice.Report.Models;

public class PortfolioModel
{
        public string portfolioId { get; set; }
        public string portfolioName { get; set; }
        public DateTime createdOn { get; set; }
        public string userId { get; set; }
        public List<string> assets { get; set; }
}

