using backend.Data.ModelsEF;
using backend.Interfaces;
using backend.Repositories;
using backend.Services;
using backend.Services.Imports;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=finix_database.db"));
builder.Services.AddHostedService<AuthSeedService>();
builder.Services.AddSingleton<JwtTokenService>();

builder.Services.AddCors(p => p.AddPolicy("CorsRules", build =>
{
    build.SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:5173", "http://localhost:3000")
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));
builder.Services.AddScoped<IInvoiceImport, InvoiceImport>();
builder.Services.AddHostedService<InvoiceImporterHostedService>();

builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Finix group system",
        Description = "Enpoints MVP desafio facturas",
    });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Token JWT de autorización para la cabecera de consultas. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };
    c.AddSecurityRequirement(securityRequirement);

});

// needed to load configuration from appsettings.json
builder.Services.AddOptions();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsRules"); 

app.UseAuthorization();

app.MapControllers();

app.Run();
