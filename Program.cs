using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Serve published (WebAssembly AOT compiled) app
if (builder.Configuration["ServePublished"] == bool.TrueString)
{
    var webRoot = Path.Combine(builder.Environment.ContentRootPath, "..", "diabloblazor", "bin", "Release", "net7.0", "publish", "wwwroot");
    if (Directory.Exists(webRoot))
    {
        builder.WebHost.UseWebRoot(webRoot);
    }
    else
    {
        Console.Error.WriteLine("***** Publish folder not found => NOT serving WebAssembly AOT compiled app.");
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
