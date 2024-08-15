using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Configuration;
using System.Text;
using Taskhub.Common;
using Taskhub.Common.Exceptionhandling;
using Taskhub.Common.HealthChecks;
using Taskhub.Common.SharedMethods;
using Taskhub.Entities;
using Taskhub.Services.AuthService;
using Taskhub.Services.ProjectService;
using TaskHubApi.StartupConfig;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<TaskhubDbContext>(options =>
{
    options.UseSqlServer(ConnectionString);
});


builder.Services.AddScoped<SharedMethods>();
builder.Services.AddScoped<APIResponse<object>>();  
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IExceptionhandling, Exceptionhandling>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
        c=>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
        });
builder.Services.AddHealthChecks()
                .AddCheck<RandomHealthChecks>(name: "Site Health Check")
                .AddCheck<RandomHealthChecks>(name: "Database Health Check");

builder.Services.AddResponseCaching();

builder.Services.AddHealthChecksUI(opts =>
{
    opts.AddHealthCheckEndpoint(name: "api", uri: "/health");
    opts.SetEvaluationTimeInSeconds(seconds: 5);
    opts.SetMinimumSecondsBetweenFailureNotifications(seconds: 10);
}).AddInMemoryStorage();

builder.Services.AddWatchDogServices();


builder.Services.AddAuthentication(defaultScheme: "Bearer")
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>(key: "Authentication:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>(key: "Authentication:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>(key: "Authentication:SecretKey")!))
        };
    }

    );


builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("SuperadminOnly", policy =>
            policy.RequireRole("Superadmin"));
    opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
});

builder.Services.AddMemoryCache();
builder.AddRateLimitServices();

//Logging Functionality 
builder.Host.UseSerilog((ctx, lc)=>lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));



var app = builder.Build();

app.UseWatchDogExceptionLogger();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseSerilogRequestLogging();
    
app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks(pattern:"/health",options:new HealthCheckOptions
    {
     ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse 
});
app.UseHealthChecksUI();

app.UseWatchDog(opts =>
{
    opts.WatchPageUsername = app.Configuration.GetValue<string>(key: "WatchDog:UserName");
    opts.WatchPagePassword = app.Configuration.GetValue<string>(key: "WatchDog:Password");
    opts.Blacklist = "health";
});

app.UseIpRateLimiting();

app.Run();
