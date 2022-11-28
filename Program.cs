using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();

    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    
    builder.Logging.AddSerilog(logger, dispose: true);
    
    builder.Host.UseSerilog((context, services, configuration) => 
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));
    
    builder.Services.AddDbContext<WebAppContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("WebAppContext") ?? throw new InvalidOperationException("Connection string 'WebAppContext' not found.")));

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging(loggingOptions =>
    {
        loggingOptions.IncludeQueryInRequestPath = true;
    });
    
    app.UseRouting();
    app.UseAuthentication();;

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}