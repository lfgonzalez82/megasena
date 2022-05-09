using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ResultadosService : IResultadosService
{
    private readonly IExcelService _excelService;
    private readonly IDezenaSorteioService _dezenaSorteioService;
    private readonly ResultadoContext _resultadoContext;

    public ResultadosService(IExcelService excelService,
                             IDezenaSorteioService dezenaSorteioService,
                             ResultadoContext resultadoContext)
    {
        _excelService = excelService;
        _dezenaSorteioService = dezenaSorteioService;
        _resultadoContext = resultadoContext;

    }

    public async Task<int> InserirResultado(Resultado _resultado)
    {
        _resultadoContext.Resultados.Add(_resultado);
        await _resultadoContext.SaveChangesAsync();

        var listaDezenaSorteio = new List<DezenaSorteio>();
        var itemDezenaSorteio = await _dezenaSorteioService.VerificaSorteioCadastrado(_resultado.NumeroSorteio);
        if (itemDezenaSorteio == null)
        {
            listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = _resultado.NumeroSorteio, Dezena = _resultado.Dezena1 });
            listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = _resultado.NumeroSorteio, Dezena = _resultado.Dezena2 });
            listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = _resultado.NumeroSorteio, Dezena = _resultado.Dezena3 });
            listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = _resultado.NumeroSorteio, Dezena = _resultado.Dezena4 });
            listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = _resultado.NumeroSorteio, Dezena = _resultado.Dezena5 });
            listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = _resultado.NumeroSorteio, Dezena = _resultado.Dezena6 });
        }
        await _dezenaSorteioService.InserirDezenasPorSorteio(listaDezenaSorteio);

        return 1;

    }

    public async Task<List<Resultado>> RetornarResultados(int? pageNumber, int pageSize)
    {
        try
        {

            return await PaginatedList<Resultado>.CreateAsync(_resultadoContext.Resultados, pageNumber ?? 1, pageSize);
            //return await _resultadoContext.Resultados.ToListAsync();
        }
        catch (Exception ex)
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

        if (!_excelService.VerificarExistenciaDiretorio(path))
        {
            _excelService.CriarDiretorio(path);
        }

        _excelService.EscreverArquivoDiretorio(path, _importExcel);


        var listaResultadosCadastrados = _resultadoContext.Resultados.ToList();
        var listaResultadosExcel = _excelService.PegarDadosExcel(Path.Combine(path, _importExcel.file.FileName));

        var listaCadastrar = new List<Resultado>();
        var listaDezenaSorteio = new List<DezenaSorteio>();
        foreach (var itemListaExcel in listaResultadosExcel)
        {

            if (listaResultadosCadastrados.Where(lrc => lrc.NumeroSorteio == itemListaExcel.NumeroSorteio).Count() == 0)
            {
                var itemDezenaSorteio = await _dezenaSorteioService.VerificaSorteioCadastrado(itemListaExcel.NumeroSorteio);
                if (itemDezenaSorteio == null)
                {
                    listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = itemListaExcel.NumeroSorteio, Dezena = itemListaExcel.Dezena1 });
                    listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = itemListaExcel.NumeroSorteio, Dezena = itemListaExcel.Dezena2 });
                    listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = itemListaExcel.NumeroSorteio, Dezena = itemListaExcel.Dezena3 });
                    listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = itemListaExcel.NumeroSorteio, Dezena = itemListaExcel.Dezena4 });
                    listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = itemListaExcel.NumeroSorteio, Dezena = itemListaExcel.Dezena5 });
                    listaDezenaSorteio.Add(new DezenaSorteio { NumeroSorteio = itemListaExcel.NumeroSorteio, Dezena = itemListaExcel.Dezena6 });
                }
                listaCadastrar.Add(itemListaExcel);
            }

        }


        await _resultadoContext.Resultados.AddRangeAsync(listaCadastrar);
        await _resultadoContext.SaveChangesAsync();

        await _dezenaSorteioService.InserirDezenasPorSorteio(listaDezenaSorteio);




        _excelService.ApagarArquivoDiretorio(Path.Combine(path, _importExcel.file.FileName));
        return true;


    }

    
}