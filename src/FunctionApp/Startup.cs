using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]
namespace FunctionApp;

public class Startup : FunctionsStartup
{
    public IConfiguration Configuration { get; set; }

    public Startup() { }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        Configuration = builder.ConfigurationBuilder.Build();
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton(Configuration);
    }
}
