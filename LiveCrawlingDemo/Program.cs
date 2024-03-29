using Hangfire.SqlServer;
using Hangfire;
using LiveCrawlingDemo;
using Microsoft.EntityFrameworkCore;
using Hangfire.Common;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register context 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(connectionString, o => o.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name)));

//Register for DI
builder.Services.AddTransient<ISendHubService, SendHubService>();
builder.Services.AddTransient<ILotteryService, LotteryService>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });

// Hangfire
var options = new SqlServerStorageOptions
{
    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    QueuePollInterval = TimeSpan.Zero
};
builder.Services.AddHangfire(config
    => config.UseSqlServerStorage(connectionString, options)
    .WithJobExpirationTimeout(TimeSpan.FromHours(1)));
builder.Services.AddHangfireServer();

// SignalR
builder.Services.AddSignalR();

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

app.UseRouting();

app.UseAuthorization();
app.UseHangfireDashboard("/LotteryJobs");
var recurringJobs = app.Services.GetService<IRecurringJobManager>();
// "*/10 * * * *" min(0-59)/second hour(0-23) dayofmonth(1-31) month(1-12) dayofweek(0-6)
recurringJobs.AddOrUpdate("CreateNewLotteryResult", Job.FromExpression<ISendHubService>(x => x.AddNewLotteryResult()), "10 18 * * *", TimeZoneInfo.Local);
recurringJobs.AddOrUpdate("UpdateLiveLotteryResult", Job.FromExpression<ISendHubService>(x => x.UpdateLiveLotteryResult()), "*/5 15-40/1 18 * * *", TimeZoneInfo.Local);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHangfireDashboard();
app.MapHub<UpdateLotteryHub>(app.Configuration["SignalR:HubName"] ?? "Hub");
app.Run();
