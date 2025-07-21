# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Control de Lotes/Control de Lotes.csproj", "Control de Lotes/"]
RUN dotnet restore "Control de Lotes/Control de Lotes.csproj"
COPY . .
WORKDIR "/src/Control de Lotes"
RUN dotnet build "Control de Lotes.csproj" -c Release -o /app/build

# Etapa publish
FROM build AS publish
RUN dotnet publish "Control de Lotes.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Control de Lotes.dll"]
