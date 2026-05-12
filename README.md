# Лабораторная работа № 3 — WebAppPassport

**Авторы:** Добра Виктор, Ишаков Денис  
**Группа:** СГН3-41Б  
**Репозиторий:** https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/tree/feature  
**Задеплоено на:** https://webapppassport.fly.dev  

---

## Схема данных

[Схема базы данных (Яндекс Диск)](https://disk.yandex.ru/i/bLt5yo8PFb7Beg)

---

## Структура репозитория

```
WebAppPassport/
├── WebAppPassport.sln
└── WebAppPassport/
    ├── Program.cs                          # Точка входа, DI, JWT, Swagger
    ├── WebAppPassport.csproj
    ├── appsettings.json
    ├── Dockerfile
    ├── fly.toml
    │
    ├── Common/
    │   ├── VisaType.cs                     # Enum типов виз
    │   └── VisaTypeLabels.cs               # Строковые метки для VisaType
    │
    ├── Controllers/
    │   ├── PassportController.cs
    │   ├── CountryController.cs
    │   ├── CompareController.cs
    │   ├── RankController.cs
    │   ├── StackController.cs
    │   ├── UserController.cs
    │   ├── ProfileController.cs
    │   └── Models/                         # DTO (ViewModels)
    │       ├── AuthViewModels.cs
    │       ├── PassportViewModels.cs
    │       ├── CountryViewModels.cs
    │       ├── RankViewModels.cs
    │       └── ProfileViewModels.cs
    │
    ├── Converters/
    │   ├── FromEfToServiceConverter.cs     # Entity → Model
    │   ├── FromServiceToEfConverter.cs     # Model → Entity
    │   ├── FromServiceToViewModelConverter.cs  # Model → DTO
    │   └── FromViewModelToServiceConverter.cs  # DTO → Model
    │
    ├── DataBase/
    │   ├── AppContext.cs                   # DbContext
    │   ├── Extensions.cs                   # AddDatabase() extension
    │   ├── Models/                         # EF-сущности
    │   │   ├── Country.cs
    │   │   ├── Passport.cs
    │   │   ├── PassportCountryVisa.cs
    │   │   └── User.cs
    │   ├── Configurations/                 # Fluent API конфигурации
    │   │   ├── CountryConfiguration.cs
    │   │   ├── PassportConfiguration.cs
    │   │   ├── PassportCountryVisaConfiguration.cs
    │   │   └── UserConfiguration.cs
    │   ├── Repositories/
    │   │   ├── IRepository.cs
    │   │   └── Repository.cs
    │   └── Migrations/
    │
    ├── Services/
    │   ├── Models/                         # Сервисные модели
    │   │   ├── Country.cs
    │   │   ├── Passport.cs
    │   │   ├── PassportCountryVisa.cs
    │   │   └── User.cs
    │   ├── ResponseModels/                 # Вспомогательные ответные модели
    │   ├── PassportService/
    │   │   ├── IPassportService.cs
    │   │   └── PassportService.cs
    │   ├── CountryService/
    │   │   ├── ICountryService.cs
    │   │   └── CountryService.cs
    │   ├── UserService/
    │   │   ├── IUserService.cs
    │   │   └── UserService.cs
    │   ├── RankService/
    │   │   ├── IRankService.cs
    │   │   └── RankService.cs
    │   ├── ExternalApiServices/
    │   │   ├── IExternalApiService.cs
    │   │   ├── ExternalApiService.cs
    │   │   └── Models/
    │   ├── StaticDataServices/
    │   │   ├── IStaticService.cs
    │   │   ├── StaticService.cs
    │   │   └── StaticData/
    │   │       ├── population.csv
    │   │       └── models.csv
    │   └── BackgroundServices/
    │       ├── ISyncService.cs
    │       └── BaseSyncService.cs
    └── Migrations/
```

---

## 1. Сущности БД, контексты и репозитории

### EF-сущности

| Сущность | Файл |
|---|---|
| `Country` | [DataBase/Models/Country.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Models/Country.cs) |
| `Passport` | [DataBase/Models/Passport.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Models/Passport.cs) |
| `PassportCountryVisa` | [DataBase/Models/PassportCountryVisa.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Models/PassportCountryVisa.cs) |
| `User` | [DataBase/Models/User.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Models/User.cs) |

### Конфигурации (Fluent API)

| Конфигурация | Файл |
|---|---|
| `CountryConfiguration` | [DataBase/Configurations/CountryConfiguration.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Configurations/CountryConfiguration.cs) |
| `PassportConfiguration` | [DataBase/Configurations/PassportConfiguration.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Configurations/PassportConfiguration.cs) |
| `PassportCountryVisaConfiguration` | [DataBase/Configurations/PassportCountryVisaConfiguration.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Configurations/PassportCountryVisaConfiguration.cs) |
| `UserConfiguration` | [DataBase/Configurations/UserConfiguration.cs](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Configurations/UserConfiguration.cs) |

### DbContext

[`DataBase/AppContext.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/AppContext.cs) — `AppContext : DbContext` регистрирует все `DbSet<>` и применяет конфигурации через `ApplyConfiguration(...)`.

[`DataBase/Extensions.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Extensions.cs) — extension-метод `AddDatabase()`, регистрирующий контекст с PostgreSQL-строкой подключения.

### Репозиторий

| Файл | Описание |
|---|---|
| [`DataBase/Repositories/IRepository.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Repositories/IRepository.cs) | Интерфейс репозитория |
| [`DataBase/Repositories/Repository.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/DataBase/Repositories/Repository.cs) | Реализация: CRUD для всех сущностей, операции связывания паспортов/стран с пользователем, массовое обновление, управление видимостью |

Миграции: [`Migrations/`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/tree/feature/WebAppPassport/Migrations) — применяются автоматически при старте через `db.Database.Migrate()`.

---

## 2. Бизнес-логика в сервисах. Конвертеры между слоями

Слоёная архитектура: **Entity (EF) ↔ Model (Service) ↔ DTO (ViewModel/Controller)**

### Сервисы

| Сервис | Интерфейс | Реализация |
|---|---|---|
| Паспорта | [`IPassportService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/PassportService/IPassportService.cs) | [`PassportService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/PassportService/PassportService.cs) |
| Страны | [`ICountryService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/CountryService/ICountryService.cs) | [`CountryService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/CountryService/CountryService.cs) |
| Пользователи | [`IUserService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/UserService/IUserService.cs) | [`UserService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/UserService/UserService.cs) |
| Рейтинг | [`IRankService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/RankService/IRankService.cs) | [`RankService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/RankService/RankService.cs) |

### Сервисные модели (промежуточный слой)

| Модель | Файл |
|---|---|
| `Country` | [`Services/Models/Country.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/Models/Country.cs) |
| `Passport` | [`Services/Models/Passport.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/Models/Passport.cs) |
| `PassportCountryVisa` | [`Services/Models/PassportCountryVisa.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/Models/PassportCountryVisa.cs) |
| `User` | [`Services/Models/User.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/Models/User.cs) |

### Конвертеры

| Конвертер | Направление | Файл |
|---|---|---|
| `FromEfToServiceConverter` | Entity → Model | [`Converters/FromEfToServiceConverter.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Converters/FromEfToServiceConverter.cs) |
| `FromServiceToEfConverter` | Model → Entity | [`Converters/FromServiceToEfConverter.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Converters/FromServiceToEfConverter.cs) |
| `FromServiceToViewModelConverter` | Model → DTO | [`Converters/FromServiceToViewModelConverter.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Converters/FromServiceToViewModelConverter.cs) |
| `FromViewModelToServiceConverter` | DTO → Model | [`Converters/FromViewModelToServiceConverter.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Converters/FromViewModelToServiceConverter.cs) |

Конвертеры реализованы как статические классы с extension-методами. Например, `passport.ToDetailViewModel()`, `efUser.ToServiceEntity()`.  
Для предотвращения циклических зависимостей при сериализации применяются «shallow»-версии вложенных объектов (например, `CountrySummaryOnly`, `PassportShallow`).

---

## 3. API-контроллеры и DTO

### Контроллеры

| Контроллер | Маршруты | Файл |
|---|---|---|
| `PassportController` | `GET /`, `GET /passport/{iso}`, `GET /passport?iso=` | [`Controllers/PassportController.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/PassportController.cs) |
| `CountryController` | `GET /countries`, `GET /country/{iso}` | [`Controllers/CountryController.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/CountryController.cs) |
| `CompareController` | `GET /compare?isos=` | [`Controllers/CompareController.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/CompareController.cs) |
| `RankController` | `GET /rank` | [`Controllers/RankController.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/RankController.cs) |
| `StackController` | `GET /stack` *(требует JWT)* | [`Controllers/StackController.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/StackController.cs) |
| `UserController` | `POST /user/register`, `POST /user/login`, `POST /user/addPassport`, `POST /user/addCountry`, `DELETE /user/removePassport`, `DELETE /user/removeCountry`, `PATCH /user/visibility/passports`, `PATCH /user/visibility/countries` | [`Controllers/UserController.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/UserController.cs) |
| `ProfileController` | `GET /u/{username}` | [`Controllers/ProfileController.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/ProfileController.cs) |

### DTO (ViewModels)

| DTO | Файл |
|---|---|
| `RegisterRequest`, `LoginRequest`, `TokenViewModel`, `VisibilityRequest` | [`Controllers/Models/AuthViewModels.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/Models/AuthViewModels.cs) |
| `PassportListItemViewModel`, `PassportDetailViewModel`, `PassportRankInfoViewModel` | [`Controllers/Models/PassportViewModels.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/Models/PassportViewModels.cs) |
| `CountrySummaryViewModel`, `CountryListItemViewModel`, `CountryDetailViewModel` | [`Controllers/Models/CountryViewModels.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/Models/CountryViewModels.cs) |
| `RankViewModel`, `RankPassportViewModel`, `RankCountryViewModel` | [`Controllers/Models/RankViewModels.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/Models/RankViewModels.cs) |
| `ProfileViewModel` | [`Controllers/Models/ProfileViewModels.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/Models/ProfileViewModels.cs) |

---

## 4. Аутентификация и авторизация (JWT)

Настройка в [`Program.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Program.cs):

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();
```

JWT-секрет загружается из переменной окружения `JWT_SECRET` (через `.env`-файл или Fly.io secrets).

Генерация токена реализована в [`UserService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/UserService/UserService.cs) — метод `GenerateJwt(User user)`:
- Клейм `ClaimTypes.Name` = username
- Клейм `ClaimTypes.NameIdentifier` = id пользователя
- Срок действия: 7 дней
- Подпись: HMAC-SHA256

Пароли хранятся в виде хеша BCrypt.

### Использование `[Authorize]` в контроллерах

| Контроллер | Атрибут | Описание |
|---|---|---|
| [`StackController`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/StackController.cs) | `[Authorize]` на уровне класса | Весь контроллер защищён — только авторизованные пользователи могут получить свой стек виз |
| [`UserController`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Controllers/UserController.cs) | `[Authorize]` на уровне методов | `addPassport`, `addCountry`, `removePassport`, `removeCountry`, `visibility/passports`, `visibility/countries` — требуют JWT |

Эндпоинты `register`, `login`, `GET /`, `GET /rank`, `GET /u/{username}`, `GET /compare` — публичные, не требуют авторизации.

---

## 5. Документация API (Swagger / Scalar)

Для документирования API используется встроенный в .NET генератор OpenAPI-документа и Scalar UI:

```csharp
builder.Services.AddOpenApi(options => { ... });  // генерация /openapi/v1.json
app.MapOpenApi();
app.MapScalarApiReference(options =>              // UI на /scalar/v1
{
    options.Title = "WebAppPassport API";
    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});
```

**Интерактивная документация:** https://webapppassport.fly.dev/scalar/v1  
**OpenAPI JSON:** https://webapppassport.fly.dev/openapi/v1.json

Scalar поддерживает Bearer-авторизацию «из коробки» — токен можно вставить прямо в UI для тестирования защищённых эндпоинтов.

---

## 6. Прочие компоненты

### Фоновый сервис синхронизации

[`Services/BackgroundServices/BaseSyncService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/BackgroundServices/BaseSyncService.cs) — реализует `IHostedService`:

- При старте проверяет, пуста ли БД. Если пуста — инициализирует список стран и паспортов из внешнего API.
- Каждые 24 часа обновляет визовые правила для всех паспортов.
- После каждой синхронизации пересчитывает метрики паспортов: `VisaFreeCount`, `VisaOnArrivalCount`, `EtaCount`, `RequiredVisaCount`, `MobilityScore`, `WorldRank`, `TotalPopulation`.

### Интеграция с внешним API

[`Services/ExternalApiServices/ExternalApiService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/ExternalApiServices/ExternalApiService.cs) — обращается к Henley Passport Index API (`api.henleypassportindex.com/api/v3/`):

- `GetAllCountriesAndPassportsAsync()` — получить список всех стран и паспортов.
- `GetAllDestinationsByPassportAsync(passport)` — получить все визовые правила для конкретного паспорта (VisaFree / VisaOnArrival / ETA / RequiredVisa).

Модели запросов: [`Services/ExternalApiServices/Models/`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/tree/feature/WebAppPassport/Services/ExternalApiServices/Models)

### Статические данные

[`Services/StaticDataServices/StaticService.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/StaticDataServices/StaticService.cs) — обогащает данные стран при инициализации:

- [`population.csv`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/StaticDataServices/StaticData/population.csv) — население и площадь страны.
- [`models.csv`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Services/StaticDataServices/StaticData/models.csv) — допустимость двойного гражданства (`DualCitizenshipAllowed`).

Данные читаются с помощью `CsvHelper` и хранятся в памяти.

### CORS и прокси

В [`Program.cs`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Program.cs) настроена политика CORS `AllowAnyOrigin` и `UseForwardedHeaders(XForwardedProto)` для корректной работы за Fly.io reverse proxy (HTTPS-схема в OpenAPI-документе).

### Деплой

[`Dockerfile`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/Dockerfile) + [`fly.toml`](https://git.iu7.bmstu.ru/dvs24s026/WebAppPassport/-/blob/feature/WebAppPassport/fly.toml) — приложение задеплоено на платформу [Fly.io](https://fly.io).  
БД — PostgreSQL, также поднятая на Fly.io. Строка подключения и `JWT_SECRET` передаются через Fly secrets.
