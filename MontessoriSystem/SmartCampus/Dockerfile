FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Instala wkhtmltopdf
RUN apt-get update && apt-get install -y \
    wget fontconfig libjpeg62-turbo libxrender1 libxext6 xfonts-75dpi xfonts-base gdebi-core \
    && wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6.1-2/wkhtmltox_0.12.6.1-2.bullseye_amd64.deb \
    && gdebi --non-interactive wkhtmltox_0.12.6.1-2.bullseye_amd64.deb \
    && rm wkhtmltox_0.12.6.1-2.bullseye_amd64.deb

# ================================
# BUILD
# ================================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# En lugar de copiar uno por uno, copiamos todo el contexto de una vez
# para que Docker encuentre las dependencias entre proyectos.
COPY . .

# Restaurar (Asegúrate de que la ruta al .sln o al .csproj principal sea correcta desde la raíz)
# Si el Dockerfile está en la raíz, esto funcionará:
RUN dotnet restore "SmartCampus/SmartCampus.csproj"

# Publicar
RUN dotnet publish "SmartCampus/SmartCampus.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ================================
# FINAL
# ================================
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartCampus.dll"]