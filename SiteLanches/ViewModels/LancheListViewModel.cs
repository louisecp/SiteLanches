using SiteLanches.Models;

namespace SiteLanches.ViewModels
{
    //definindo a viewModel
    public class LancheListViewModel
    {
        public IEnumerable<Lanche> Lanches { get; set; } //definindo uma propriedade para exibir uma lista de lanches

        public string CategoriaAtual { get; set; }
    }
}
