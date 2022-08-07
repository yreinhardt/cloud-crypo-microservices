namespace Microservice.Report.Models;

public class CoinModel
{
    public Dictionary<string, Dictionary<string, float>> Assets;
}

/* 
 {
  "bitcoin": {
    "usd": 23005,
    "eur": 22582
  },
  "cardano": {
    "usd": 0.517365,
    "eur": 0.50785
  },
  "ethereum": {
    "usd": 1684.98,
    "eur": 1653.99
  }
}
*/