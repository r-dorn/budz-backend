using System.Text;
using budz_backend.Middleware;
using budz_backend.Models.Settings.Jwt;
using budz_backend.Models.Settings;
using budz_backend.Services.MongoServices.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Opw.HttpExceptions.AspNetCore;
using budz_backend.Models.Validator;
using FluentValidation;
using budz_backend.Models.User;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
ConnectionMultiplexer redisMultiplexer = ConnectionMultiplexer.Connect(
    new ConfigurationOptions
    {
        User = builder.Configuration.GetSection("Redis:Username").Value,
        Password = builder.Configuration.GetSection("Redis:Password").Value,
        EndPoints = { builder.Configuration.GetSection("redis:ConnectionString").Value }
    }
);

builder.Services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);


builder.Services.AddMvc().AddHttpExceptions();


builder.Services.AddScoped<UserService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<User>, UserValidator>();

builder.Services.Configure<MongoConfig>(
    builder.Configuration.GetSection("Mongodb")
);


builder.Services.Configure<RedisSettings>(
    builder.Configuration.GetSection("Redis")
);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var app = builder.Build();

app.UseHttpExceptions();
app.UseSession();
app.UseMiddleware<JwtMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();


app.Run();






