# Todo API

Учебный веб-API на ASP.NET Core 9: менеджер задач с регистрацией, JWT-авторизацией и ролями.

## Стек

- ASP.NET Core 9 Web API (контроллеры)
- Entity Framework Core 9 + SQLite
- JWT Bearer, BCrypt
- Swagger (Swashbuckle)
- xUnit

## Запуск

1. Скопировать `todo/appsettings.json.example` в `todo/appsettings.json` и заменить `Jwt:Key` на свой ключ (минимум 32 символа).
2. Запустить:

```bash
cd todo
dotnet run
```

3. Открыть `https://localhost:<порт>/swagger`.

База `todo.db` создаётся автоматически при первом запуске. При первом запуске создаётся администратор с данными из секции `SeedAdmin` в `appsettings.json`.

## Тесты

```bash
dotnet test
```

## Эндпоинты

| Метод | URL | Доступ | Описание |
|---|---|---|---|
| POST | /api/auth/register | все | регистрация, возвращает токен |
| POST | /api/auth/login | все | вход, возвращает токен |
| GET | /api/tasks | авторизованный | свои задачи |
| GET | /api/tasks/all | Admin | задачи всех пользователей |
| GET | /api/tasks/{id} | владелец / Admin | одна задача |
| POST | /api/tasks | авторизованный | создать задачу |
| PUT | /api/tasks/{id} | владелец / Admin | изменить задачу |
| DELETE | /api/tasks/{id} | владелец / Admin | удалить задачу |

Токен передаётся в заголовке `Authorization: Bearer <токен>`.

## Коды ответов

| Код | Когда |
|---|---|
| 200 / 201 / 204 | успех |
| 400 | ошибка валидации |
| 401 | нет токена или неверный логин/пароль |
| 403 | недостаточно прав или чужая задача |
| 404 | задача не найдена |
| 409 | логин или email уже заняты |
| 500 | внутренняя ошибка сервера |

## Структура

- `Controllers`: приём HTTP-запросов (Auth, Tasks)
- `Models`: сущности БД (User, TaskItem)
- `DTOs`: объекты запросов и ответов, с валидацией
- `Data`: контекст EF Core
- `Services`: бизнес-логика за интерфейсами (внедрение зависимостей)
- `Middleware`: глобальная обработка ошибок
- `Exceptions`: собственные исключения
- `Migrations`: схема БД

## Архитектура

Controller → Service (через интерфейс) → AppDbContext → SQLite.
Контроллеры не знают про БД, сервисы не знают про HTTP.
