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

    public async Task<List<Resultado>> RetornarResultados()
    {
        try{
            return await _resultadoContext.Resultados.ToListAsync();
        }
        catch(Exception ex) {
            return null;
        }
        
    }

    public async Task<Resultado> ValidarExistenciaResultado(Resultado _resultado)
    {
        return await _resultadoContext.Resultados.Where(r => r.NumeroSorteio == _resultado.NumeroSorteio).SingleOrDefaultAsync();
    }

    public async Task<int> CarregarDadosPlanilhaExcel(ImportExcel _importExcel) 
    {

        string path = @"E:\Upload";
        
        if (!_excelService.VerificarExistenciaDiretorio(path)){
            _excelService.CriarDiretorio(path);
        }

        _excelService.EscreverArquivoDiretorio(path, _importExcel);

        
        var listaResultados =  _excelService.PegarDadosExcel(Path.Combine(path, _importExcel.file.FileName));
        
        _resultadoContext.Resultados.AddRange(listaResultados);
        return await _resultadoContext.SaveChangesAsync();
        
        

    }


}