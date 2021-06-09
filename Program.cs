using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Serve published (WebAssembly AOT compiled) app
//builder.WebHost.UseWebRoot(@$"{builder.Environment.WebRootPath}\..\diabloblazor\bin\Release\net6.0\publish\wwwroot");

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
