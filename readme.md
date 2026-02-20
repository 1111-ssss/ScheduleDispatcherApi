# ScheduleApi

**REST API** для управления расписанием колледжа.  
Микросервис предназначен для:

- автоматизации работы диспетчера
- периодической синхронизации данных из legacy-системы

Проект построен как **микросервис** и работает в паре со вторым сервисом.  
**Связанный репозиторий** (клиент-сервис):  
[← Нажми →](https://github.com/StayNight-withMe/ShceduleClientApi)

## Архитектура и ключевые принципы

- **Clean Architecture** + **Vertical Slice Architecture** *(гибрид)*
- **CQRS** через библиотеку **MediatR**
- **Minimal APIs** (ASP.NET Core)
- **Result-ориентированный подход** для обработки бизнес-логики без исключений.
- **Спецификации** для выразительных и переиспользуемых запросов

## Технологический стек

| Компонент | Используемая библиотека / технология | Назначение|
|:-|:-|:-|
| ASP.NET Core | 8.0 / 9.0 | Минимальные API, роутинг, middleware |
| MediatR | MediatR | CQRS, команды и запросы |
| Валидация | FluentValidation | Валидация входных моделей |
| Результаты операций | Ardalis.Result (или FluentResults) | Единый тип результата вместо исключений |
| Спецификации | Ardalis.Specification | Переиспользуемые, композируемые запросы |
| Аутентификация | JWT Bearer | Токены для диспетчера и (опционально) клиентов |
| Хеширование паролей | Argon2id | Безопасное хранение паролей диспетчеров |
| База данных (основная) | PostgreSQL + EF Core | Хранение расписания, кабинетов, расчасовки |
| База данных (legacy, read-only) | MS SQL Server + Dapper | Импорт справочников и старых данных |
| Документация API | Scalar (OpenAPI) | Интерактивная документация и тестирование |

## Структура проекта (Vertical Slice + Clean Architecture)
```
src/
├─ API/                        # Точка входа · Minimal APIs · конфигурация
│  ├─ Endpoints/               # (может быть пусто — эндпоинты через extensions)
│  ├─ Extensions/
├─ Program.cs
├─ appsettings*.json
│  └─ Properties/launchSettings.json
│
├─ Application/                # Use-cases, handlers, валидация, behaviors
│  └─ Features/                 # ← вертикальные срезы (по доменным возможностям)
├─ Domain/                     # Сущности, value objects, ошибки, спецификации
│  ├─ Entities/                # Group, Teacher, Subject, Classroom, ScheduleItem, Removal …
│  └─ Specifications/
└─ Infrastructure/             # Конкретные реализации
   ├─ DataBase/
   ├─ Services/
   └─ UnitOfWork/
```

## Базы данных и синхронизация

Система работает с **двумя** базами данных:

1. **Legacy DB** — MS SQL Server (только чтение)  
   Содержит справочники: преподаватели, группы, специальности, предметы, связи Subject_Teacher.

2. **Core DB** — PostgreSQL (чтение + запись)  
   Хранит актуальное расписание, кабинеты, снятия, расчасовку, редактируемые связи предмет-преподаватель.

**Синхронизация** выполняется по команде диспетчера (эндпоинт `/api/sync/trigger`).  
Обычно запускается 1–2 раза в семестр или при значительных изменениях в legacy-системе.

## Эндпоинты API

| Категория | Метод | Путь | Принимает | Возвращает |
|----------|--------|-------------------|-----------------|--------------------|
| **Auth** | `POST` | `/api/auth/login` | `LoginUserQuery` | `AuthDTO` | 200, 400, 500 |
| **Auth** | `POST` | `/api/auth/refresh` | `RefreshTokenQuery` | `AuthDTO` | 200, 400, 500 |
| **Sync** | `POST` | `/api/sync` | — | — | 200, 500 |
| **Dispatcher** | `GET` | `/api/dispatcher/get` | `GetWorkloadQuery` | `WorkloadSummaryDTO` | 200, 400, 500 |
| **Dispatcher** | `POST` | `/api/dispatcher/save` | `SaveWorkloadCommand` | — | 200, 400, 500 |
| **Dispatcher** | `POST` | `/api/dispatcher/finalize` | `FinalizeDayScheduleCommand` | — | 200, 400, 500 |
| **Dispatcher** | `GET` | `/api/dispatcher/lessons` | — | `AllLessonsDTO` | 200, 500 |

> Подробное описание параметров, форматов ответа и примеров запросов → **Scalar UI** после запуска.

## Запуск проекта локально

### Требования

- .NET 8 / .NET 9 SDK
- PostgreSQL 15+
- MS SQL Server (legacy) — хотя бы для чтения

### Шаги

1. Склонируйте репозиторий  
   `git clone …`

2. Настройте `appsettings.Development.json` / user-secrets

```json
{
  "ConnectionStrings": {
    "CoreDb":             "Host=localhost;Database=schedule_core;Username=…",
    "LegacyDb":           "Server=…;Database=legacy;User Id=…;Password=…;"
  },
  "Jwt": {
    "Key":                "очень-длинный-случайный-ключ-минимум-64-символа",
    "Issuer":             "schedule-api",
    "Audience":           "schedule-clients"
  },
  "Sync": {
    "BatchSize":          500
  }
}
```

3. Примените миграции для Core DB
dotnet ef database update --project src/Infrastructure --startup-project src/API
4. Запустите
dotnet run --project src/API

Swagger → http://localhost:5000/swagger (или https://localhost:5001/swagger)

## Полезные команды
```bash
# Сборка
dotnet build

# Тесты
dotnet test

# Новая миграция
dotnet ef migrations add Name --project src/Infrastructure --startup-project src/API

# Обновление БД
dotnet ef database update   --project src/Infrastructure --startup-project src/API
```

## UI диспетчера
Макет интерфейса (Figma):
https://www.figma.com/design/9gudUAUSsOtCZIeUJs1QfC/Настолка_Распес