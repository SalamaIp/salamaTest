#first stage Base Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY *.csproj .
#restore dependencies
RUN dotnet retore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0   
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT [ "dotnet" , "Quiz.dll" ]
