using SiteLanches.Models;

namespace SiteLanches.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria>Categorias { get; }
    }
}
