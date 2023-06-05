# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# WORKDIR /src
# COPY ["Server/CPM.Server.csproj", "Server/"]
# RUN dotnet restore "Server/CPM.Server.csproj"
# COPY . .
# WORKDIR "/src/Server"
# RUN dotnet build "CPM.Server.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "CPM.Server.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Server/CPM.Server.csproj", "Server/"]
RUN dotnet restore "Server/CPM.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "CPM.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CPM.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 9000
ENV ASPNETCORE_URLS=http://*:9000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CPM.Server.dll"]
