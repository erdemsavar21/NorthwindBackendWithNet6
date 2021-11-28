using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot();

builder.Host.ConfigureAppConfiguration(
    (context,config) => { config.AddJsonFile("ocelot.json");}
);

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

app.UseRouting();

//  app.UseEndpoints(endpoints =>
//             {
//                 endpoints.MapControllers();
//             });

app.MapControllers();

app.UseOcelot();

app.Run();
