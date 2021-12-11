FROM mcr.microsoft.com/dotnet/sdk:5.0 AS prepare
WORKDIR /

#restore
COPY ../AbdtLectures.sln ./

COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore

# copy everything else and build app
COPY . ./
RUN dotnet build --no-restore

FROM prepare AS test
ENTRYPOINT ["dotnet", "test", "-l", "console;verbosity=normal", "--no-restore"]

FROM prepare AS build

RUN dotnet build --no-restore -c Release

WORKDIR /Practice5.ApiGateway
RUN dotnet publish --no-restore -c Release -o out

WORKDIR /Practice5.CardService
RUN dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS gtw
WORKDIR /app
COPY --from=build /Practice5.ApiGateway/out ./
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "Practice5.ApiGateway.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS card
WORKDIR /app
COPY --from=build /Practice5.CardService/out ./
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "Practice5.CardService.dll"]