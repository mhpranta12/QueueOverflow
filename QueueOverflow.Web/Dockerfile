FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["QueueOverflow.Web/*.csproj", "QueueOverflow.Web/"]
COPY ["QueueOverflow.Application/*.csproj", "QueueOverflow.Application/"]
COPY ["QueueOverflow.Domain/*.csproj", "QueueOverflow.Domain/"]
COPY ["QueueOverflow.Infrastructure/*.csproj", "QueueOverflow.Infrastructure/"]
COPY ["QueueOverflow.Service/*.csproj", "QueueOverflow.Service/"]
RUN dotnet restore "QueueOverflow.Web/QueueOverflow.Web.csproj"
COPY . .
WORKDIR "/src/QueueOverflow.Web"
RUN dotnet build "QueueOverflow.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "QueueOverflow.Web.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "QueueOverflow.Web.dll"]