FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5001

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
ENTRYPOINT ["dotnet", "homesteadAPI.dll"]
