# 1. სთეიჯი: ბილდი (.NET 9 SDK-ს გამოყენებით)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ვაკოპირებთ .csproj ფაილს და ვატარებთ restore-ს
COPY ["PhishingDetectorApi.csproj", "./"]
RUN dotnet restore "PhishingDetectorApi.csproj"

# ვაკოპირებთ დანარჩენ კოდს და ვკეთებთ Publish-ს
COPY . .
RUN dotnet publish "PhishingDetectorApi.csproj" -c Release -o /app/publish

# 2. სთეიჯი: Runtime (მსუბუქი გარემო გაშვებისთვის)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# პორტის მიმაგრება, რომელსაც Render-ი გამოიყენებს
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PhishingDetectorApi.dll"]