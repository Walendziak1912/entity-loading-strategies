# Shared.Persistance.Common

Biblioteka zawierająca wspólne komponenty konfiguracji dla projektów związanych z persystencją danych.

## Struktura projektu

```
Shared.Persistance.Common/
├── Options/                    # Klasy opcji (Option Pattern)
│   └── DatabaseOptions.cs
├── Providers/                  # Interfejsy i implementacje providerów
│   ├── IDatabaseOptionsProvider.cs
│   └── DatabaseOptionsProvider.cs
├── Extensions/                 # Extension methods do rejestracji w DI
│   └── DatabaseConfigurationExtensions.cs
├── Config/                     # Pliki konfiguracyjne
│   └── appsettings.json
└── Shared.Persistance.Common.csproj
```

## Jak dodać nową konfigurację

### 1. Utwórz klasę opcji w folderze Options/

```csharp
// Options/EmailOptions.cs
namespace Shared.Persistance.Common.Options
{
    public class EmailOptions
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
```

### 2. Utwórz interfejs providera w folderze Providers/

```csharp
// Providers/IEmailOptionsProvider.cs
using Shared.Persistance.Common.Options;

namespace Shared.Persistance.Common.Providers
{
    public interface IEmailOptionsProvider
    {
        EmailOptions Value { get; }
    }
}
```

### 3. Utwórz implementację providera w folderze Providers/

```csharp
// Providers/EmailOptionsProvider.cs
using Microsoft.Extensions.Options;
using Shared.Persistance.Common.Options;

namespace Shared.Persistance.Common.Providers
{
    public class EmailOptionsProvider : IEmailOptionsProvider
    {
        public EmailOptions Value { get; }

        public EmailOptionsProvider(IOptions<EmailOptions> options)
        {
            Value = options.Value;
        }
    }
}
```

### 4. Dodaj extension method w folderze Extensions/

```csharp
// Extensions/EmailConfigurationExtensions.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Persistance.Common.Options;
using Shared.Persistance.Common.Providers;

namespace Shared.Persistance.Common.Extensions
{
    public static class EmailConfigurationExtensions
    {
        public static IServiceCollection AddEmailConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailOptions>(
                configuration.GetSection("Email"));
            
            services.AddSingleton<IEmailOptionsProvider, EmailOptionsProvider>();
            
            return services;
        }
    }
}
```

### 5. Dodaj konfigurację w Config/appsettings.json

```json
{
  "Database": {
    "ConnectionString": "Server=localhost;Database=TwitterDb;Trusted_Connection=true;TrustServerCertificate=true;",
    "DatabaseName": "TwitterDb"
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-password"
  }
}
```

## Użycie w innych projektach

### 1. Dodaj referencję do projektu

```bash
dotnet add reference Shared/Persistance/Common/Shared.Persistance.Common.csproj
```

### 2. Dodaj using statements

```csharp
using Shared.Persistance.Common.Options;
using Shared.Persistance.Common.Providers;
using Shared.Persistance.Common.Extensions;
```

### 3. Zarejestruj w DI

```csharp
// W Program.cs lub Startup.cs
services.AddDatabaseConfiguration(configuration);
services.AddEmailConfiguration(configuration);
```

### 4. Użyj w kodzie

```csharp
public class EmailService
{
    private readonly EmailOptions _emailOptions;
    private readonly DatabaseOptions _dbOptions;

    public EmailService(
        IEmailOptionsProvider emailProvider,
        IDatabaseOptionsProvider dbProvider)
    {
        _emailOptions = emailProvider.Value;
        _dbOptions = dbProvider.Value;
    }
}
```

## Konwencje nazewnictwa

- **Klasy opcji**: `{Nazwa}Options` (np. `DatabaseOptions`, `EmailOptions`)
- **Interfejsy providerów**: `I{Nazwa}OptionsProvider` (np. `IDatabaseOptionsProvider`)
- **Implementacje providerów**: `{Nazwa}OptionsProvider` (np. `DatabaseOptionsProvider`)
- **Extension methods**: `{Nazwa}ConfigurationExtensions` (np. `DatabaseConfigurationExtensions`)
- **Nazwy metod**: `Add{Nazwa}Configuration` (np. `AddDatabaseConfiguration`)

## Kolejność ładowania konfiguracji

1. `appsettings.json` z projektu uruchamianego
2. `Shared/Persistance/Common/Config/appsettings.json`
3. User Secrets
4. Zmienne środowiskowe

## Przykłady sekcji konfiguracyjnych

### Database
```json
{
  "Database": {
    "ConnectionString": "Server=localhost;Database=MyDb;Trusted_Connection=true;",
    "DatabaseName": "MyDb"
  }
}
```

### Email
```json
{
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "user@example.com",
    "Password": "password"
  }
}
```

### Redis
```json
{
  "Redis": {
    "ConnectionString": "localhost:6379",
    "Database": 0,
    "InstanceName": "MyApp"
  }
}
```

## Wymagania

- .NET 8.0
- Microsoft.Extensions.Options
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.DependencyInjection 