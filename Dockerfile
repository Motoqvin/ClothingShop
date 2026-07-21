FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/ClothingStoreApp.Core/ClothingStoreApp.Core.csproj src/ClothingStoreApp.Core/
COPY src/ClothingStoreApp.Infrastructure/ClothingStoreApp.Infrastructure.csproj src/ClothingStoreApp.Infrastructure/
COPY src/ClothingStoreApp.Presentation/ClothingStoreApp.Presentation.csproj src/ClothingStoreApp.Presentation/
 
RUN dotnet restore src/ClothingStoreApp.Presentation/ClothingStoreApp.Presentation.csproj
 
COPY src/ src/
 
RUN dotnet publish src/ClothingStoreApp.Presentation/ClothingStoreApp.Presentation.csproj \
    -c Release \
    -o /app/publish \
    --no-restore
 
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN addgroup --system --gid 1000 appgroup \
 && adduser --system --uid 1000 --ingroup appgroup appuser
 
COPY --from=build /app/publish .

RUN mkdir -p /app/wwwroot/uploads/products \
 && chown -R appuser:appgroup /app
 
USER appuser
 
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
 
ENTRYPOINT ["dotnet", "ClothingStoreApp.Presentation.dll"]