using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using PhinaMart.Helpers;
using PhinaMart.Models;
using PhinaMart.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("PhinaMart");
builder.Services.AddDbContext<PhinaMartContext>(x => x.UseSqlServer(connectionString));

// Add Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add automapper Register
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add authentication Login
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/AccessDenied";
})
.AddFacebook(options =>
{
    options.AppId = "1685959258607865";
    options.AppSecret = "18cee327c0c9da060db8123289d7781d";
})
.AddGoogle(options =>
{
    options.ClientId = "908460529684-mkhdgbf7uclr15gel8tm2sbuogi8pvo0.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["PaypalOptions:AppId"],
    builder.Configuration["PaypalOptions:AppSecret"],
    builder.Configuration["PaypalOptions:Mode"]
));

builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddSingleton<IVnPayService, VnPayService>();
builder.Services.AddScoped<CommentService, CommentServiceImpl>();
builder.Services.AddScoped<WishlistService, WishListServiceImpl>();
builder.Services.AddScoped<StaticalService, StaticalServiceImpl>();
builder.Services.AddScoped<VoucherService, VoucherServiceImpl>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
