using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC;
using MoviesAPI_EFC.Services.Contract;
using MoviesAPI_EFC.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

builder.Services.AddScoped(provider =>
{
    var mConfig = new MapperConfiguration(conf =>
    {
        conf.AddProfile(new AutoMapperProfile());
    });

    return mConfig.CreateMapper();
});

builder.Services.AddTransient<IFileManager, FileManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
