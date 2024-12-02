using Microsoft.EntityFrameworkCore;
using TA9_Graph_DB_Managment.Services;
using TA9_Graph_DB_Managment.SharedDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<INodeService, NodeService>();
builder.Services.AddScoped<IEdgeService, EdgeService>();
builder.Services.AddCors();
builder.Services.AddDbContext<GraphDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("TA9_Graph_service")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
