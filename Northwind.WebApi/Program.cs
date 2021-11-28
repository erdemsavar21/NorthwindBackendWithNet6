using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encription;
using Core.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Northwind.Business.DependencyResolvers.Autofac;
using Northwind.DataAccess.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});
// Add services to the container.

builder.Services.AddControllers();

// builder.Services.AddCors(options =>
//           {
//               options.AddPolicy("AllowOrigin", builder => builder.WithOrigins("https://localhost")); //sadece localhosttan gelen istekleri almak icin güvenlik
//           });

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
//builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });

builder.Services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule()
            });

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings))); //Kullanmiyoruz simdilik ama appsettings de tanimlanan attribute bu sekilde nesnelere atayabilriz
builder.Services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ESourcing.Products",
                    Version = "v1"
                });
            }); 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind.Products v1"));
            }

            //app.UseCors(builder => builder.WithOrigins("https://localhost").AllowAnyHeader());

            //app.ConfigureCustomExceptionMiddleware(); //Hata dönusünde detayli aciklamayi son kullaniciya göstermemek icin

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
