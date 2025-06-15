# Establecer la imagen base para el SDK de .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto y restaurar las dependencias
COPY ["StockAvaibleTest-API/StockAvaibleTest-API.csproj", "StockAvaibleTest-API/"]
RUN dotnet restore "StockAvaibleTest-API/StockAvaibleTest-API.csproj"

# Copiar el resto del código fuente
COPY . .

# Compilar la aplicación
WORKDIR "/src/StockAvaibleTest-API"
RUN dotnet build "StockAvaibleTest-API.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "StockAvaibleTest-API.csproj" -c Release -o /app/publish

# Crear la imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Variables de entorno para la base de datos
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Server=your_server;Database=your_database;User Id=your_user;Password=your_password;TrustServerCertificate=true"

# Exponer el puerto 8080
EXPOSE 8080

# Punto de entrada para la aplicación
ENTRYPOINT ["dotnet", "StockAvaibleTest-API.dll"]
