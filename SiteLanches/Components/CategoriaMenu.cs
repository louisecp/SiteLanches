using Microsoft.AspNetCore.Mvc;
using SiteLanches.Repositories.Interfaces;

namespace SiteLanches.Components
{
    public class CategoriaMenu : ViewComponent
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaMenu(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categorias = _categoriaRepository.Categorias.OrderBy(c => c.CategoriaName);
            return View(categorias);
        }
    }
}
