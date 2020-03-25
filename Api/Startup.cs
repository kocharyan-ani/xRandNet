using System;
using System.Text;
using Api.Database.Context;
using Api.Database.Repositories;
using Api.Services;
using Api.Services.Bug;
using Core.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace Api {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            CustomLogger.WebMode = true;
            services.AddCors(options => {
                options.AddPolicy("EnableCORS",
                    builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });
            services.AddControllers();
            // Database
            services.AddDbContext<DatabaseContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                    mySqlOptions =>
                        mySqlOptions.ServerVersion(new ServerVersion(new Version(5, 7, 28), ServerType.MySql))));
            // Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAppRepository, AppRepository>();
            services.AddTransient<IInfoRepository, InfoRepository>();
            services.AddTransient<ILinkRepository, LinkRepository>();
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<IInfoRepository, InfoRepository>();
            services.AddTransient<IBugRepository, BugRepository>();
            services.AddTransient<IUserManualFileRepository, UserManualFileRepository>();
            // Services
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<LinkService>();
            services.AddScoped<NewsService>();
            services.AddScoped<InfoService>();
            services.AddScoped<BugService>();
            services.AddScoped<AppService>();
            // Authentication
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetSection("IssuerDomain").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration.GetSection("SecretKey").Value)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            // Authorization
            services.AddAuthorization(options => {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("role", "Admin"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseCors("EnableCORS");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}