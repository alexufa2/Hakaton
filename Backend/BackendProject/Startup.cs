using TemplateApp.DAL;
using TemplateApp.MiddleWare;
using TemplateApp.Services;
using TemplateApp.Services.Hosted;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;
using System.Threading.Tasks;
using TemplateApp.Utils.Crypto;
using FileParsing;

namespace TemplateApp
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMyOrigin",
                builder => builder.WithOrigins("*"));
            });

            //builder => builder.WithOrigins("*").
            //WithMethods(new string[] { "GET", "PUT", "POST", "DELETE", "OPTIONS" }).WithHeaders(new string[] {
            //	"Content-Type"
            //	}).AllowCredentials());
            //services.AddAntiforgery(options =>
            //{
            //	options.CookieDomain = "contoso.com";
            //	options.CookieName = "X-CSRF-TOKEN-COOKIENAME";
            //	options.CookiePath = "Path";
            //	options.FormFieldName = "AntiforgeryFieldname";
            //	options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
            //	options.RequireSsl = false;
            //	options.SuppressXFrameOptionsHeader = false;
            //});

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidateAntiForgeryTokenAttribute());
            });

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //services.AddAuthentication("BasicAuthentication")
            //   .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            string secretKey = Configuration.GetSection("Default").GetValue<string>("secret");
            if (string.IsNullOrEmpty(secretKey)) throw new System.Exception("Ключ для токенов не может быть пустым !");

            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };

                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                x.SecurityTokenValidators.Add(
                    new GostTokenValidator(services.BuildServiceProvider(), key));
            });

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDataGovRuService, DataGovRuService>();
            services.AddHttpContextAccessor();
            services.AddHostedService<NotificationHostedService>();

            string fileDir = Configuration.GetSection("Default").GetValue<string>("fileDir");
            string appDir = Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            services.AddScoped<IFileService, FileService>(t => new FileService(appDir, fileDir));
            services.AddScoped<ITextFileParser, TextFileParser>();

            services.AddScoped<IMiddlewareFactory, MiddlewareFactory>();
            services.AddLogging();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IAntiforgery antiforgery)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            loggerFactory.AddConsole();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            //	app.UseCors("AllowMyOrigin");
            app.UseCors(builder =>
       builder.AllowAnyOrigin()
       .AllowAnyHeader()
       .AllowAnyMethod());


            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });





            app.UseAuthentication();

        }
    }
}
