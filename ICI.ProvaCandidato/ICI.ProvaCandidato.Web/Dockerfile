# Estágio base com runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
# Definir usuário não-root
RUN adduser --disabled-password --gecos '' appuser
USER appuser

# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
# Copiar apenas os arquivos .csproj para otimizar cache
COPY ["ICI.ProvaCandidato.Web/ICiProvaCandidato.Web.csproj", "ICI.ProvaCandidato.Web/"]
COPY ["ICI.ProvaCandidato.Dados/ICiProvaCandidato.Infra.csproj", "ICI.ProvaCandidato.Dados/"]
COPY ["ICI.ProvaCandidato.Negocio/ICiProvaCandidato.Aplicacao.csproj", "ICI.ProvaCandidato.Negocio/"]
COPY ["ICiProvaCandidato.Dominio/ICiProvaCandidato.Dominio.csproj", "ICiProvaCandidato.Dominio/"]
RUN dotnet restore "ICI.ProvaCandidato.Web/ICiProvaCandidato.Web.csproj"
# Copiar o restante do código
COPY . .
WORKDIR "/src/ICI.ProvaCandidato.Web"
RUN dotnet build "ICiProvaCandidato.Web.csproj" -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish "ICiProvaCandidato.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Healthcheck para verificar a aplicação
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s \
  CMD curl --fail http://localhost:80/health || exit 1
ENTRYPOINT ["dotnet", "ICiProvaCandidato.Web.dll"]