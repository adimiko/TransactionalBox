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
COPY ["source/Directory.Packages.props", "source/"]
COPY ["source/Directory.Build.props", "source/"]
COPY ["samples/Bank/TransactionalBox.CustomerRegistrations/TransactionalBox.CustomerRegistrations.csproj", "samples/Bank/TransactionalBox.CustomerRegistrations/"]
COPY ["source/TransactionalBox.Outbox.EntityFrameworkCore/TransactionalBox.Outbox.EntityFrameworkCore.csproj", "source/TransactionalBox.Outbox.EntityFrameworkCore/"]
COPY ["source/Internals/TransactionalBox.Internals.DistributedLock.EntityFrameworkCore/TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.csproj", "source/Internals/TransactionalBox.Internals.DistributedLock.EntityFrameworkCore/"]
COPY ["source/Internals/TransactionalBox.Internals.DistributedLock/TransactionalBox.Internals.DistributedLock.csproj", "source/Internals/TransactionalBox.Internals.DistributedLock/"]
COPY ["source/Internals/TransactionalBox.Internals.KeyedInMemoryLock/TransactionalBox.Internals.KeyedInMemoryLock.csproj", "source/Internals/TransactionalBox.Internals.KeyedInMemoryLock/"]
COPY ["source/TransactionalBox.Outbox/TransactionalBox.Outbox.csproj", "source/TransactionalBox.Outbox/"]
COPY ["source/Internals/TransactionalBox.Internals.EventHooks/TransactionalBox.Internals.EventHooks.csproj", "source/Internals/TransactionalBox.Internals.EventHooks/"]
COPY ["source/Internals/TransactionalBox.Internals.SequentialGuid/TransactionalBox.Internals.SequentialGuid.csproj", "source/Internals/TransactionalBox.Internals.SequentialGuid/"]
COPY ["source/Internals/TransactionalBox.Internals.Transport.InMemory/TransactionalBox.Internals.Transport.InMemory.csproj", "source/Internals/TransactionalBox.Internals.Transport.InMemory/"]
COPY ["source/TransactionalBox/TransactionalBox.csproj", "source/TransactionalBox/"]
COPY ["source/TransactionalBox.Outbox.Kafka/TransactionalBox.Outbox.Kafka.csproj", "source/TransactionalBox.Outbox.Kafka/"]
RUN dotnet restore "./samples/Bank/TransactionalBox.CustomerRegistrations/./TransactionalBox.CustomerRegistrations.csproj"
COPY . .
WORKDIR "/src/samples/Bank/TransactionalBox.CustomerRegistrations"
RUN dotnet build "./TransactionalBox.CustomerRegistrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TransactionalBox.CustomerRegistrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionalBox.CustomerRegistrations.dll"]