# Hawalaty System - TurkeyLibya Transfer System

A comprehensive Windows desktop application for managing money transfers between Libya and Turkey. Built with WPF (Windows Presentation Foundation) and .NET 8, featuring a modern Material Design interface with full Arabic and English language support.

## ✨ Features

### 🎛️ Dashboard
- **Live Statistics**: Real-time display of total incoming/outgoing transfers and current balances
- **Transfer Summaries**: Daily, weekly, and monthly status reports
- **Recent Activity**: Quick view of latest transfers
- **Quick Reports**: One-click generation of Excel reports

### 💸 Transfer Management
- **New Transfer Entry**: Complete transfer form with auto-generated transfer IDs
- **Transfer Tracking**: Monitor transfer status from pending to completion
- **Payout & Collection**: Handle receiving side confirmations
- **PDF Receipts**: Professional transfer receipts with company branding

### 👥 Agents & Branch Management
- **Multi-Country Support**: Manage agents in both Libya and Turkey
- **Agent Profiles**: Complete contact information and daily limits
- **Role-Based Access**: Customizable permissions for different user types
- **Balance Tracking**: Monitor agent balances by currency

### 📊 Advanced Reports & Analytics
- **Flexible Filtering**: Generate reports by date, currency, country, agent, or status
- **Multiple Export Formats**: Export data to Excel or PDF
- **Financial Insights**: Profit/loss analysis based on exchange rate differences
- **Custom Date Ranges**: Historical data analysis with custom periods

### 💱 Currency & Exchange Management
- **Multi-Currency Support**: Handle TRY, LYD, and USD transactions
- **Daily Rate Updates**: Manual input or optional auto-fetch from external sources
- **Exchange Rate History**: Track rate changes over time
- **Profit Analysis**: Calculate earnings from exchange rate margins

### 🔐 Security & User Management
- **Multi-User System**: Support for Admin, Accountant, Agent, Manager, and Viewer roles
- **Secure Authentication**: BCrypt password hashing with session management
- **Cross-Border Access**: Users can log in from both Libya and Turkey
- **Role-Based Permissions**: Granular access control for different features
- **Automatic Backups**: Daily system backup functionality

### 🌐 Internationalization
- **Bilingual Interface**: Complete Arabic and English language support
- **RTL Support**: Proper right-to-left text rendering for Arabic
- **Cultural Formatting**: Date, number, and currency formatting per locale
- **Dynamic Language Switching**: Change language without restart

## 🛠️ Technology Stack

- **Framework**: .NET 8 with WPF (Windows Presentation Foundation)
- **Database**: SQLite with Entity Framework Core
- **UI Library**: Material Design In XAML Toolkit
- **Architecture**: MVVM (Model-View-ViewModel) Pattern
- **PDF Generation**: iTextSharp
- **Excel Export**: ClosedXML
- **Security**: BCrypt.Net for password hashing

## 📋 System Requirements

- **Operating System**: Windows 10 version 1809 or later
- **Runtime**: .NET 8.0 Desktop Runtime
- **Memory**: 4 GB RAM minimum, 8 GB recommended
- **Storage**: 500 MB free space for application and database
- **Display**: 1024x768 minimum resolution, 1920x1080 recommended

## 🚀 Installation

### Prerequisites
1. Install [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Ensure Windows is up to date

### Build from Source
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd HawalatySystem
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the application:
   ```bash
   dotnet build --configuration Release
   ```

4. Run the application:
   ```bash
   dotnet run --project HawalatySystem
   ```

### Visual Studio Setup
1. Open `HawalatySystem.sln` in Visual Studio 2022
2. Right-click the solution and select "Restore NuGet Packages"
3. Set `HawalatySystem` as the startup project
4. Press F5 to build and run

## 👤 Default Credentials

**Administrator Account:**
- Username: `admin`
- Password: `admin123`

> ⚠️ **Important**: Change the default password immediately after first login

## 🎯 Usage Guide

### First Time Setup
1. Launch the application
2. Log in with default credentials
3. Change the admin password in Settings
4. Set up exchange rates for current day
5. Create agent accounts for Libya and Turkey offices
6. Create user accounts for staff members

### Daily Operations
1. **Update Exchange Rates**: Start each day by updating currency rates
2. **Process New Transfers**: Use the "New Transfer" screen for incoming requests
3. **Monitor Dashboard**: Check statistics and pending transfers
4. **Handle Completions**: Mark transfers as completed when payment is confirmed
5. **Generate Reports**: Create daily/weekly reports for management

### Transfer Workflow
1. **Receive Transfer Request**: Customer provides sender/receiver details
2. **Create Transfer**: Enter details in "New Transfer" form
3. **Generate Receipt**: Print PDF receipt for customer
4. **Notify Receiving Agent**: Send transfer details to destination
5. **Confirm Completion**: Mark transfer complete when recipient collects

### User Management
1. **Admin Users**: Can access all features and manage other users
2. **Accountants**: Handle transfers and generate financial reports
3. **Agents**: Process transfers within their assigned regions
4. **Managers**: View reports and monitor operations
5. **Viewers**: Read-only access to transfer data

## 🗂️ Database Structure

The application uses SQLite with the following main entities:
- **Users**: System user accounts with roles and permissions
- **Transfers**: Core transfer transactions with full audit trail
- **Agents**: Regional agents handling transfers
- **ExchangeRates**: Daily currency exchange rates
- **AgentBalances**: Current balances per agent and currency

## 🔧 Configuration

### Database Location
By default, the SQLite database (`hawalaty.db`) is created in the application directory. The database is automatically initialized on first run with default data.

### Backup Strategy
- Manual backups can be created by copying the `hawalaty.db` file
- Implement automated backup scripts for production deployment
- Store backups in secure, offsite locations

### Security Considerations
- All passwords are hashed using BCrypt
- User sessions are managed securely
- Database contains sensitive financial data - ensure proper file permissions
- Consider encrypting the database file for enhanced security

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/new-feature`
3. Commit changes: `git commit -am 'Add new feature'`
4. Push to branch: `git push origin feature/new-feature`
5. Submit a Pull Request

## 📄 License

This project is proprietary software. All rights reserved.

## 🆘 Support

For technical support or questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation in the `/docs` folder

## 🔄 Updates & Maintenance

- **Regular Updates**: Check for application updates monthly
- **Database Maintenance**: Run database optimization quarterly
- **Security Patches**: Apply security updates immediately
- **Backup Verification**: Test backup restore procedures regularly

---

**Hawalaty System** - Streamlining money transfers between Turkey and Libya with modern technology and robust security.
