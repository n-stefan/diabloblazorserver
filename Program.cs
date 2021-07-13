using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Serve published (WebAssembly AOT compiled) app
if (builder.Configuration["ServePublished"] == bool.TrueString)
{
    var webRoot = Path.Combine(builder.Environment.ContentRootPath, "..", "diabloblazor", "bin", "Release", "net6.0", "publish", "wwwroot");
    builder.WebHost.UseWebRoot(webRoot);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".mpq"] = "application/octet-stream";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });

app.UseBlazorFrameworkFiles();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapFallbackToFile("index.html"));

await app.RunAsync();
