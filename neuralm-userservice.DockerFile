FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["src/Neuralm.Services/Neuralm.Services.UserService/Neuralm.Services.UserService.Rest/Neuralm.Services.UserService.Rest.csproj", "src/Neuralm.Services/Neuralm.Services.UserService/Neuralm.Services.UserService.Rest/"]
RUN dotnet restore "src/Neuralm.Services/Neuralm.Services.UserService/Neuralm.Services.UserService.Rest/Neuralm.Services.UserService.Rest.csproj"
COPY . .
WORKDIR "/src/src/Neuralm.Services/Neuralm.Services.UserService/Neuralm.Services.UserService.Rest"
RUN dotnet build "Neuralm.Services.UserService.Rest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neuralm.Services.UserService.Rest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neuralm.Services.UserService.Rest.dll"]
