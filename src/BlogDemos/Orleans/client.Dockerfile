FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["ConsoleApp2/ConsoleApp2.csproj", "ConsoleApp2/"]
COPY ["Mahua.Interfaces/Mahua.Interfaces.csproj", "Mahua.Interfaces/"]
RUN dotnet restore "ConsoleApp2/ConsoleApp2.csproj"
COPY . .
WORKDIR "/src/ConsoleApp2"
RUN dotnet build "ConsoleApp2.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ConsoleApp2.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
EXPOSE 9000
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConsoleApp2.dll"]