using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ResultadosService : IResultadosService
{
    private readonly IExcelService _excelService;
    private readonly ResultadoContext _resultadoContext;

    public ResultadosService(IExcelService excelService, ResultadoContext resultadoContext)
    {
        _excelService = excelService;
        _resultadoContext = resultadoContext;

    }

    public async Task<int> InserirResultado(Resultado _resultado)
    {
        _resultadoContext.Resultados.Add(_resultado);
        return await _resultadoContext.SaveChangesAsync();
        
    }

    public async Task<List<Resultado>> RetornarResultados(int? pageNumber,int pageSize)
    {
        try{

            return await PaginatedList<Resultado>.CreateAsync(_resultadoContext.Resultados,pageNumber ?? 1, pageSize);
            //return await _resultadoContext.Resultados.ToListAsync();
        }
        catch(Exception ex)
        { 
            Util.LogErro(ex);
            throw;
        }
        
    }

    public async Task<Resultado> ValidarExistenciaResultado(Resultado _resultado)
    {
        return await _resultadoContext.Resultados.Where(r => r.NumeroSorteio == _resultado.NumeroSorteio).SingleOrDefaultAsync();
    }

    public async Task<bool> CarregarDadosPlanilhaExcel(ImportExcel _importExcel) 
    {

        string path = Path.GetTempPath();
        
        if (!_excelService.VerificarExistenciaDiretorio(path)){
            _excelService.CriarDiretorio(path);
        }

        _excelService.EscreverArquivoDiretorio(path, _importExcel);

        
        var listaResultadosCadastrados = _resultadoContext.Resultados.ToList();
        var listaResultadosExcel =  _excelService.PegarDadosExcel(Path.Combine(path, _importExcel.file.FileName));

        var listaCadastrar = new List<Resultado>();
        foreach (var itemListaExcel in listaResultadosExcel) {
            
            if (listaResultadosCadastrados.Where(lrc => lrc.NumeroSorteio == itemListaExcel.NumeroSorteio).Count() == 0)
            {
                listaCadastrar.Add(itemListaExcel);
            }

        }

        
        await _resultadoContext.AddRangeAsync(listaCadastrar);
        await _resultadoContext.SaveChangesAsync();


        
        _excelService.ApagarArquivoDiretorio(Path.Combine(path,_importExcel.file.FileName));
        return true;
        

    }


}