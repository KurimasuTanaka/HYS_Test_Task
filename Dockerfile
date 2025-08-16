FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["HYStest/HYStest.csproj", "HYStest/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Database/Database.csproj", "Database/"]
RUN dotnet restore "HYStest/HYStest.csproj"

COPY . .
WORKDIR "/src/HYStest"
RUN dotnet build "HYStest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HYStest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HYStest.dll"]
