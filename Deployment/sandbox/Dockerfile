FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR usr/src/company-service

COPY . .
RUN dotnet publish -c Release

COPY signature.txt .
COPY ./bin/Release/netcoreapp3.1/publish/ .

ENV ENVIRONMENT 'sandbox'
ENV JAEGER_AGENT_PORT '6831'

ENTRYPOINT ["dotnet", "company-service.dll"]