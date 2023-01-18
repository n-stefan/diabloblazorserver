using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Serve published (WebAssembly AOT compiled) app
if (builder.Configuration["ServePublished"] == bool.TrueString)
{
    var webRoot = Path.Combine(builder.Environment.ContentRootPath, "bin", "Release", "net7.0", "publish", "wwwroot");
    if (Directory.Exists(webRoot))
    {
        builder = WebApplication.CreateBuilder(new WebApplicationOptions { WebRootPath = webRoot, EnvironmentName = "Production" });
        Console.WriteLine("***** Publish folder found => serving WebAssembly AOT compiled app.");
    }
    else
    {
        Console.WriteLine("***** Publish folder not found => not serving WebAssembly AOT compiled app.");
    }
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

app.MapFallbackToFile("index.html");

await app.RunAsync();
