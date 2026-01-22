using FluentValidation;
using FoodStore.Server.Application.Behaviors;
using FoodStore.Server.Application.Common.Interfaces;
using FoodStore.Server.Application.Foods.Commands;
using FoodStore.Server.Application.Middlewares;
using FoodStore.Server.Application.Services;
using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Identity;
using FoodStore.Server.Identity.DataModels;
using FoodStore.Server.Infrastructure;
using FoodStore.Server.Infrastructure.Interceptor;
using FoodStore.Server.Infrastructure.ResendEmail;
using FoodStore.Server.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resend;
using Serilog;
using System.Text;
using DotNetEnv;

Env.Load();
var builder = WebApplication.CreateBuilder(args);

// Logging (Serilog)
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

// Services
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(nameof(JwtConfiguration)));

// Configure FoodStoreDbContext and attach SoftDeleteInterceptor
builder.Services.AddDbContext<FoodStoreDbContext>((sp, options) =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FoodStoreConnection"),
        sql => sql.MigrationsAssembly(typeof(FoodStoreDbContext).Assembly.FullName));
    options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
});

// Configure UserDbContext and attach SoftDeleteInterceptor
builder.Services.AddDbContext<UserDbContext>((sp, options) =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("IdentityConnection"),
        sql => sql.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName));
    options.AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddOptions();
builder.Services.AddHttpClient<ResendClient>();
builder.Services.Configure<ResendClientOptions>(o =>
{
    o.ApiToken = builder.Configuration["Resend:ApiKey"]!;
});
builder.Services.AddTransient<IResend, ResendClient>();
builder.Services.AddTransient<IEmailService, ResendEmailService>();


// load typed config so we use the same values for creation and validation
var jwtConfig = builder.Configuration
    .GetSection(nameof(JwtConfiguration))
    .Get<JwtConfiguration>() ?? throw new InvalidOperationException("Missing JwtConfiguration section");
// configure authentication/validation with the same secret used by TokenProvider
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtConfig.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
    {
        policy
            .WithOrigins("http://localhost:5106")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddSingleton<SoftDeleteInterceptor>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(CreateFood).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateFood.CreateFoodCommandValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header. Enter **only the token** (no 'Bearer ' prefix).",
        Scheme = "Bearer"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new List<string>()
        }
    });
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var roleName in Enum.GetNames(typeof(UserRole)))
    {
        var exists = await roleManager.RoleExistsAsync(roleName);
        if (!exists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}


// Middleware Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowSwagger");
app.UseStatusCodePages();

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
