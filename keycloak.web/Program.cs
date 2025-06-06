using keycloak.web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Keys")))
    .SetDefaultKeyLifetime(TimeSpan.FromDays(10));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme, opts =>
    {
        opts.LoginPath = "/Auth/SignIn";
        opts.ExpireTimeSpan = TimeSpan.FromDays(60);
        opts.SlidingExpiration = true;
        opts.Cookie.Name = "webCookie";
        opts.AccessDeniedPath = "/AccessDenied";
    });


builder.Services.AddAuthorization(x => { x.AddPolicy("CityIstanbul", y => { y.RequireClaim("city", "istanbul"); }); });

//Authorization Code Grant Type Example
//builder.Services.AddAuthentication(options =>
//    {
//        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
//    }).AddCookie(opts => { opts.AccessDeniedPath = "/Home/AccessDenied"; })
//    .AddOpenIdConnect(opts =>
//    {
//        opts.RequireHttpsMetadata = false;

//        opts.Authority = "http://localhost:8080/realms/MyCompany";
//        opts.ClientId = builder.Configuration.GetSection("IdentityOption")["ClientId"]!;
//        opts.ClientSecret = builder.Configuration.GetSection("IdentityOption")["ClientSecret"];
//        opts.ResponseType = "code";

//        opts.GetClaimsFromUserInfoEndpoint = true;
//        opts.SaveTokens = true;
//        opts.Scope.Add("profile email address phone roles");


//        //opts.TokenValidationParameters = new TokenValidationParameters
//        //{
//        //    NameClaimType = "preferred_username",
//        //    RoleClaimType = "roles"
//        //};


//        //opts.Events.OnRedirectToIdentityProvider += context =>
//        //{
//        //    context.Options.Authority = "http://localhost:8080/realms/ExampleTenant";

//        //    return Task.CompletedTask;
//        //};
//    });


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();