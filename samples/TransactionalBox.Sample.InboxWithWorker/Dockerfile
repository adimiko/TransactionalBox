#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["samples/TransactionalBox.Sample.InboxWithWorker/TransactionalBox.Sample.InboxWithWorker.csproj", "samples/TransactionalBox.Sample.InboxWithWorker/"]
RUN dotnet restore "./samples/TransactionalBox.Sample.InboxWithWorker/./TransactionalBox.Sample.InboxWithWorker.csproj"
COPY . .
WORKDIR "/src/samples/TransactionalBox.Sample.InboxWithWorker"
RUN dotnet build "./TransactionalBox.Sample.InboxWithWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TransactionalBox.Sample.InboxWithWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionalBox.Sample.InboxWithWorker.dll"]