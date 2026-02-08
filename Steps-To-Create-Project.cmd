:: Create the main solution folder and enter it
mkdir Task-Project-Management-System
cd Task-Project-Management-System

:: Create a new solution file
dotnet new sln -n Task-Project-Management-System

:: Create the Blazor WebAssembly project with interactivity
dotnet new blazor --interactivity WebAssembly --all-interactive -n Task-Project-Management-System

:: Add the server/host project to the solution
dotnet sln add Task-Project-Management-System\Task-Project-Management-System.csproj

:: Add the client project to the solution
dotnet sln add Task-Project-Management-System.Client\Task-Project-Management-System.Client.csproj

:: Create a Shared class library for DTOs / models / utils
dotnet new classlib -n Shared

:: Add the Shared project to the solution
dotnet sln add Shared\Shared.csproj

:: Add a reference from the Server project to the Shared project
dotnet add Task-Project-Management-System\Task-Project-Management-System.csproj reference Shared\Shared.csproj

:: Add a reference from the Client project to the Shared project
dotnet add Task-Project-Management-System.Client\Task-Project-Management-System.Client.csproj reference Shared\Shared.csproj
