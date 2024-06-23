using ApiPeliculas.Migraciones;
using ApiPeliculas.Modelos;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repositorio;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoriaRepositorio,Categoria_Repositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, Pelicula_Repositorio>();
builder.Services.AddScoped<IUsuarioRepositorio,UsuarioRepositorio>();
builder.Services.AddAutoMapper(typeof(Peliculas_Mapper));
// Add services to the container.

builder.Services.AddControllers(x =>
{
    x.CacheProfiles.Add("PorDefecto30", new CacheProfile { Duration = 30 }); 
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Cadena_Coneccion")));

builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<AplicationDbContext>();
 builder.Services.AddCors(x => x.AddPolicy("PolicyCors",builder =>
 {
     builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
 }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PolicyCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
