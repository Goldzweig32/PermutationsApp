using PermutationsApp.Singletons;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<StatsSingleton>();
builder.Services.AddSingleton<EnglishDictionarySingleton>();
EnglishDictionarySingleton.Instance.Initialize();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthorization();

app.MapControllers();

app.RunAsync("http://localhost:8000").GetAwaiter().GetResult();