# Coding Exercise #

This README would normally document whatever steps are necessary to get your application up and running.

## Running app ##

To start the application go to solution directory and run command:
```bash
dotnet run --project ./CodingExercise/CodingExercise.csproj
```

After that app will listening on:
-  http://localhost:5000
-  https://localhost:5001

The API for exercise items supports basic CRUD operations and is available at path: **/api/item**

## Swagger ##

To can use a prepared swagger: **https://localhost:5001/swagger/index.html**

## Testing ##
To start tests go to solution directory and run command:
```bash
dotnet test ./CodingExercise.Tests/CodingExercise.Tests.csproj
```