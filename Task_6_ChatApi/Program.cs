using Microsoft.EntityFrameworkCore;
using Task_6_ChatApi.Core.Context;
using Task_6_ChatApiServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(config =>
{
    //string connectionString = builder.Configuration.GetConnectionString("ConnectionPostgresSql")!;
    //config.UseNpgsql(connectionString);

    config.UseInMemoryDatabase("Memory");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(cors =>
    {
        cors.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
