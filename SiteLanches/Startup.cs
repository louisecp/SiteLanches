using SiteLanches.Context;
using Microsoft.EntityFrameworkCore;
using SiteLanches.Repositories.Interfaces;
using SiteLanches.Repositories;
using SiteLanches.Models;
using Microsoft.AspNetCore.Builder;

namespace SiteLanches;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        //registrando o Context como um serviço
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //toda vez que uma instância for solicitada referenciando a Interface o conteiner nativo da injeção de 
        //dependencia vai criar uma instancia da classe e vai injetar no construtor onde estiver sendo solicitado 
        services.AddTransient<ILancheRepository,LancheRepository>();
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddTransient<IPedidoRepository,PedidoRepository>();
        //definir um serviço para poder acessar os recursos do HTTPContext
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //o AddScoped permite criar uma instancia do serviço a cada request, ou seja, se dois usúarios solicitarem o objeto carrinho ao mesmo tempo eles obterão estancias diferentes.
        services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));
        services.AddControllersWithViews();

        //Registrando os middlewares
        services.AddMemoryCache();
        services.AddSession();

        
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        //ativação do Session
        app.UseSession();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {

            endpoints.MapControllerRoute(
                name: "categoriaFiltro",
                pattern: "Lanche/{action}/{categoria?}",
                defaults: new {controller = "Lanche", action = "List"});

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
