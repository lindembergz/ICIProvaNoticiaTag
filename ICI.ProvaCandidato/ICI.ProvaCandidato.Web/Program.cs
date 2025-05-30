using ICiProvaCandidato.Aplicacao.Interfaces;
using ICiProvaCandidato.Aplicacao.Servicos;
using ICiProvaCandidato.Dominio.Repositorios;
using ICiProvaCandidato.Dominio.UoW;
using ICiProvaCandidato.Infra.Dados;
using ICiProvaCandidato.Infra.Dados.Repositorios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContextPool<ContextoSistemaNoticiasDDD>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IRepositorioNoticia, RepositorioNoticia>();
builder.Services.AddScoped<IRepositorioTag, RepositorioTag>();
builder.Services.AddScoped<IServicoAplicacaoNoticia, ServicoAplicacaoNoticia>();
builder.Services.AddScoped<IServicoAplicacaoTag, ServicoAplicacaoTag>();


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
   {
       options.JsonSerializerOptions.PropertyNamingPolicy = null;
   });

builder.Services.AddResponseCompression();


var app = builder.Build();

app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Noticia}/{action=Index}/{id?}");
});

app.Run();