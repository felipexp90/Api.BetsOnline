FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-stretch AS build
WORKDIR /src
COPY ["Api.BetsOnline/Api.BetsOnline.csproj", "Api.BetsOnline/"]
RUN dotnet restore "Api.BetsOnline/Api.BetsOnline.csproj"
COPY . .
WORKDIR "/src/Api.BetsOnline"
RUN dotnet build "Api.BetsOnline.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Api.BetsOnline.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api.BetsOnline.dll"]
