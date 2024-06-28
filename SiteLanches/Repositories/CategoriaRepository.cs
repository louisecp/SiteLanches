using SiteLanches.Context;
using SiteLanches.Models;
using SiteLanches.Repositories.Interfaces;

namespace SiteLanches.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        IEnumerable<Categoria> ICategoriaRepository.Categorias => _context.Categorias;
    }
}
