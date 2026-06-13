# Entity Loading Strategies

Projekt demonstrujący różne strategie ładowania encji w Entity Framework Core.

## Struktura projektu

```
entity-loading-strategies/
├── Shared/
│   └── Persistance/
│       ├── Common/                    # Wspólna biblioteka konfiguracji
│       │   ├── Config/
│       │   │   └── appsettings.json   # Konfiguracja bazy danych
│       │   ├── Extensions/
│       │   │   └── DatabaseConfigurationExtensions.cs
│       │   ├── Options/
│       │   │   └── DatabaseOptions.cs
│       │   └── Providers/
│       │       ├── DatabaseOptionsProvider.cs
│       │       ├── IDatabaseOptionsProvider.cs
│       │       ├── ITestDataProvider.cs
│       │       └── TestDataProvider.cs
│       └── DbUp/                      # Migracje bazy danych
│           ├── Scripts/
│           │   ├── 001_CreateUsersTable.sql
│           │   ├── 002_CreatePostsTable.sql
│           │   ├── 003_CreateCommentsTable.sql
│           │   └── 004_SeedTestData.sql
│           ├── DataGenerator.cs       # Generator danych testowych
│           └── Program.cs
└── LoadingStrategiesRepo.sln
```

## Konfiguracja

### DatabaseOptions

Klasa `DatabaseOptions` zawiera następujące właściwości:

```csharp
public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}
```

### appsettings.json

```json
{
  "Database": {
    "ConnectionString": "Data Source=DESKTOP-3P8MQAQ\\SQLSERVER22;User ID=darek;Password=123;Database=Twitter;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False",
    "DatabaseName": "Twitter"
  }
}
```

## Użycie

### 1. Migracje bazy danych

Uruchom migracje za pomocą projektu DbUp:

```bash
dotnet run --project Shared/Persistance/DbUp
```

Program automatycznie:
- Sprawdzi czy baza danych istnieje
- Wykona wszystkie nieukończone migracje
- Pokaże historię wykonanych migracji

### 2. Generowanie danych testowych

Dane testowe można wygenerować ręcznie używając klasy `DataGenerator`:

```csharp
var dataGenerator = new DataGenerator(connectionString);
await dataGenerator.GenerateTestDataAsync();
```

Generator tworzy:
- **50 użytkowników** z losowymi danymi
- **200 postów** przypisanych do losowych użytkowników
- **500 komentarzy** przypisanych do losowych postów i użytkowników

### 3. Konfiguracja w innych projektach

Aby użyć konfiguracji bazy danych w innym projekcie:

```csharp
// Rejestracja w DI
services.AddDatabaseConfiguration(configuration);

// Użycie
var dbOptionsProvider = provider.GetRequiredService<IDatabaseOptionsProvider>();
var dbOptions = dbOptionsProvider.Value;
```

## Funkcjonalności

### Shared.Persistance.Common

- **DatabaseOptions**: Konfiguracja bazy danych
- **DatabaseOptionsProvider**: Provider konfiguracji
- **TestDataProvider**: Generator danych testowych (dostępny przez DI)
- **DatabaseConfigurationExtensions**: Extension methods do rejestracji w DI

### Shared.Persistance.DbUp

- **Migracje**: Automatyczne wykonywanie skryptów SQL
- **Historia migracji**: Śledzenie wykonanych migracji w tabeli `SchemaVersions`
- **Generator danych**: Dynamiczne generowanie danych testowych
- **Konfiguracja**: Ładowanie konfiguracji z Common

## Schemat bazy danych

### Tabela Users
- Id (PK)
- Username
- Email
- FirstName
- LastName
- Bio
- ProfilePictureUrl

### Tabela Posts
- Id (PK)
- UserId (FK -> Users)
- Content
- Title
- ImageUrl
- LikesCount

### Tabela Comments
- Id (PK)
- PostId (FK -> Posts)
- UserId (FK -> Users)
- Content
- LikesCount

## Tabele systemowe

### SchemaVersions
DB Up automatycznie tworzy tabelę `SchemaVersions` do śledzenia wykonanych migracji:
- Id (PK)
- ScriptName
- Applied
- Checksum

## Rozszerzanie konfiguracji

Aby dodać nowe opcje konfiguracji:

1. **Dodaj właściwość do DatabaseOptions**:
```csharp
public class DatabaseOptions
{
    // ... istniejące właściwości
    public string NewOption { get; set; } = string.Empty;
}
```

2. **Dodaj do appsettings.json**:
```json
{
  "Database": {
    // ... istniejące opcje
    "NewOption": "wartość"
  }
}
```

3. **Stwórz provider (opcjonalnie)**:
```csharp
public interface INewOptionsProvider
{
    string Value { get; }
}

public class NewOptionsProvider : INewOptionsProvider
{
    public string Value { get; }
    
    public NewOptionsProvider(IOptions<DatabaseOptions> options)
    {
        Value = options.Value.NewOption;
    }
}
```

4. **Zarejestruj w extension method**:
```csharp
public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
{
    // ... istniejąca rejestracja
    services.AddSingleton<INewOptionsProvider, NewOptionsProvider>();
    return services;
}
```
