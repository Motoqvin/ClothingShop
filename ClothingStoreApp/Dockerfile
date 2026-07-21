FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/ClothingStoreApp.Core/ClothingStoreApp.Core.csproj ./ClothingStoreApp.Core/
COPY src/ClothingStoreApp.Infrastructure/ClothingStoreApp.Infrastructure.csproj ./ClothingStoreApp.Infrastructure/
COPY src/ClothingStoreApp.Presentation/ClothingStoreApp.Presentation.csproj ./ClothingStoreApp.Presentation/

RUN dotnet restore ./ClothingStoreApp.Presentation/ClothingStoreApp.Presentation.csproj

COPY src/ .

WORKDIR /src/ClothingStoreApp.Presentation

RUN dotnet publish -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

RUN addgroup --system --gid 1000 appgroup \
 && adduser --system --uid 1000 --ingroup appgroup appuser \
 && mkdir -p /app/wwwroot/uploads/products \
 && chown -R appuser:appgroup /app

USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ClothingStoreApp.Presentation.dll"]