FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SnapTalk.WebAPI/SnapTalk.WebAPI.csproj", "SnapTalk.WebAPI/"]
RUN dotnet restore "SnapTalk.WebAPI/SnapTalk.WebAPI.csproj"
COPY . .
WORKDIR "/src/SnapTalk.WebAPI"
RUN dotnet build "SnapTalk.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SnapTalk.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SnapTalk.WebAPI.dll"]
