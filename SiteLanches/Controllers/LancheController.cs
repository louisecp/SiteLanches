using Microsoft.AspNetCore.Mvc;
using SiteLanches.Models;
using SiteLanches.Repositories;
using SiteLanches.Repositories.Interfaces;
using SiteLanches.ViewModels;

namespace SiteLanches.Controllers
{
    public class LancheController : Controller
    {
        private ICategoriaRepository _categoriaRepository { get; }
        private readonly ILancheRepository _lancheRepository; //injetando uma instância, isso é permitido porque já referenciamos o serviço em Startup.

        public LancheController(ICategoriaRepository categoriaRepository, ILancheRepository lancheRepository)
        {
            //instância do repositório
            _lancheRepository = lancheRepository;
            _categoriaRepository = categoriaRepository;
        }

        public IActionResult List(string categoria)
        {
            IEnumerable<Lanche>lanches;
            //Uma string para armazenar o nome da categoria atual que está sendo visualizada.

            string categoriaAtual = string.Empty;
            //Se categoria é nula ou vazia, todos os lanches são selecionados e ordenados por LancheId.
            //categoriaAtual é definido como "Todos os lanches"
            if (string.IsNullOrEmpty(categoria))
            {
                lanches = _lancheRepository.Lanches.OrderBy(l  => l.LancheId);
                categoriaAtual = "Todos os lanches";
            }
            
            else
            {
                
                lanches = _lancheRepository.Lanches
                    .Where(l => l.Categoria.CategoriaName.Equals("categoria"))
                    .OrderBy(c => c.Name);

                categoriaAtual = categoria;
            }

            var lanchesListViewModel = new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };
            
            return View(lanchesListViewModel);
                  
        }
        public IActionResult Details(int LancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(l=> l.LancheId == LancheId);

            if (lanche == null)
            {
                return View("~/Views/Error/Error.cshtml");
            }

            return View(lanche);
        }

        public ViewResult Search(string searchString)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if(string.IsNullOrEmpty(searchString))
            {
                lanches = _lancheRepository.Lanches.OrderBy(p => p.LancheId);
                categoriaAtual = "Todos os Lanches";
            }

            else
            {
                lanches = _lancheRepository.Lanches
                    .Where(p => p.Name.ToLower().Contains(searchString.ToLower()));

                if (lanches.Any())
                    categoriaAtual = "Lanches";
                else
                    categoriaAtual = "Nenhum lanche foi encontrado";
            }

            return View("~/Views/Lanche/List.cshtml", new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            });
        }
    }
}
