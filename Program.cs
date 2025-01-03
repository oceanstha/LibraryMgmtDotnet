using LibraryMgmt.Data;
using LibraryMgmt.Repository;
using LibraryMgmt.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LibraryDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookIssueRepository, BookIssueRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddSingleton<IPdfService, PdfService>();
builder.Services.AddRazorPages();




builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.Cookie.Name = "UserAuthCookie";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireClaim("Role", "User"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireClaim("Role", "Manager"));
    options.AddPolicy("AdminOrManagerPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Role" && (c.Value == "Admin" || c.Value == "Manager"))
        ));
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://www.w3.org",
                                              "https://localhost:7012");
                      });
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100_000_000; // 100 MB
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
