FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

COPY / /

RUN dotnet build

COPY  /publish .
ENTRYPOINT ["dotnet", "Practice5.CardService.dll"]
