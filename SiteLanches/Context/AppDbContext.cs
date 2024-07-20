using Microsoft.EntityFrameworkCore;
using SiteLanches.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SiteLanches.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Carrega as informações de configurações necessárias para configurar o DbContext
        }

        //Definição das propriedades DbSet vão definir quais classes quero mapear para quais tabelas

        //Mapear a classe categoria para uma tabela categorias
        public DbSet<Categoria> Categorias { get; set; }

        //Mapear a classe lanche para uma tabela Lanche
        public DbSet<Lanche> Lanches { get; set; }

        //Mapear a classe pedido para uma tabela Pedido
        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }
    }
}
