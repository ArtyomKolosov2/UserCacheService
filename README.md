# UserCacheService

UserCacheService is a simple web application. It is a set of CRUD operations, it has a caching mechanism that is updated by a background job every 10 minutes.
Some endpoints are secured with basic authentication. There is also a console application that can be used to test it. You can also run integration tests to make sure
that the basic functionality is working properly.

## Features
- Thread save in memory cache which is updated every 10 minutes
- Basic authentication implementation
- Usage of DDD approach for the application
- Web endpoints handle different types of content (xml, json) in the same controller scope
- Database communication is handled by Entity Framework

## Tech

- .NET 6
- C# 10
- ASP.NET Core
- Entity Framework with MySQL database provider
- MediatR
- Flurl
- Mapster
- xUnit & FluentAssertions - test execution & assertions

UserCacheService code is on a [public repository](https://github.com/ArtyomKolosov2/UserCacheService) on GitHub.

## How to start

There are two options how to start applications:

1. Using docker-compose file.
2. By building the application on your local machine.

### Option 1
**You must have a docker engine installed on your machine**

Web
- Navigate to a folder with the solution code
- Find and open folder `Docker`
- Beforehand you can adjust `.env` file to change settings for the controller
- Open console or terminal in the scope of the folder
- Write docker-compose command `docker compose -f docker-compose.yml -f docker-compose.override.yml --env-file .env up -d --build`
- Observe the application on `localhost:5000` (default value)

Console
- Navigate to a folder with the solution code
- Find and open folder `UserCacheService.ConsoleApplication`
- Open console or terminal in the scope of the folder
- Write docker commands
  - `docker image build -t consoleapplication .`
  - `docker container run -it --network docker_default consoleapplication`
- Start application and change default url to `http:\\usercacheservice:80`

### Option 2
**You must have a .NET 6 runtime installed on your machine as well as MySQL**

- Navigate to a folder with the solution code
- Build the solution `dotnet build UserCacheService.sln`
  
Web
- Go to folder with the results of the build of UserCacheService `{SolutionRoot}\UserCacheService\bin\Debug\net6.0`
- Update `appsettings.json` config for your system (set correct connection string or change basic authorization values)
- Use `dotnet UserCacheService.dll` or find and open `UserCacheService.exe`
- Observe the application on `localhost:5000` (default value)

Console
- Go to folder with the results of the build of UserCacheService.ConsoleApplication `{SolutionRoot}\UserCacheService.ConsoleApplication\bin\Debug\net6.0`
- Use `dotnet UserCacheService.ConsoleApplication.dll` or find and open `UserCacheService.ConsoleApplication.exe`
