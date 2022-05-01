
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IResultadosService {

    Task<int> InserirResultado(Resultado _resultado);

    Task<List<Resultado>> RetornarResultados(int? pageNumber, int pageSize);

    Task<Resultado> ValidarExistenciaResultado(Resultado _resultado);

    Task<bool> CarregarDadosPlanilhaExcel(ImportExcel _importExcel);
    
}