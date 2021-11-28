FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80



# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Northwind.WebApp/Northwind.WebApp.csproj", "Northwind.WebApp/"]
RUN dotnet restore "Northwind.WebApp/Northwind.WebApp.csproj"
COPY . .
WORKDIR "/src/Northwind.WebApp"
RUN dotnet build "Northwind.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.WebApp.dll"]