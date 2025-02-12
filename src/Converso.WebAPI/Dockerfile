FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SnapTalk.WebAPI/SnapTalk.WebAPI.csproj", "SnapTalk.WebAPI/"]
COPY ["SnapTalk.DAL/SnapTalk.DAL.csproj", "SnapTalk.DAL/"]
COPY ["SnapTalk.Common/SnapTalk.Common.csproj", "SnapTalk.Common/"]
COPY ["SnapTalk.BLL/SnapTalk.BLL.csproj", "SnapTalk.BLL/"]
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
