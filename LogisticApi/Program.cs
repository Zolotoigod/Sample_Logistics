using LogisticApi.Abstraction.Repositories;
using LogisticApi.DBContext;
using Microsoft.EntityFrameworkCore;
using MySqlRepositories.Repositories;
using Servises;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init app");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<SampleLogisticContext>(
        opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("LogisticDB")), ServiceLifetime.Singleton);

    builder.Services.AddSingleton<IArticleRepository, ArticlesRepository>()
        .AddSingleton<IDocumentRepository, DocumentRepository>();

    builder.Services.AddSingleton<ILogisticService, LogisticService>();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

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
}
catch (Exception ex)
{
    logger.Error(ex);
}
finally
{
    LogManager.Shutdown();
}

