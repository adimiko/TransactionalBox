#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["samples/Directory.Packages.props", "samples/"]
COPY ["samples/Bank/TransactionalBox.BankAccounts/TransactionalBox.BankAccounts.csproj", "samples/Bank/TransactionalBox.BankAccounts/"]
RUN dotnet restore "./samples/Bank/TransactionalBox.BankAccounts/./TransactionalBox.BankAccounts.csproj"
COPY . .
WORKDIR "/src/samples/Bank/TransactionalBox.BankAccounts"
RUN dotnet build "./TransactionalBox.BankAccounts.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TransactionalBox.BankAccounts.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionalBox.BankAccounts.dll"]