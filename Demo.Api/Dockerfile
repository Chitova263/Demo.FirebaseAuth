FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY "Demo.FirebaseAuth.sln" "Demo.FirebaseAuth.sln"
COPY "Demo.IdentityService/Demo.IdentityService.csproj" "Demo.IdentityService/Demo.IdentityService.csproj"
COPY "Demo.Api/Demo.Api.csproj" "Demo.Api/Demo.Api.csproj"
COPY "Demo.Common/Demo.Common.csproj" "Demo.Common/Demo.Common.csproj"
#COPY "Demo.EmailService/Demo.EmailService.csproj" "Demo.EmailService/Demo.EmailService.csproj"

RUN dotnet restore "Demo.FirebaseAuth.sln"

COPY . .
WORKDIR /src/Demo.Api
RUN dotnet publish --configuration Release -o /Demo.Spa

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /Demo.Spa .
ENTRYPOINT ["dotnet", "Demo.Api.dll"]
