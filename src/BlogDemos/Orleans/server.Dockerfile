FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["ConsoleApp1/ConsoleApp1.csproj", "ConsoleApp1/"]
COPY ["Mahua.Interfaces/Mahua.Interfaces.csproj", "Mahua.Interfaces/"]
COPY ["Mahua.Implements/Mahua.Implements.csproj", "Mahua.Implements/"]
RUN dotnet restore "ConsoleApp1/ConsoleApp1.csproj"
COPY . .
WORKDIR "/src/ConsoleApp1"
RUN dotnet build "ConsoleApp1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ConsoleApp1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
EXPOSE 9000
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConsoleApp1.dll"]