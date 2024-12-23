using Microsoft.AspNetCore.Http.Features;
using PassportGenerationSystem.DAL;

var builder = WebApplication.CreateBuilder(args);


// Configure file size limit for uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;  // Increase file size limit
});
builder.Services.AddScoped<User_DAL>();
builder.Services.AddScoped<Account_DAL>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Helps mitigate cross-site scripting attacks
    options.Cookie.IsEssential = true; // Required for cookies to work in GDPR-compliant applications
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable routing
app.UseRouting();

// Enable session middleware
app.UseSession();

// Authorization middleware (optional, based on your authentication needs)
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
