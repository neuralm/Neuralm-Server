# FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app

EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["src/Neuralm.Services/Neuralm.Services.MessageQueue/Neuralm.Services.MessageQueue.NeuralmMQ/Neuralm.Services.MessageQueue.NeuralmMQ.csproj", "src/Neuralm.Services/Neuralm.Services.MessageQueue/Neuralm.Services.MessageQueue.NeuralmMQ/"]
RUN dotnet restore "src/Neuralm.Services/Neuralm.Services.MessageQueue/Neuralm.Services.MessageQueue.NeuralmMQ/Neuralm.Services.MessageQueue.NeuralmMQ.csproj"
COPY . .
WORKDIR "/src/src/Neuralm.Services/Neuralm.Services.MessageQueue/Neuralm.Services.MessageQueue.NeuralmMQ"
RUN dotnet build "Neuralm.Services.MessageQueue.NeuralmMQ.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Neuralm.Services.MessageQueue.NeuralmMQ.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neuralm.Services.MessageQueue.NeuralmMQ.dll"]
