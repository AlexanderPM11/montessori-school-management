FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Instala wkhtmltopdf correctamente
RUN apt-get update && apt-get install -y \
    wget \
    fontconfig \
    libjpeg62-turbo \
    libxrender1 \
    libxext6 \
    xfonts-75dpi \
    xfonts-base \
    gdebi-core \
    && wget https://github.com/wkhtmltopdf/packaging/releases/download/0.12.6.1-2/wkhtmltox_0.12.6.1-2.bullseye_amd64.deb \
    && gdebi --non-interactive wkhtmltox_0.12.6.1-2.bullseye_amd64.deb \
    && rm wkhtmltox_0.12.6.1-2.bullseye_amd64.deb


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SmartCampus/SmartCampus.csproj", "SmartCampus/"]
COPY ["montessoriSystem.core.Application/MontessoriSystem.Core.Application.csproj", "montessoriSystem.core.Application/"]
COPY ["MontessoriSystem.Core.Domain/MontessoriSystem.Core.Domain.csproj", "MontessoriSystem.Core.Domain/"]
COPY ["MontessoriSystem.Core.Identity/MontessoriSystem.Infrastructure.Identity.csproj", "MontessoriSystem.Core.Identity/"]
COPY ["MontessoriSystem.Infrastructure.Shared/MontessoriSystem.Infrastructure.Shared.csproj", "MontessoriSystem.Infrastructure.Shared/"]
COPY ["MontessoriSystem.Infrastructure.Persistence/MontessoriSystem.Infrastructure.Persistence.csproj", "MontessoriSystem.Infrastructure.Persistence/"]
COPY ["MontessoriSystem.Infrastructure.Security/MontessoriSystem.Infrastructure.Security.csproj", "MontessoriSystem.Infrastructure.Security/"]
RUN dotnet restore "./SmartCampus/SmartCampus.csproj"
COPY . .
WORKDIR "/src/SmartCampus"

# Copia explícita de las vistas al directorio de publicación
RUN mkdir -p /app/Views && cp -r Views/* /app/Views/

RUN dotnet build "./SmartCampus.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmartCampus.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copia las vistas desde el stage de build
COPY --from=build /app/Views ./Views
ENTRYPOINT ["dotnet", "SmartCampus.dll"]