using Microsoft.EntityFrameworkCore;
using SiteLanches.Context;

namespace SiteLanches.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext context)
        {
            _context = context;
        }
        public string CarrinhoCompraId {  get; set; }

        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider services) 
        {
            //define uma sessão
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            //obtem um serviço do tipo do nosso contexto
            var context = services.GetService<AppDbContext>();

            //obtem ou gera o Id do carrinho
            string carrinhoId = session.GetString("carrinhoId") ?? Guid.NewGuid().ToString();   

            //atribui o id do carrinho na Sessão
            session.SetString("CarrinhoId", carrinhoId);

            //retorna o carrinho com o contexto e o Id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };
        }

        public void AdicionarAoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                    s => s.Lanche.LancheId == lanche.LancheId &&
                    s.CarrinhoCompraId == CarrinhoCompraId);

            if( carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    CarrinhoCompraId = CarrinhoCompraId,
                    Lanche = lanche,
                    Quantidade = 1
                };
                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
                Console.WriteLine($"Adicionando novo item ao carrinho: {lanche.Name}, Quantidade: 1");
            }
            else
            {
                carrinhoCompraItem.Quantidade++;
                Console.WriteLine($"Incrementando quantidade do item: {lanche.Name}, Nova Quantidade: {carrinhoCompraItem.Quantidade}");
            }
            _context.SaveChanges();
            Console.WriteLine("Dados salvos no banco de dados.");
        }

        public int RemoverDoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId &&
                s.CarrinhoCompraId == CarrinhoCompraId);

            var quantidadeLocal = 0;

            if(carrinhoCompraItem != null )
            {
                if(carrinhoCompraItem.Quantidade > 1) 
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;
                }
                else
                {
                    _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
                }
            }
            _context.SaveChanges();
            return quantidadeLocal;
        }

        //public List<CarrinhoCompraItem> GetCarrinhoCompraItens()
        //{
        //    return CarrinhoCompraitens ??
        //        (CarrinhoCompraitens =
        //        _context.CarrinhoCompraItens
        //        .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
        //        .Include(s => s.Lanche)
        //        .ToList());
        //}

        public List<CarrinhoCompraItem> GetCarrinhoCompraItens()
        {
            if (CarrinhoCompraItens == null)
            {
                CarrinhoCompraItens = _context.CarrinhoCompraItens
                    .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                    .Include(s => s.Lanche)
                    .ToList();
            }
            return CarrinhoCompraItens;
        }

        public void LimparCarrinho()
        {
            var carrinhoItens = _context.CarrinhoCompraItens
                                .Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId);

            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
            _context.SaveChanges();
        }
        public decimal GetCarrinhoCompraTotal()
        {
            var total = _context.CarrinhoCompraItens
                        .Where(c=> c.CarrinhoCompraId==CarrinhoCompraId)
                        .Select(c => c.Lanche.Preco * c.Quantidade).Sum();

            return total;
        }
    }
}
