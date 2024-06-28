using SiteLanches.Models;

namespace SiteLanches.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        void CriarPedido(Pedido pedido);
        Pedido GetPedidoById(int pedidoId);
        List<Pedido> GetPedidos();
    }
}
