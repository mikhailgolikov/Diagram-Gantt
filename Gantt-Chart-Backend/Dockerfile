FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./*.sln ./
COPY ./Gantt-Chart-Backend/*.csproj ./Gantt-Chart-Backend/
COPY ./Gantt-Chart-Backend/. ./Gantt-Chart-Backend/

RUN dotnet restore

WORKDIR /app/Gantt-Chart-Backend

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN groupadd -r appgroup && useradd -r -g appgroup appuser
USER appuser

COPY --from=build app/publish .
ENTRYPOINT ["dotnet", "Gantt-Chart-Backend.dll"]