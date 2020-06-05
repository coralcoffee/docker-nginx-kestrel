FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster 

COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]

EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "run"]