using BookCatalogeApi.Models;
using Microsoft.EntityFrameworkCore;
using BookCatalogeApi.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var secureSchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type=SecuritySchemeType.Http,
        BearerFormat="JWT",
        Scheme="bearer"
    };
    opt.AddSecurityDefinition("Bearer", secureSchema);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer",
                }
            },
            new string[]{}
        }

    });
});

builder.Services.AddDbContext<ContextDataBase>(opt =>
{
    opt.UseNpgsql(builder.Configuration["ConnectionStrings"]);
});

builder.Services.AddDbContext<IdentityContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration["IdentityStrings"]);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = true;
    opt.SignIn.RequireConfirmedEmail = false;
    opt.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = AuthOptions.ISSUER,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.UseStaticFiles();
app.MapControllers();

app.Run();

