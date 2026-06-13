# Shared.Persistance.DbUp - Migracja bazy danych TwitterDb

Projekt do zarządzania migracjami bazy danych MS SQL Server z użyciem DB Up.

## Opis

Projekt tworzy bazę danych `TwitterDb` z następującymi tabelami:
- **Users** - użytkownicy systemu
- **Posts** - posty użytkowników
- **Comments** - komentarze do postów

## Struktura bazy danych

### Tabela Users
- Id (INT, PRIMARY KEY)
- Username (NVARCHAR(50), UNIQUE)
- Email (NVARCHAR(100), UNIQUE)
- FirstName (NVARCHAR(50))
- LastName (NVARCHAR(50))
- Bio (NVARCHAR(500), NULLABLE)
- ProfilePictureUrl (NVARCHAR(500), NULLABLE)
- CreatedAt (DATETIME2)
- UpdatedAt (DATETIME2)

### Tabela Posts
- Id (INT, PRIMARY KEY)
- UserId (INT, FOREIGN KEY -> Users.Id)
- Content (NVARCHAR(1000))
- Title (NVARCHAR(200), NULLABLE)
- ImageUrl (NVARCHAR(500), NULLABLE)
- LikesCount (INT)
- CreatedAt (DATETIME2)
- UpdatedAt (DATETIME2)

### Tabela Comments
- Id (INT, PRIMARY KEY)
- PostId (INT, FOREIGN KEY -> Posts.Id)
- UserId (INT, FOREIGN KEY -> Users.Id)
- Content (NVARCHAR(500))
- LikesCount (INT)
- CreatedAt (DATETIME2)
- UpdatedAt (DATETIME2)

## Konfiguracja

### Skąd pobierana jest konfiguracja?

- **Domyślna konfiguracja** znajduje się w pliku:
  - `Shared/Persistance/Common/appsettings.json`
- **Możesz nadpisać ustawienia** w swoim projekcie (np. API) dodając własny `appsettings.json` z sekcją `Database`.
- **Możesz użyć User Secrets** lub zmiennych środowiskowych, jeśli nie chcesz trzymać connection stringa w repozytorium.

### Przykład sekcji Database

```json
{
  "Database": {
    "ConnectionString": "Server=localhost;Database=TwitterDb;Trusted_Connection=true;TrustServerCertificate=true;",
    "DatabaseName": "TwitterDb"
  }
}
```

### Kolejność ładowania konfiguracji
1. `appsettings.json` z projektu uruchamianego (jeśli istnieje)
2. `Shared/Persistance/Common/appsettings.json`
3. User Secrets (jeśli skonfigurowane)
4. Zmienne środowiskowe

## Użycie Option Pattern

Konfiguracja bazy danych jest bindowana do klasy `DatabaseOptions` z biblioteki `Shared.Persistance.Common`.

**Rejestracja w DI:**
```csharp
services.AddDatabaseConfiguration(configuration);
```

**Dostęp w kodzie:**
```csharp
public class SomeService
{
    private readonly DatabaseOptions _dbOptions;
    public SomeService(IDatabaseOptionsProvider provider)
    {
        _dbOptions = provider.Value;
    }
}
```

## Uruchomienie migracji

1. **Przywróć i zbuduj solucję:**
   ```bash
   dotnet restore
   dotnet build
   ```
2. **Uruchom migrator:**
   ```bash
   dotnet run --project Shared/Persistance/DbUp/PersistanceDbUp.csproj
   ```

## Skrypty migracji

Skrypty SQL znajdują się w folderze `Scripts/`:
- `001_CreateUsersTable.sql` - Tworzenie tabeli Users
- `002_CreatePostsTable.sql` - Tworzenie tabeli Posts
- `003_CreateCommentsTable.sql` - Tworzenie tabeli Comments
- `004_SeedTestData.sql` - Podstawowe dane testowe

## Generowanie danych testowych

Projekt używa biblioteki **Bogus** do generowania realistycznych danych testowych:
- Generuje losowych użytkowników z polskimi imionami i nazwiskami
- Tworzy posty z różnorodną treścią
- Dodaje komentarze do postów
- Używa losowych obrazów z Picsum Photos

## Wymagania

- .NET 8.0
- MS SQL Server (lokalny lub zdalny)
- Odpowiednie uprawnienia do tworzenia bazy danych

## Rozwiązywanie problemów

### Błąd połączenia
- Sprawdź czy serwer MS SQL jest uruchomiony
- Zweryfikuj connection string
- Upewnij się, że masz odpowiednie uprawnienia

### Błąd certyfikatu SSL
- Dodaj `TrustServerCertificate=true;` do connection stringa
- Lub skonfiguruj certyfikat SSL na serwerze

### Błąd uprawnień
- Upewnij się, że użytkownik ma uprawnienia do tworzenia bazy danych
- Sprawdź czy może tworzyć tabele i indeksy 