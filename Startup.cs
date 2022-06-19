using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Auth
{
    public class Startup
    {
        private const string jwtCookieName = "jwtCoooookie";
        private const string privateKey = "lkfjsdlfjsdklfjlsd";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IAuthorizationHandler, GenderMinAgeRequirementHandler>();
            services.AddSingleton<IAuthorizationHandler, GenderMinAgeRequirementHandler1>();
            services.AddSingleton<IAuthorizationHandler, NonAdminUserRequirementHandler>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config =>
            {
                config.ClaimsIssuer = "NikhilFromStartup";
                config.Challenge = "challengeString";
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "NikhilFromJwtClass",
                    ValidateAudience = true,
                    ValidAudience = "toTheOneIssued",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
                };
                config.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = config =>
                    {
                        var jwtToken = config.Request.Cookies.FirstOrDefault(cookie => cookie.Key == jwtCookieName).Value;
                        Console.WriteLine($"jwt token is: {jwtToken}");
                        config.Token = jwtToken;
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    },
                    OnChallenge = config =>
                    {
                        return Task.CompletedTask;
                    },
                    OnForbidden = config =>
                    {
                        return Task.CompletedTask;
                    },
                };
            });
            services.AddAuthorization(config =>
            {
                config.AddPolicy("MaleAbove21", config =>
                {
                    config.RequireClaim("gender1");
                    config.RequireClaim("age");
                    config.AddRequirements(new GenderMinAgeRequirement(21, "male"));
                });
                config.AddPolicy("nonAdminUser", config =>
                {
                    config.RequireClaim("role1");
                    config.AddRequirements(new NonAdminUserRequirement());
                });
                config.AddPolicy("both", config =>
                {
                    config.AddRequirements(new GenderMinAgeRequirement(30, "male"), new NonAdminUserRequirement());
                });
                config.AddPolicy("idMustBe434", config => {
                    config.RequireAssertion(context => {
                        string userId = context.User.FindFirst(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
                        return userId == "434";
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
