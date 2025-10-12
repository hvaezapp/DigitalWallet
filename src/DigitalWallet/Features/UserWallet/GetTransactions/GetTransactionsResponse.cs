using DigitalWallet.Features.Transactions.Common;

namespace DigitalWallet.Features.UserWallet.GetTransactions;

public record GetTransactionsResponse
(
  DateTime CreatedOn,
  string Descripiton,
  string TypeName,
  string KindName
);

