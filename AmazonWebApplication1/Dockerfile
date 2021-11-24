#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AmazonWebApplication1/AmazonWebApplication1.csproj", "AmazonWebApplication1/"]
RUN dotnet restore "AmazonWebApplication1/AmazonWebApplication1.csproj"
COPY . .
WORKDIR "/src/AmazonWebApplication1"
RUN dotnet build "AmazonWebApplication1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmazonWebApplication1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AmazonWebApplication1.dll"]