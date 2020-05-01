FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
RUN useradd -M dotnetUser
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["homesteadAPI/homesteadAPI.csproj", "homesteadAPI/"]
RUN dotnet restore "homesteadAPI/homesteadAPI.csproj"
COPY . .
WORKDIR "/src/homesteadAPI"
RUN dotnet build "homesteadAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "homesteadAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER dotnetUser
CMD dotnet homesteadAPI.dll --urls=http://+:$PORT
