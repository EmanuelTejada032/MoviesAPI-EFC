using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC;
using MoviesAPI_EFC.Services.Contract;
using MoviesAPI_EFC.Services.Implementation;
using NetTopologySuite;
using NetTopologySuite.Geometries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"),
    sqlServerOptions => sqlServerOptions.UseNetTopologySuite()
));

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid:4326));

builder.Services.AddSingleton(provider =>
{
    var mConfig = new MapperConfiguration(conf =>
    {
        var geometryFactory = provider.GetRequiredService<GeometryFactory>();   
        conf.AddProfile(new AutoMapperProfile(geometryFactory));
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
