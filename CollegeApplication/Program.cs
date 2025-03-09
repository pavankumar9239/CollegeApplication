
using CollegeApplication.Logger;

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

            services.AddControllers(options => options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IMyLogger, LogToFile>();
        }
    }
}
