using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public interface IDezenaSorteioService{

    Task<bool> GerarDezenasPorSorteio();
    Task<List<DezenaSorteio>> PegarDezenasPorSorteio();

    Task<int> InserirDezenasPorSorteio(List<DezenaSorteio> _listaDezenaSorteio);

    List<DezenaSorteio> GerarListaDezenaSorteio(List<int> _listaResultado, int _dezenaProcurada);

    Task<List<IGrouping<int,DezenaSorteio>>> ListarDezenasAgrupadas();

    Task GerarResultadoExcel();

    Task<DezenaSorteio> VerificaSorteioCadastrado(int _numeroSorteio);

    Task<List<int>> RetornaSorteioDezena1(int _dezenaProcura);
    Task<List<int>> RetornaSorteioDezena2(int _dezenaProcura);
    Task<List<int>> RetornaSorteioDezena3(int _dezenaProcura);
    Task<List<int>> RetornaSorteioDezena4(int _dezenaProcura);
    Task<List<int>> RetornaSorteioDezena5(int _dezenaProcura);
    Task<List<int>> RetornaSorteioDezena6(int _dezenaProcura);

}