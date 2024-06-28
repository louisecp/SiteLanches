using Microsoft.EntityFrameworkCore;
using SiteLanches.Context;
using SiteLanches.Models;
using SiteLanches.Repositories.Interfaces;

namespace SiteLanches.Repositories
{
    public class LancheRepository : ILancheRepository
    {
        private readonly AppDbContext _context;

        public LancheRepository(AppDbContext contexto)
        {
            _context = contexto;
        }
        //posso usar a instancia do contexto para acessar lanches e incluir as categorias.
        public IEnumerable<Lanche> Lanches => _context.Lanches.Include(c => c.Categoria);

        public IEnumerable<Lanche> LanchesPreferidos => _context.Lanches.Where( l=> l.IsLanchePreferido).Include(c => c.Categoria);

        public Lanche GetLancheById(int LancheId) => _context.Lanches.FirstOrDefault(l => l.LancheId == LancheId);
    }
}
