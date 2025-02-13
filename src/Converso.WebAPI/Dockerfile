FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Converso.WebAPI/Converso.WebAPI.csproj", "Converso.WebAPI/"]
COPY ["Converso.DAL/Converso.DAL.csproj", "Converso.DAL/"]
COPY ["Converso.Common/Converso.Common.csproj", "Converso.Common/"]
COPY ["Converso.BLL/Converso.BLL.csproj", "Converso.BLL/"]
RUN dotnet restore "Converso.WebAPI/Converso.WebAPI.csproj"
COPY . .
WORKDIR "/src/Converso.WebAPI"
RUN dotnet build "Converso.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Converso.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Converso.WebAPI.dll"]
