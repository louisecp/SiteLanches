using SiteLanches.Context;
using Microsoft.EntityFrameworkCore;
using SiteLanches.Repositories.Interfaces;
using SiteLanches.Repositories;
using SiteLanches.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;

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

        services.AddIdentity<Microsoft.AspNetCore.Identity.IdentityUser, Microsoft.AspNetCore.Identity.IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        //Esta linha está configurando as opções de identidade para o serviço. 
        services.Configure<IdentityOptions>(options =>
        {
            //Default Password settings
            options.Password.RequireDigit = true;//Define que a senha deve conter pelo menos um dígito numérico.
            options.Password.RequireLowercase = true;//Define que a senha deve conter pelo menos uma letra minúscula.
            options.Password.RequireNonAlphanumeric = true;//Define que a senha deve conter pelo menos um caractere não alfanumérico (como @, #, $, etc.).
            options.Password.RequireUppercase = true; //Define que a senha deve conter pelo menos uma letra maiúscula.
            options.Password.RequiredLength = 8; //Define que a senha deve ter no mínimo 8 caracteres de comprimento.
            options.Password.RequiredUniqueChars = 1;//Define que a senha deve conter pelo menos 1 caractere único (não repetido).
        });

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

        app.UseAuthentication();
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
