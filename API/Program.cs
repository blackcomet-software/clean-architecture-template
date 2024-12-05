using System.Text;
using API.Configuration;
using API.Middlewares;
using Application;
using Carter;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add environment variables to configuration based on a naming convention
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.ConfigureHttpJsonOptions(options => options.ConfigureJsonOptions());

// swagger (this is steel needed if you use scalar)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.ConfigureSwaggerGeneration());

builder.Services.AddApplicationProject();
builder.Services.AddPersistanceProject();

// Authorization with supabase
builder.Services.AddAuthorization();
var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtSecret"]!);
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(bytes),
        ValidAudience = builder.Configuration["Authentication:ValidAudience"],
        ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalDevelopment",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "http://localhost:4200") // Add allowed origins
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Optional if you need cookies/auth
        });
});

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCarter(null, options => { options.WithEmptyValidators(); });

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseCors("AllowLocalDevelopment");

app.MapCarter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        // documentName becomes v1
        options.RouteTemplate = "openapi/{documentName}.json";
    });
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("[INSERT TITLE]")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Fetch);
    });
}

app.UseHttpsRedirection();

app.Run();