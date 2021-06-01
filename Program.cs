using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting;

var app = WebApplication.CreateBuilder(args).Build();

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

app.Run();
