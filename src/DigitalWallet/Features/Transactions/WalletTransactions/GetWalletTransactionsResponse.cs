namespace DigitalWallet.Features.Transactions.WalletTransactions;

public record GetWalletTransactionsResponse
(
  DateTime CreatedOn,
  string Descripiton,
  string TypeName,
  string KindName

);

