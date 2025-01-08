using Microsoft.EntityFrameworkCore;
using ToDoApp.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para ouvir portas específicas
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5251); // Porta HTTP
    options.ListenAnyIP(7239, listenOptions => listenOptions.UseHttps()); // Porta HTTPS
});

// Adicionar serviços ao contêiner
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adicionar serviços necessários
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar o CORS configurado
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
