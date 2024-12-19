using Microsoft.EntityFrameworkCore;
using WebApp_OOP_Lr9.DataBase;
using WebApp_OOP_Lr9.Servises;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddDataAnnotationsLocalization();
builder.Services.AddRazorPages();


builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer("Data Source=DESKTOP-9L7NQAF;Initial Catalog=WebApplicationDb;Integrated Security=True;Connect Timeout=30;TrustServerCertificate=True;"));

builder.Services.AddScoped<INewBuildingsService, NewBuildingsService>();
builder.Services.AddScoped<IPropertiesService, PropertiesService>();
builder.Services.AddScoped<IFinancingOptionsService, FinancingOptionsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseDeveloperExceptionPage();

app.UseAuthorization();

app.MapRazorPages();

app.Run();