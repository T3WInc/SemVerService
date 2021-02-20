#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS builder
#WORKDIR /source
#COPY ./src/version.client/version.client.csproj .
#RUN dotnet restore

#COPY ./src/version.client/* ./
#RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:3.1-alpine3.12
WORKDIR /app
COPY ./src/version.client/ .
#COPY --from=builder /app .
#ENTRYPOINT ["dotnet run --project /app "~/repo/semverservicesample/.git" "SemVerServiceSample" "d4756207-07ac-4ffa-beb0-3149ebe0fe19""]