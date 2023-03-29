using MyLab.WebErrors;

namespace TestServerNet7;

public class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers(b => b.AddExceptionProcessing());

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}