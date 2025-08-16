using DataAccess;
using Database;
using HYStest.Services.MeetingSchedulerService;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContextFactory<Context>();

        builder.Services.AddControllers();

        builder.Services.AddDataAccess();

        builder.Services.AddScoped<IMeetingSchedulerService, MeetingSchedulerService>();


        var app = builder.Build();

        app.MapControllers();

        app.UseHttpsRedirection();
        app.MapSwagger();
        app.UseSwaggerUI();
        app.Run();
    }
}

