using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

var buildConfiguration = builder.Configuration["BuildConfiguration"];
var targetFramework = builder.Configuration["TargetFramework"];
// Serve published (WebAssembly AOT compiled) app
if (string.Equals(builder.Configuration["ServePublished"], bool.TrueString, StringComparison.Ordinal) && !string.IsNullOrWhiteSpace(buildConfiguration) && !string.IsNullOrWhiteSpace(targetFramework))
{
    string bin, publish, wwwroot;
    var contentRoot = builder.Environment.ContentRootPath;
    var webRoot = contentRoot.Contains(buildConfiguration) && contentRoot.Contains(targetFramework) && contentRoot.EndsWith(nameof(publish)) ?
        // From the command line
        Path.Combine(contentRoot, nameof(wwwroot)) :
        // From the IDE
        Path.Combine(contentRoot, nameof(bin), buildConfiguration, targetFramework, nameof(publish), nameof(wwwroot));
    if (Directory.Exists(webRoot))
    {
        builder = WebApplication.CreateBuilder(new WebApplicationOptions { WebRootPath = webRoot, EnvironmentName = Environments.Production });
        Console.WriteLine($"***** '{webRoot}' found => serving WebAssembly AOT compiled app.");
    }
    else
    {
        Console.WriteLine($"***** '{webRoot}' not found => not serving WebAssembly AOT compiled app.");
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
