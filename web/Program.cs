using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using seq_demo.Filters;
using seq_demo.Helper;
using Serilog;
using Serilog.Events;
using serilog_seq_demo.Adapter.Common;
using serilog_seq_demo.Common.Utility;
using serilog_seq_demo.Core.Module.Admin;
using serilog_seq_demo.Core.Module.Admin.Implement;
using serilog_seq_demo.Core.Module.Message;
using serilog_seq_demo.Core.Module.Message.Implement;
using serilog_seq_demo.DataServices.DAI;
using serilog_seq_demo.DataServices.DAO;

// 指定 LOG 輸出到 Console 及 Seq
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    // Filter out ASP.NET Core infrastructre logs that are Information and below
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    DBConfig.Configure(builder.Services);

    // 使用 Serilog
    builder.Host.UseSerilog();


    // DI
    builder.Services.AddScoped<IAdminDAO, AdminDAO>();
    builder.Services.AddScoped<IAdminModule, AdminModule>();
    builder.Services.AddScoped<IMessageModule, MessageModule>();

    // Add services to the container.
    builder.Services.AddControllersWithViews(config =>
    {
        // 註冊 serilog action filter
        config.Filters.Add(typeof(SerilogLoggingActionFilter));
    });

    builder.Services.AddHttpClient();

    builder.Services.AddHttpClient<IMessageModule, MessageModule>()
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
        .AddPolicyHandler(GetCircuitBreakerPolicy());

    ServiceLocator.SetLocatorProvider(builder.Services.BuildServiceProvider());

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
    // 設置 Serilog
    app.UseSerilogRequestLogging(options =>
    {
        // options.MessageTemplate = "Handled  {RequestPath}";
        options.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
        options.GetLevel = LogHelper.CustomGetLevel;
    });

    app.UseRouting();
    app.UseAuthorization();
    app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .CircuitBreakerAsync(1, TimeSpan.FromSeconds(30),
            (e,t,c)=>
            {
                Log.Information("過載保護");
                Console.WriteLine("過載保護");
            },
            c=>
            {
                Log.Information("恢復正常");
                Console.WriteLine("恢復正常");
            });
}