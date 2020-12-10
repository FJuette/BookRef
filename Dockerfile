FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

RUN cd /usr/lib && ln -s libgdiplus.so gdiplus.dll
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /build
COPY src/BookRef.Api/. .
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookRef.Api.dll"]
