using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using Microsoft.EntityFrameworkCore;
using MySqlRepositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SampleLogisticContext>(
    opt=> opt.UseSqlServer(builder.Configuration.GetConnectionString("LogisticDB")), ServiceLifetime.Singleton);

builder.Services.AddSingleton<IArticleRepository, ArticlesRepository>()
    .AddSingleton<IDocumentRepository, DocumentRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
