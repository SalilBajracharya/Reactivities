using API.Extensions;

var builder = WebApplication.CreateBuilder(args)
                .AddApplicationServices();

builder.Services.AddControllers();


var app = builder.Build();
app.SetupMiddleware().Wait();
app.Run();
