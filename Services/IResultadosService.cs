
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IResultadosService {

    Task<int> InserirResultado(Resultado _resultado);

    Task<List<Resultado>> RetornarResultados();

    Task<Resultado> ValidarExistenciaResultado(Resultado _resultado);

    Task<int> CarregarDadosPlanilhaExcel(ImportExcel _importExcel);
    
}