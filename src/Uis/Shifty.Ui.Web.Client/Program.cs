using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shifty.Ui.Shared.Services;
using Shifty.Ui.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the Shifty.Ui.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();

await builder.Build().RunAsync();
