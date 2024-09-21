# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#start For .NET dev certs Not For Production
#RUN dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\cafenetapi.pfx -p crypticpassword
#RUN dotnet dev-certs https --trust
#end
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cafe-NET-API.csproj", "."]
RUN dotnet restore "./Cafe-NET-API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Cafe-NET-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Cafe-NET-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
#start For .NET dev certs Not For Production
#WORKDIR /appCOPY --from=publish /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
#end
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cafe-NET-API.dll"]

#sample "docker run" for dev: docker run --rm -it -p 8060:8080 cafenetapi
#Sample "docker run" for https: docker run --rm -it -p 8061:8080 -p 8060:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORTS=8060 -e ASPNETCORE_Kestrel__Certificates__Default__Password="crypticpassword" -e ASPNETCORE_Kestrel__Certificates__Default__Path=c:\https\cafenetapi.pfx -v %USERPROFILE%\.aspnet\https:C:\https\ --user ContainerAdministrator cafenetapi