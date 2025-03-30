
using CollegeApplication.Logger;
using Serilog;
using Repository.DBContext;
using Microsoft.EntityFrameworkCore;
using CollegeApplication.Configurations;
using Repository.RepositoryExtensions;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CollegeApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //By default 4 types of logging will be included i.e, Debug, Console, EventSource, Event Log. That will be deafly when we used createBuilder method.
            //if we want to clear all loggers.
            //builder.Logging.ClearProviders();
            //if we want to add any providers, use below.
            //builder.Logging.AddConsole();
            //builder.Logging.AddDebug();


            ConfigureServices(builder.Services);

            builder.Services.AddDbContext<CollegeDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeDB")));

            //builder.Logging.AddLog4Net();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    //To allow all origins
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
                //options.AddPolicy("AllowAll", policy =>
                //{
                //    //To allow all origins
                //    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                //});
                options.AddPolicy("AllowLocalHost", policy =>
                {
                    //To Allow localhost origins
                    policy.WithOrigins("http://localhost:5197").AllowAnyHeader().AllowAnyMethod();
                });
                options.AddPolicy("AllowOnlyGoogle", policy =>
                {
                    //To Allow few origins
                    policy.WithOrigins("http://google.com", "http://mail.google.com", "http://drive.google.com").AllowAnyHeader().AllowAnyMethod();
                });
                options.AddPolicy("AllowOnlyMicrosoft", policy =>
                {
                    //To Allow few origins
                    policy.WithOrigins("http://microsoft.com", "http://onedrive.microsoft.com", "http://mail.microsoft.com").AllowAnyHeader().AllowAnyMethod();
                });
                //options.AddPolicy("MyTestPolicy", policy =>
                //{
                //    //To Allow few origins
                //    policy.WithOrigins("http://localhost:5197").AllowAnyHeader().AllowAnyMethod();

                //    //To allow all origins
                //    //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                //});
            });

            var keyJWTSecretGoogle = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretGoogle"));
            var keyJWTSecretMicrosoft = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretMicrosoft"));
            var keyJWTSecretLocal = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretLocal"));

            var googleAudience = builder.Configuration.GetValue<string>("GoogleAudience");
            var microsoftAudience = builder.Configuration.GetValue<string>("MicrosoftAudience");
            var localAudience = builder.Configuration.GetValue<string>("LocalAudience");

            var googleIssuer = builder.Configuration.GetValue<string>("GoogleIssuer");
            var microsoftIssuer = builder.Configuration.GetValue<string>("MicrosoftIssuer");
            var localIssuer = builder.Configuration.GetValue<string>("LocalIssuer");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("LoginForGoogle", options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretGoogle),

                    ValidateIssuer = true,
                    ValidIssuer = googleIssuer,

                    ValidateAudience = true,
                    ValidAudience = googleAudience
                };
            }).AddJwtBearer("LoginForMicrosoft", options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretMicrosoft),

                    ValidateIssuer = true,
                    ValidIssuer = microsoftIssuer,

                    ValidateAudience = true,
                    ValidAudience = microsoftAudience
                };
            }).AddJwtBearer("LoginForLocal", options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyJWTSecretLocal),

                    ValidateIssuer = true,
                    ValidIssuer = localIssuer,

                    ValidateAudience = true,
                    ValidAudience = localAudience
                };
            });

            #region Serilog settings
            //For using Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Log/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //If you want to use only serilog and not any other loggers.
            //builder.Host.UseSerilog();

            //If you want to use Serilog along with default loggers.
            //builder.Logging.AddSerilog();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseHttpsRedirection();

            app.UseRouting();

            //should be added before Authorization and after routing
            //app.UseCors("MyTestPolicy");
            //To use default policy
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("api/testingEndpoint",
                    context => context.Response.WriteAsync("Test Response"))
                    .RequireCors("AllowLocalHost");

                endpoints.MapControllers()
                .RequireCors("AllowAll");

                endpoints.MapGet("api/testingEndpoint2",
                    context => context.Response.WriteAsync("Test Response 2"));
            });

            //app.MapControllers();

            app.Run();
        }

        static void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); //options => options.ReturnHttpNotAcceptable = true

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //Use Serilog
            //services.AddSerilog();

            //services.AddScoped<IMyLogger, LogToFile>();

            //Adding DB context class from Repository project
            services.AddDbContext<CollegeDBContext>(options =>
            {
                options.UseSqlServer();
            });

            services.AddAutoMapper(typeof(AutoMapperConfig));

            RepositoryExtensions.AddRepositoriesExtensions(services);
        }
    }
}
