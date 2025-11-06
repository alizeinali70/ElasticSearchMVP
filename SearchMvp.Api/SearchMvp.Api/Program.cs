using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SearchMvp.Api;
using SearchMvp.Api.Data;
using SearchMvp.Api.Search;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Elasticsearch client
builder.Services.Configure<EsSettings>(builder.Configuration.GetSection("Elasticsearch"));
builder.Services.AddSingleton(sp =>
{
    var cfg = sp.GetRequiredService<IOptions<EsSettings>>().Value;
    var settings = new ElasticsearchClientSettings(new Uri(cfg.Uri));

    return new ElasticsearchClient(settings);
});

builder.Services.AddScoped<IProductSearch, ElasticsearchProductSearch>();

// Swagger + CORS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ng", p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200"));
});

var app = builder.Build();

// Middleware
app.UseCors("ng");
app.UseSwagger();
app.UseSwaggerUI();

// Default route (redirects to Swagger UI)
app.MapControllers();

app.Run();