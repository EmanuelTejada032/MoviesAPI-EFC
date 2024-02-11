using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI_EFC;
using MoviesAPI_EFC.Helpers;
using MoviesAPI_EFC.Services.Contract;
using MoviesAPI_EFC.Services.Implementation;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"),
    sqlServerOptions => sqlServerOptions.UseNetTopologySuite()
));

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid:4326));
builder.Services.AddSingleton<HashService>();
builder.Services.AddScoped<MovieExistAttribute>();

builder.Services.AddSingleton(provider =>
{
    var mConfig = new MapperConfiguration(conf =>
    {
        var geometryFactory = provider.GetRequiredService<GeometryFactory>();   
        conf.AddProfile(new AutoMapperProfile(geometryFactory));
    });

    return mConfig.CreateMapper();
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtkey"])),
                    ClockSkew = TimeSpan.Zero
                });


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("isAdmin"));
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddTransient<IFileManager, FileManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
