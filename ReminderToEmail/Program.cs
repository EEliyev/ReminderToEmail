using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReminderToEmail.Core.Interfaces;
using ReminderToEmail.Core.Repositories;
using ReminderToEmail.Data;
using ReminderToEmail.Helper;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<ReminderContext>(op => op.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<MyActionFilter>();

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My Web API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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



builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:Key").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

builder.Services.AddRateLimiter(opt =>
{
    opt.AddFixedWindowLimiter("Basic", optionts =>
    {
        optionts.Window = TimeSpan.FromSeconds(12);
        optionts.PermitLimit = 4;
        optionts.QueueLimit = 2;
        optionts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    }).RejectionStatusCode = 429; 
});
builder.Services.AddRateLimiter(opt =>
{
    opt.AddSlidingWindowLimiter("Sliding", optionts =>
    {
        optionts.Window = TimeSpan.FromSeconds(12);
        optionts.PermitLimit = 4;
        optionts.QueueLimit = 2;
        optionts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        optionts.SegmentsPerWindow = 2;
    }).RejectionStatusCode = 429;
});

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
    .AllowAnyMethod();
}));

builder.Services.AddTransient<IEmailService, EmailService>();

var smtpConfig = builder.Configuration.GetSection("SmtpConfig").Get<SmtpConfig>();
var smtpClient = new SmtpClient(smtpConfig.Host, smtpConfig.Port)
{
    UseDefaultCredentials = false,
    Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password),
    EnableSsl = smtpConfig.EnableSsl
};

builder.Services.AddSingleton(smtpClient);

builder.Services.AddHostedService<ReminderProcessor>();

var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandling>();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
