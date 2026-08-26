# SmartQuiz

SmartQuiz is a full-stack quiz platform for creating, taking, and reviewing quizzes. It combines an Angular web application with an ASP.NET Core API and an optional Gemini-powered question generator.

## Features

- User authentication with JWT tokens
- Quiz experiences with protected routes
- Board, class, and quiz management API endpoints
- Configurable AI-generated questions
- Swagger/OpenAPI documentation in development
- Clean layered .NET solution structure

## Technology Stack

| Area | Technology |
| --- | --- |
| Web client | Angular 18, TypeScript, Angular Material, Bootstrap |
| API | ASP.NET Core 8 Web API |
| Domain | C# domain entities and enums |
| Authentication | JWT bearer authentication |
| AI integration | Google Gemini API (optional) |
| Documentation | Swagger / OpenAPI |

## Repository Structure

```text
SmartQuiz/
├── smartquiz-ui/             # Angular client
├── SmartQuiz.API/            # ASP.NET Core host and controllers
├── SmartQuiz.Application/    # Application services and contracts
├── SmartQuiz.Domain/         # Entities and domain enums
├── SmartQuiz.Infrastructure/ # External services and stores
├── SmartQuiz.Persistence/    # Persistence layer
├── SmartQuiz.Shared/         # Shared DTOs
└── SmartQuiz.sln             # .NET solution
```

## Prerequisites

- .NET SDK 8.0 or later
- Node.js 18.19+ or 20+
- npm

## Getting Started

### Start the API

From the repository root:

```powershell
dotnet restore SmartQuiz.sln
dotnet run --project .\SmartQuiz.API\SmartQuiz.API.csproj --launch-profile http
```

The API is available at `http://localhost:5214` and Swagger is available at `http://localhost:5214/swagger`.

### Start the Angular client

In a second terminal:

```powershell
Set-Location .\smartquiz-ui
npm install
npm start
```

The web client is available at `http://localhost:4200`.

## Configuration

Development configuration lives in `SmartQuiz.API/appsettings.Development.json`. Keep API keys outside source control. The Gemini integration can be enabled with an environment variable:

```powershell
$env:Gemini__ApiKey = "your-api-key"
dotnet run --project .\SmartQuiz.API\SmartQuiz.API.csproj --launch-profile http
```

Set `Gemini__Enabled` to `true` when using the AI question generator. Never commit real credentials to `appsettings*.json`.

## Useful Commands

```powershell
# Build the complete .NET solution
dotnet build SmartQuiz.sln

# Build the Angular application
Set-Location .\smartquiz-ui
npm run build

# Run Angular unit tests
npm test
```

## API Areas

- `AuthController`: registration and login
- `BoardsController`: quiz board operations
- `ClassesController`: class operations
- `QuizController`: quiz and question workflows

## Development Notes

- The API uses the `http` launch profile on port `5214`.
- The Angular development server uses port `4200` by default.
- Swagger is enabled in the Development environment.
- Build output, dependencies, IDE files, and local configuration are excluded through `.gitignore`.

## License

This project is currently maintained as a private application. Add a license before distributing it publicly.