FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY bin/Release/netcoreapp3.1/publish App/

WORKDIR /App
ENTRYPOINT ["dotnet", "Backend.dll"]


# -- ENV for https
# ENV ASPNETCORE_URLS="https://+;http://+"
# ENV ASPNETCORE_HTTPS_PORT=8001
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password="thunder"
# ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/App/Backend.pfx"v