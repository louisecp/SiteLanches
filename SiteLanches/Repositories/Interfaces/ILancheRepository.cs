using SiteLanches.Models;

namespace SiteLanches.Repositories.Interfaces
{
    public interface ILancheRepository
    {

        //Na interface não é preciso definir o modificador de acesso, porque por padrão os métodos e propriedades são públicos.
        IEnumerable<Lanche>Lanches { get; }

        IEnumerable<Lanche> LanchesPreferidos { get; }

        Lanche GetLancheById(int LancheId);

    }
}
