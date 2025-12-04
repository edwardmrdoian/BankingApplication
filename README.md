# 🏦 ATM Banking Console Application (C# .NET 8)

A fully functional ATM console application built with **C# (.NET 8)**.  
The project simulates core ATM operations such as user authentication, registration, balance management, currency wallets, and transaction history.

All account data is stored in a local **JSON file** and loaded at runtime.  
Logging is implemented using **Microsoft.Extensions.Logging**.

---

## 🚀 Features

### 🔐 Authentication System
- Login using:
  - **Card Number**
  - **CVC**
  - **Expiration Date**
  - **PIN**
- Input validation & retry logic  
- Secure step-by-step prompts  
- Exit at any stage by typing `exit`

---

### 🆕 User Registration
During registration, the system automatically generates:

- **16-digit card number** (xxxx-xxxx-xxxx-xxxx)
- **3-digit CVC**
- **Valid expiration date** (expires in +4 years)
- **Unique account ID**

User provides:
- First name  
- Last name  
- 4-digit PIN

New accounts are saved instantly to `accounts.json`.

---

### 💰 Account Management
Each user has wallets for:
- **GEL**
- **USD**
- **EUR**

Supports:
- Deposit
- Withdraw
- Currency balance viewing
- Transfers between own currencies
- Full transaction history

---

### 📝 Persistent Storage
All accounts are saved in:  
Data/accounts.json


The application uses:
- `JsonStorageService` for reading/writing
- Shared reference lists so changes persist immediately

---

### 🛠 Tech Stack

| Component | Description |
|----------|-------------|
| **Language** | C# (.NET 8) |
| **Project Type** | Console Application |
| **Data Storage** | JSON file |
| **Logging** | Microsoft.Extensions.Logging |
| **Architecture** | Service-based (AuthService, AccountService) |
| **Patterns** | Separation of concerns, dependency injection style |

---

## 📂 Project Structure
 BankingApplication/
- │
- ├── Data/
- │ └── accounts.json
- │
- ├── Models/
- │ ├── Account.cs
- │ ├── CardDetails.cs
- │ ├── TransactionHistory.cs
- │
- ├── Services/
- │ ├── AuthService.cs
- │ ├── AccountService.cs
- │ └── JsonStorageService.cs
- │
- ├── Utils/
- │ └── Utils.cs
- │
- ├── Menu/
- │ └── ConsoleMenu.cs
- │
- ├── Logging/
- │ └── AtmLoggerFactory.cs
- │
- ├── Program.cs
- └── README.md

---

## ▶️ How to Run the Project

### 1. Clone the repository
```bash
git clone <your repo url>
cd BankingApplication
```
### 2. Restore dependencies
```bash
dotnet restore
```
### 3. Run the application
```bash
dotnet run
```

## 🧪 Usage Flow
### 🔹 Login
 Enter:
- Card Number
- CVC
- Expiration Date
- PIN
If any step is incorrect, the system asks again.

### 🔹 Registration
 User enters:
- First name
- Last name
- PIN
### Then system generates:
=== REGISTRATION SUCCESSFUL ===
- User ID: 5
- Card Number: 4921-1503-4452-8831
- PIN: 1234
- CVC: 774
- Expiration Date: 02/29
- Then you can login immediately — no restart needed.

### 🔹 ATM Menu
Example options:
1. View Balances
2. Withdraw
3. Deposit
4. Transfer Currency
5. View Transaction History
6. Logout

## 📌 Logging
All activities (login attempts, registration, errors) are logged using:
Logs/atm-log.txt
Including:
- Errors
- Warnings
- User activity
- System events

## 🔒 Security Notes
This project is for educational purposes.
In real banking software:

### ❌ Never store PIN/CVC in plain text
✔ Use hashing & encryption
✔ Follow PCI-DSS standards

## 📄 License
This project is open-source and free to modify.

## ❤️ Contributing
Contributions and improvements are welcome!
Feel free to open issues or PRs.
