using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentSync.Data.Data;
using System.Configuration;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using StudentSync.Service.Http;
using StudentSync.Web.Controllers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<StudentSyncDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("StudentSyncCon")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/Auth/Login";
           options.LogoutPath = "/Auth/Logout";
           options.AccessDeniedPath = "/Auth/AccessDenied";
       });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();  

builder.Services.AddScoped<IHttpService, HttpService>();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


builder.Services.AddHttpClient();


var baseUrl = builder.Configuration.GetValue<string>("BaseUrl");
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(baseUrl),
    Timeout = Timeout.InfiniteTimeSpan

});


//builder.Services.AddHttpClient<BatchController>(client =>
//{
//    client.BaseAddress = new Uri(baseUrl);
//    client.DefaultRequestHeaders.Accept.Clear();
//    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//});

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
app.UseCors("AllowAllOrigins");


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
