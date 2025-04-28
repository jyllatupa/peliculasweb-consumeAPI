using Microsoft.AspNetCore.Authentication.Cookies;
using PeliculasWeb.Repositorio;
using PeliculasWeb.Repositorio.IRepositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configurar cliente HTTP para toda la aplicación, injeccion de dependencia y se utiliza para poder utilizar el cliente http de manera global
//registrar el servicio HttpClient en el contenedor de dependencias. es una clase que se utiliza para hacer solicitudes HTTP (por ejemplo, llamar a APIs externas)
builder.Services.AddHttpClient();

//Agregamos Autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.LoginPath = "/Home/Login";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.SlidingExpiration = true;
});

//Agregar los servicios como inyección de dependencias
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IAccountRepositorio, AccountRepositorio>();

//Se debe registrar esto para que el HttpContextAccesor que esta en el _Layout funcione
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Agregar Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

//agregado
app.UseAuthentication(); //Si se usa Autenticación

app.UseRouting();

app.UseAuthorization();
app.UseSession(); //Cuando se usa Sessions

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
