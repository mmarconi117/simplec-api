using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Specify the Angular app's URL
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add services for MVC controllers with views
builder.Services.AddControllersWithViews();

// Add HTTP client services
builder.Services.AddHttpClient();

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAngular");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles(); // Serve static files

app.UseRouting(); // Enable routing

app.UseAuthorization(); // Enable authorization

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map the API controllers
app.MapControllers();

// Run the application
app.Run();
