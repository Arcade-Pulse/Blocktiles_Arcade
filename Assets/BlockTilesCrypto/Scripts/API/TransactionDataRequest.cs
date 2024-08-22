using System;
using System.Collections.Generic;

public class TransactionDataRequest
{
    public List<TransactionData> binance { get; set; }
    public List<TransactionData> coinbase { get; set; }
    public string message { get; set; }
}

public class TransactionData
{
    public string cashoutMethod { get; set; }
    public string email { get; set; }
    public string transactionId { get; set; }
    public double amountInUSD;
    public DateTime transactionDateTime { get; set; }
    public string transactionRequestStatus { get; set; }
    public int reserveCoins { get; set; }
    public string reserveCoinsStatus { get; set; }
    public string transactionQueryStatus { get; set; }
}

// public class Coinbase
// {
//     public string cashoutMethod { get; set; }
//     public string transactionId { get; set; }
//     public DateTime transactionDateTime { get; set; }
//     public string transactionRequestStatus { get; set; }
//     public int reserveCoins { get; set; }
//     public string reserveCoinsStatus { get; set; }
//     public string transactionQueryStatus { get; set; }
// }