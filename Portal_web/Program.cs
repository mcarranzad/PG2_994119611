using Microsoft.EntityFrameworkCore;
using sgc.ml.Models;
using sgc.ml.Repository;
using sgc.ml.Repository.Interfaces;
using sgc.ml.Resources;
using sgc.ml.Services;
using sgc.ml.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var connectionString = builder.Configuration.GetConnectionString("Default");

// set sqlserver connection string
builder.Services.AddDbContextFactory<DataContext>(builder =>
    builder.UseMySQL(connectionString));

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IAdmClienteRepository, AdmClienteRepository>();
builder.Services.AddTransient<IAdmHistoryRepository, AdmHistoryRepository>();
builder.Services.AddTransient<IAdmUsuarioRepository, AdmUsuarioRepository>();
builder.Services.AddTransient<IAdmRolRepository, AdmRolRepository>();
builder.Services.AddTransient<IAdmPreguntaRepository, AdmPreguntaRepository>();

builder.Services.AddHttpClient<INeuronalService, NeuronalService>()
    .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Security}/{action=Login}/{id?}");

app.Run();