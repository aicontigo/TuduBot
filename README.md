# TuduBot - Telegram Todoist Assistant

TuduBot is a Telegram bot built with .NET 9 that integrates with Todoist. It allows users to manage tasks directly via Telegram using their Todoist API key.

## Features

- Store and manage Todoist API keys
- Retrieve and select default Todoist project
- Add tasks via commands or forwarded messages
- Inline keyboard-based interface
- Secure storage of user keys (with encryption support)

## Architecture Overview

- Telegram.Bot SDK-based .NET 9 Worker
- PostgreSQL database using EF Core
- Todoist API integration via todoist.net library

## Getting Started

### Prerequisites

- .NET 9 SDK
- PostgreSQL instance
- Docker (for deployment)
- Telegram Bot Token
- Todoist API Key (per user)

### Configuration

All settings are stored in:

- `appsettings.json`
- `appsettings.Development.json`
- `appsettings.Production.json`

Example `appsettings.json`:

```json
{
  "Bot": {
    "Token": "your_telegram_bot_token"
  },
  "Database": {
    "ConnectionString": "Host=localhost;Database=tudubot;Username=postgres;Password=your_password"
  },
  "Encryption": {
    "UseDataProtection": true
  }
}
```

### Docker Usage

```bash
docker build -t tudubot .
docker run -e DOTNET_ENVIRONMENT=Production tudubot
```

## Telegram Commands

| Command        | Description                             |
|----------------|-----------------------------------------|
| `/start`       | Welcome and show main menu              |
| `/menu`        | Show inline keyboard menu               |
| `/setkey`      | Add or update Todoist API key           |
| `/deletekey`   | Delete stored API key                   |
| `/add`         | Add a task manually                     |
| `/setproject`  | Set default project from Todoist        |

## MVP Functionality

- Save and validate Todoist API key
- Fetch and select projects
- Add task via text or forwarded message
- Interactive inline menu

## Technologies

| Component       | Technology         |
|-----------------|--------------------|
| Language        | C# (.NET 9)        |
| Telegram SDK    | Telegram.Bot       |
| Database        | PostgreSQL         |
| ORM             | EF Core            |
| Todoist Client  | todoist.net        |
| Logging         | Serilog            |

## Future Enhancements

- Localization support
- OAuth for Todoist instead of static API keys
- PIN-based authorization
- Metrics and logging dashboards
- Add comments to the tasks
- Add files with comments

## Contribution

Contributions are welcome. Please follow KISS and SRP principles, use `snake_case` for all database tables and fields, and maintain clean architecture.

## License

MIT