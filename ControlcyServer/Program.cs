using Common.Database.Database;
using ControlcyServer;
using ControlcyServer.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      builder =>    
          builder.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()
                      );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Controlcy", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
} 
    
    );
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddDbContext<ManagementDbContext>();
builder.Services.AddDbContext<ControlcyDbContext>();
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    var kestrelSection = context.Configuration.GetSection("Kestrel");

    serverOptions.ListenAnyIP(80);
    serverOptions.ListenAnyIP(443);

});
var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<BasicAuthMiddleware>();
///app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
