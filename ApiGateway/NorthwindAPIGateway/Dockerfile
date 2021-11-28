FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateway/NorthwindAPIGateway/NorthwindAPIGateway.csproj", "ApiGateway/NorthwindAPIGateway/"]
RUN dotnet restore "ApiGateway/NorthwindAPIGateway/NorthwindAPIGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateway/NorthwindAPIGateway"
RUN dotnet build "NorthwindAPIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NorthwindAPIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NorthwindAPIGateway.dll"]
