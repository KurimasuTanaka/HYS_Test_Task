using DataAccess;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDataAccess();

        builder.Services.AddControllers();


        var app = builder.Build();

        app.UseHttpsRedirection();

        app.Run();
    }
}

