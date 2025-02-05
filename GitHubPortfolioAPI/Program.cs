using GitHubServiceLibrary;

var builder = WebApplication.CreateBuilder(args);


var folderPath = builder.Configuration["folderPath"];
// Add services to the container.
builder.Services.Configure<GitHubOptions>(builder.Configuration.GetSection("GitHubOptions"));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IGitHubService, GitHubService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
