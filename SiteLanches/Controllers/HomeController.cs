using Microsoft.AspNetCore.Mvc;
using SiteLanches.Models;
using SiteLanches.Repositories.Interfaces;
using SiteLanches.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace SiteLanches.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILancheRepository _lancheRepository;

        //Injeção do repositório de lanches
        public HomeController(ILancheRepository lancheRepository)
        {
            _lancheRepository = lancheRepository;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                //lista de lanches preferidos obtidos do Repositorio
                LanchesPreferidos = _lancheRepository.LanchesPreferidos
            };
            return View(model); //todo controlador retorna uma view por padrão
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}