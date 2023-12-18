using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

builder.Services.AddScoped(provider =>
{
    var mConfig = new MapperConfiguration(conf =>
    {
        conf.AddProfile(new AutoMapperProfile());
    });

    return mConfig.CreateMapper();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
