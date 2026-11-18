using PhishingDetectorApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ვარეგისტრირებთ HttpClient-ს და სერვისს ერთად (AddHttpClient ავტომატურად ამატებს მას Scoped სიცოცხლის ვადით)
builder.Services.AddHttpClient<PhishingCheckerService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/asp5/swag
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run("http://0.0.0.0:8080");