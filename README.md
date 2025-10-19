# DigitalWallet

The **User Wallet Service** is a core platform component, built on **Vertical Slice Architecture (VSA)**, enabling secure and efficient fund management.  
It is developed using **ASP.NET Core 9**, with **SQL Server** as the database. The service uses **memory caching** for optimized performance and employs **Minimal APIs** for lightweight endpoints.

## Features

### 🔹 MultiCurrency
- **CreateCurrency** 
- **UpdateRatio** 
- **GetAll** 

### 🔹 UserWallet
- **CreateWallet** 
- **GetBalance** 
- **Active** 
- **Suspend**
- **Ban**
- **ChangeTitle**
- **GetTransactions**

### 🔹 Transactions
- **IncreaseWalletBalance** 
- **DecreaseWalletBalance**
- **WalletFunds** 
- **WalletTransactions**
