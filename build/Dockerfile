ARG SONAR_PROJECT_VERSION

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
ARG SONAR_PROJECT_VERSION
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
ARG SONAR_PROJECT_VERSION
ENV PATH="$PATH:/root/.dotnet/tools"
WORKDIR /src

# Start Sonar Scanner
#RUN dotnet sonarscanner begin \
#    /k:"SemVer-Service" \
#    /v:"v$SONAR_PROJECT_VERSION" \
#    /d:sonar.host.url="https://sonar.t3winc.com" \
#    /d:sonar.login="b4c0dd76854682cd8e6a81061044b0ab0dba2c9c" \
#    /d:sonar.cs.opencover.reportsPaths=/coverage.opencover.xml

# Restore NuGet packages
COPY ["src/version.api/version.api.csproj", "version.api/version.api.csproj"]
COPY ["src/version.common/version.common.csproj", "version.common/version.common.csproj"]
COPY ["src/version.data/version.data.csproj", "version.data/version.data.csproj"]
COPY ["src/version.domain/version.domain.csproj", "version.domain/version.domain.csproj"]
COPY ["src/semverservice.sln","semverservice.sln"]
RUN dotnet restore

# Copy the rest of the files over
COPY src/. .

# Build the application
RUN dotnet publish -c Release -o /out/

# End Sonar Scanner
#RUN dotnet sonarscanner end /d:Sonar.login="b4c0dd76854682cd8e6a81061044b0ab0dba2c9c"

FROM base AS final
WORKDIR /app
COPY --from=build /out .
CMD ["sh", "/https/update_cert.sh"]
ENTRYPOINT ["dotnet", "version.api.dll"]