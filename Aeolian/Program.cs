using Aeolian.Services;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(UserService));
builder.Services.Add(new ServiceDescriptor(typeof(UserService), UserService.GetInstance(builder.Configuration.GetConnectionString("AeolianConnection"))));

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

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Login}/{id?}");

    endpoints.MapControllerRoute(
        name: "homePage",
        pattern: "home",
        defaults: new { controller = "Home", action = "Index" });
});


app.Run();
