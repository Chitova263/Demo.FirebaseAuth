
ENTRYPOINT ["dotnet", "Demo.Api.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY "Demo.FirebaseAuth.sln" "Demo.FirebaseAuth.sln"
COPY "Demo.IdentityService/Demo.IdentityService.csproj" "Demo.IdentityService/Demo.IdentityService.csproj"
COPY "Demo.Shared/Demo.Shared.csproj" "Demo.Shared/Demo.Shared.csproj"
COPY "Demo.Api/Demo.Api.csproj" "Demo.Api/Demo.Api.csproj"

RUN dotnet restore "Demo.FirebaseAuth.sln"

COPY . .
WORKDIR /Demo.Api
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Demo.Api.dll"]
