# GemNote

GemNote is a platform designed for learners to create and review flashcards on various subjects, enhancing their learning experience through efficient study methods.

## Features

- Create flashcards for different subjects.
- Review flashcards using spaced repetition.
- User authentication and profile management.
- Search and filter flashcards by subject or tags.
- Responsive design for use on different devices.

## Tech Stack

- **Frontend**: Blazor WebAssembly (WASM)
- **Backend**: ASP.NET Web API
- **Database**: SQL Server

## Getting Started

### Prerequisites

- .NET SDK 8.0
- SQL Server
- Visual Studio or VS Code

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/hoangvu1420/GemNote.git
   cd GemNote
   ```

2. Set up the SQL Server database:
   - Ensure you have specified the connection string to your local SQL Server in the `appsettings.json` file under the `GemNote.API` project.
   - Run the database migrations created by EF Core:
    ```bash
    cd GemNote.API
    dotnet ef database update
    ```

3. Build and run the backend:
   ```bash
   cd GemNote.API
   dotnet build
   dotnet run
   ```

4. Build and run the frontend:
   ```bash
   cd GemNote.Web
   dotnet build
   dotnet run
   ```

## Acknowledgments

- Thanks to the developers and community for their valuable contributions.

- For more information or assistance, feel free to contact me at hoangnguyenvu1420@gmail.com.