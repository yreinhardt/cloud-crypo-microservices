namespace Microservice.Coin.Models;

public class CodeAdditionsDeletions4Weeks
{
    public object additions { get; set; }
    public object deletions { get; set; }
}

public class CommunityData
{
    public object facebook_likes { get; set; }
    public int twitter_followers { get; set; }
    public double reddit_average_posts_48h { get; set; }
    public double reddit_average_comments_48h { get; set; }
    public int reddit_subscribers { get; set; }
    public string reddit_accounts_active_48h { get; set; }
}

public class CurrentPriceHist
{
    public double aed { get; set; }
    public double ars { get; set; }
    public double aud { get; set; }
    public double bch { get; set; }
    public double bdt { get; set; }
    public double bhd { get; set; }
    public double bmd { get; set; }
    public double brl { get; set; }
    public double btc { get; set; }
    public double cad { get; set; }
    public double chf { get; set; }
    public double clp { get; set; }
    public double cny { get; set; }
    public double czk { get; set; }
    public double dkk { get; set; }
    public double eth { get; set; }
    public double eur { get; set; }
    public double gbp { get; set; }
    public double hkd { get; set; }
    public double huf { get; set; }
    public double idr { get; set; }
    public double ils { get; set; }
    public double inr { get; set; }
    public double jpy { get; set; }
    public double krw { get; set; }
    public double kwd { get; set; }
    public double lkr { get; set; }
    public double ltc { get; set; }
    public double mmk { get; set; }
    public double mxn { get; set; }
    public double myr { get; set; }
    public double ngn { get; set; }
    public double nok { get; set; }
    public double nzd { get; set; }
    public double php { get; set; }
    public double pkr { get; set; }
    public double pln { get; set; }
    public double rub { get; set; }
    public double sar { get; set; }
    public double sek { get; set; }
    public double sgd { get; set; }
    public double thb { get; set; }
    public double @try { get; set; }
    public double twd { get; set; }
    public double uah { get; set; }
    public double usd { get; set; }
    public double vef { get; set; }
    public double vnd { get; set; }
    public double xag { get; set; }
    public double xau { get; set; }
    public double xdr { get; set; }
    public double zar { get; set; }
    public double bits { get; set; }
    public double link { get; set; }
    public double sats { get; set; }
}

public class DeveloperData
{
    public int forks { get; set; }
    public int stars { get; set; }
    public int subscribers { get; set; }
    public int total_issues { get; set; }
    public int closed_issues { get; set; }
    public int pull_requests_merged { get; set; }
    public int pull_request_contributors { get; set; }
    public CodeAdditionsDeletions4Weeks code_additions_deletions_4_weeks { get; set; }
    public int commit_count_4_weeks { get; set; }
}

public class ImageHist
{
    public string thumb { get; set; }
    public string small { get; set; }
}

public class MarketCapHist
{
    public double aed { get; set; }
    public double ars { get; set; }
    public double aud { get; set; }
    public double bch { get; set; }
    public double bdt { get; set; }
    public double bhd { get; set; }
    public double bmd { get; set; }
    public double brl { get; set; }
    public double btc { get; set; }
    public double cad { get; set; }
    public double chf { get; set; }
    public double clp { get; set; }
    public double cny { get; set; }
    public double czk { get; set; }
    public double dkk { get; set; }
    public double eth { get; set; }
    public double eur { get; set; }
    public double gbp { get; set; }
    public double hkd { get; set; }
    public double huf { get; set; }
    public double idr { get; set; }
    public double ils { get; set; }
    public double inr { get; set; }
    public double jpy { get; set; }
    public double krw { get; set; }
    public double kwd { get; set; }
    public double lkr { get; set; }
    public double ltc { get; set; }
    public double mmk { get; set; }
    public double mxn { get; set; }
    public double myr { get; set; }
    public double ngn { get; set; }
    public double nok { get; set; }
    public double nzd { get; set; }
    public double php { get; set; }
    public double pkr { get; set; }
    public double pln { get; set; }
    public double rub { get; set; }
    public double sar { get; set; }
    public double sek { get; set; }
    public double sgd { get; set; }
    public double thb { get; set; }
    public double @try { get; set; }
    public double twd { get; set; }
    public double uah { get; set; }
    public double usd { get; set; }
    public double vef { get; set; }
    public double vnd { get; set; }
    public double xag { get; set; }
    public double xau { get; set; }
    public double xdr { get; set; }
    public double zar { get; set; }
    public double bits { get; set; }
    public double link { get; set; }
    public double sats { get; set; }
}

public class MarketDataHist
{
    public CurrentPriceHist current_price { get; set; }
    public MarketCapHist market_cap { get; set; }
    public TotalVolumeHist total_volume { get; set; }
}

public class PublicInterestStats
{
    public int alexa_rank { get; set; }
    public object bing_matches { get; set; }
}

public class HistoricalData
{
    public string id { get; set; }
    public string symbol { get; set; }
    public string name { get; set; }
    public ImageHist image { get; set; }
    public MarketDataHist market_data { get; set; }
    public CommunityData community_data { get; set; }
    public DeveloperData developer_data { get; set; }
    public PublicInterestStats public_interest_stats { get; set; }
}

public class TotalVolumeHist
{
    public double aed { get; set; }
    public double ars { get; set; }
    public double aud { get; set; }
    public double bch { get; set; }
    public double bdt { get; set; }
    public double bhd { get; set; }
    public double bmd { get; set; }
    public double brl { get; set; }
    public double btc { get; set; }
    public double cad { get; set; }
    public double chf { get; set; }
    public double clp { get; set; }
    public double cny { get; set; }
    public double czk { get; set; }
    public double dkk { get; set; }
    public double eth { get; set; }
    public double eur { get; set; }
    public double gbp { get; set; }
    public double hkd { get; set; }
    public double huf { get; set; }
    public double idr { get; set; }
    public double ils { get; set; }
    public double inr { get; set; }
    public double jpy { get; set; }
    public double krw { get; set; }
    public double kwd { get; set; }
    public double lkr { get; set; }
    public double ltc { get; set; }
    public double mmk { get; set; }
    public double mxn { get; set; }
    public double myr { get; set; }
    public double ngn { get; set; }
    public double nok { get; set; }
    public double nzd { get; set; }
    public double php { get; set; }
    public double pkr { get; set; }
    public double pln { get; set; }
    public double rub { get; set; }
    public double sar { get; set; }
    public double sek { get; set; }
    public double sgd { get; set; }
    public double thb { get; set; }
    public double @try { get; set; }
    public double twd { get; set; }
    public double uah { get; set; }
    public double usd { get; set; }
    public double vef { get; set; }
    public double vnd { get; set; }
    public double xag { get; set; }
    public double xau { get; set; }
    public double xdr { get; set; }
    public double zar { get; set; }
    public double bits { get; set; }
    public double link { get; set; }
    public double sats { get; set; }
}