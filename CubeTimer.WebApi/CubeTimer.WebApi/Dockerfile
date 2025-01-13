FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet build

EXPOSE 5168

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5168"]
