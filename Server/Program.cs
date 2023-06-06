using CPM.Server.Configs;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

// db conn string from env

builder.Services.Configure<DefaultConfigurations>(
    builder.Configuration.GetSection("DefaultConfigurations"));

builder.Services.AddDbContext<CpmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CPMDatabase")));

builder.Services.AddScoped<IEntryService, EntryService>();
builder.Services.AddScoped<IAllocationService, AllocationService>();

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Server", Version = "v1" });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);
var app = builder.Build();
// database migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CpmDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
