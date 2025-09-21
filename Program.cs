using Evoltis.Data;
using Evoltis.Mapping;
using Evoltis.Repositories;
using Evoltis.Services;
using Evoltis.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
       new MySqlServerVersion(new Version(8, 0, 30)))); 

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<UsuarioProfile>());

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditarUsuarioValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DomicilioValidator>();

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
