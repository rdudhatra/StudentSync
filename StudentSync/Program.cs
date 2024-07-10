using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Core.Services;
using StudentSync.Data.Data;
using System.Configuration;
using StudentSync.Core.Services.Interfaces;

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

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICourseServices, CourseServices>();
builder.Services.AddScoped<ICourseFeeService, CourseFeeService>();
builder.Services.AddScoped<ICourseExamServices, CourseExamServices>();
builder.Services.AddScoped<ICourseSyllabusService, CourseSyllabusService>();
builder.Services.AddScoped<IBatchService, BatchService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IStudentAssessmentService, StudentAssessmentService>();
builder.Services.AddScoped<IStudentAttendanceService, StudentAttendanceService>();
builder.Services.AddScoped<IStudentInstallmentService, StudentInstallmentService>();


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
//builder.Services.AddHttpClient("WebApiClient", client =>
//{
//    client.BaseAddress = new Uri(builder.Configuration["WebApiClient:BaseAddress"]);
//    client.Timeout = TimeSpan.FromMinutes(2); // Example: 2 minutes timeout
//});

var baseUrl = builder.Configuration.GetValue<string>("BaseUrl");
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(baseUrl),
    Timeout = Timeout.InfiniteTimeSpan
}) ;


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
