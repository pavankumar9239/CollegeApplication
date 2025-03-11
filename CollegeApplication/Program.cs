
using CollegeApplication.Logger;
using Serilog;
using Repository.DBContext;
using Microsoft.EntityFrameworkCore;

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
                options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDB")));

            //builder.Logging.AddLog4Net();

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

            app.UseAuthorization();

            app.MapControllers();

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
        }
    }
}
