using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Asp.Versioning;
using System.Text;
using System.Text.Json.Serialization;
using diggie_server.src.shared.error;
using diggie_server.src.infrastructure.persistence;
using diggie_server.src.infrastructure.persistence.repositories;
using diggie_server.src.shop.features.product.get;
using diggie_server.src.shop.features.product.create;
using diggie_server.src.shop.features.product.delete;
using diggie_server.src.shop.features.product.update;
using diggie_server.src.identity.features.register;
using diggie_server.src.identity.features.login;
using diggie_server.src.infrastructure.auth.jwt;
using diggie_server.src.identity.features.otp.send;
using RazorLight;
using diggie_server.src.identity.features.otp;
using diggie_server.src.identity.features.reset;
using diggie_server.src.identity.features.otp.verify;
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates");
var engine = new RazorLightEngineBuilder()
    .UseFileSystemProject(templatePath)
    .UseMemoryCachingProvider()
    .Build();
builder.Services.AddSingleton<IRazorLightEngine>(engine);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddDbContext<AppDatabaseContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = builder.Configuration["JWT_SECRET"] ?? string.Empty;
var jwtIssuer = builder.Configuration["JWT_ISSUER"] ?? string.Empty;
var jwtAudience = builder.Configuration["JWT_AUDIENCE"] ?? string.Empty;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["X-Access-Token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5500",
            "http://127.0.0.1:5500"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<RepositoryUser>();
builder.Services.AddScoped<RepositoryOtp>();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<JwtStrategy>();

builder.Services.AddScoped<ErrorHandlerLogger>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<CreateProduct>();
builder.Services.AddScoped<GetProduct>();
builder.Services.AddScoped<UpdateProduct>();
builder.Services.AddScoped<DeleteProduct>();
builder.Services.AddScoped<RegisterHandler>();
builder.Services.AddScoped<LoginHandler>();
builder.Services.AddScoped<SendOTPHandler>();
builder.Services.AddScoped<SendStrukHandler>();
builder.Services.AddScoped<ResetHandler>();
builder.Services.AddScoped<VerifyOtpHandler>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Moso API v1");
    });
}
else
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowFrontend");

app.UseMiddleware<diggie_server.src.infrastructure.auth.guard.JwtGuardMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();