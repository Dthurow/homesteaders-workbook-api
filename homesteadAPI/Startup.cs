using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using homesteadAPI.Models;
using Microsoft.EntityFrameworkCore.Proxies;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using homesteadAPI.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace homesteadAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Add authentication for Auth0
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Configuration["Auth0:Audience"];
                    options.SaveToken = true;
                });


            //Add support for scope validation
            services.AddAuthorization(options =>
                {
                    options.AddPolicy("standard_user", policy => policy.Requirements.Add(new HasScopeRequirement("standard_user", domain)));
                    options.AddPolicy("admin_user", policy => policy.Requirements.Add(new HasScopeRequirement("admin_user", domain)));
                });

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


            //pull in connection string
            string connectionString = null;
            string envVar = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrEmpty(envVar)){
                connectionString = Configuration["Connectionstrings:database"];
            }
            else{
                //parse database URL. Format is postgres://<username>:<password>@<host>/<dbname>
                var uri = new Uri(envVar);
                var username = uri.UserInfo.Split(':')[0];
                var password = uri.UserInfo.Split(':')[1];
                connectionString = "Host=" + uri.Host + "; Database=" + uri.AbsolutePath.Substring(1) + "; Username=" + username + "; Password=" + password + "; Port=" + uri.Port + "; SSL Mode=Require; Trust Server Certificate=true;";
            }

            services.AddDbContext<HomesteadDataContext>(opt =>
              opt.UseLazyLoadingProxies() //it will auto-load related entities when properties are accessed
              .UseNpgsql(connectionString)
              );

            //use newtonsoft json with loop handling so EFCore doesn't trigger circular loops when
            //serializing to JSON in the API controller
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "MM-dd-yyyy";

            });


            //basically a terrible policy that should ONLY be used during initial dev
            //for ease of use, since I don't know what sort of front-end I actually want yet
            services.AddCors(options =>
               {
                   options.AddPolicy("DevCustomCORS",
                   builder =>
                   {
                       builder.AllowAnyOrigin();
                       builder.AllowAnyMethod();
                       builder.AllowAnyHeader();
                   });
               });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HomesteadDataContext _context, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                try
                {
                    Config CustomConfig = new Config(_context, logger);
                    CustomConfig.InitializeDatabase();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Could not initialize database with test data");
                }
                //sets up the DB and seeds it with test data

                app.UseCors("DevCustomCORS");
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
