
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using WebApi_Sample.Models.Contexts;
using WebApi_Sample.Models.Services;
using WebApi_Sample.Models.Services.Validator;

namespace WebApi_Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();

            builder.Services.AddScoped<ITokenValidator, TokenValidate>();


            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = builder.Configuration["JWtConfig:issuer"],
                    ValidAudience = builder.Configuration["JWtConfig:audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWtConfig:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
                configureOptions.SaveToken = true; // HttpContext.GetTokenAsunc();
                configureOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //log 
                        //........
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        //log
                        var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();
                        return tokenValidatorService.Execute(context);
                    },
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;

                    },
                    OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;

                    },
                    OnForbidden = context =>
                    {
                        return Task.CompletedTask;

                    }
                };

            });

            builder.Services.AddEndpointsApiExplorer();

            string connection = "Data Source=.;Initial Catalog=WebApi;Integrated Security=True;TrustServerCertificate=True";
            builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DataBaseContext>(option => option.UseSqlServer(connection));
            builder.Services.AddScoped<TodoRepository, TodoRepository>();
            builder.Services.AddScoped<CategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<UserRepository, UserRepository>();
            builder.Services.AddScoped<UserTokenRepository, UserTokenRepository>();

            builder.Services.AddApiVersioning(Options =>
            {
                Options.AssumeDefaultVersionWhenUnspecified = true;
                Options.DefaultApiVersion = new ApiVersion(1, 0);
                Options.ReportApiVersions = true;
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi.Bugeto.xml"), true);

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi_Sample", Version = "1" });
                //c.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApi_Sample", Version = "2" });

                //c.DocInclusionPredicate((doc, apiDescription) =>
                //{
                //    if (!apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                //    var version = methodInfo.DeclaringType
                //        .GetCustomAttributes<ApiVersionAttribute>(true)
                //        .SelectMany(attr => attr.Versions);

                //    return version.Any(v => $"v{v.ToString()}" == doc);
                //});

                var security = new OpenApiSecurityScheme
                {
                    Name = "JWT Auth",
                    Description = "توکن خود را وارد کنید- دقت کنید فقط توکن را وارد کنید",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(security.Reference.Id, security);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { security , new string[]{ } }
                });
            });

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    //options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
                });
                
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
