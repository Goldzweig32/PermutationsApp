using PermutationsApp.Services;

namespace PermutationsApp;

static class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddSingleton<StatsService>();
        builder.Services.AddSingleton<EnglishDictionaryService>();
        builder.Services.AddSingleton<PermutationsService>();

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAuthorization();

        app.MapControllers();
        
        await app.RunAsync("http://localhost:8000");
    }
}