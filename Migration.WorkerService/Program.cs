using Microsoft.EntityFrameworkCore;
using Migration.WorkerService;
using WebApplication1.Models;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"),
        options => { options.MigrationsAssembly("Migration.WorkerService"); });
});

var host = builder.Build();
host.Run();