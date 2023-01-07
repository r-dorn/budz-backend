FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["budz-backend/budz-backend.csproj", "budz-backend/"]
RUN dotnet restore "budz-backend/budz-backend.csproj"
COPY . .
WORKDIR "/src/budz-backend"
RUN dotnet build "budz-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "budz-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "budz-backend.dll"]
