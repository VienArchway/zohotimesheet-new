FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app

COPY prod .

EXPOSE 5000

CMD ["dotnet", "api.dll"]