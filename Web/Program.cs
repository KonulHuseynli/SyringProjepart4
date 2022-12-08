using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Concrete;
using WebApp.Services.Abstract;
using Core.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>(); 

#region connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(x =>
x.UseSqlServer(connectionString, x => x.MigrationsAssembly("DataAccess")));
#endregion


#region useridentity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

})
    .AddEntityFrameworkStores<AppDbContext>();
#endregion
#region Repository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductPhotoRepository, ProductPhotoRepository>();
builder.Services.AddScoped<IHomeMainSliderRepository, HomeMainSliderRepository>();
builder.Services.AddScoped<IHomeMainSliderPhotoRepsository, HomeMainSliderPhotoRepository>();
builder.Services.AddScoped<IOurVisionRepository, OurVisionRepository>();
builder.Services.AddScoped<IOurVisionPhotoRepository, OurVisionPhotoRepository>();


#endregion
#region Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IHomeMainSliderService, HomeMainSliderService>();
builder.Services.AddScoped<IOurVisionService, OurVisionService>();

#endregion
#region App
var app = builder.Build();
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

#endregion
#region Route
app.MapControllerRoute(
    name:"areas",
     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
#endregion