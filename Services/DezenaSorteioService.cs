using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class DezenaSorteioService : IDezenaSorteioService
{
    private readonly IExcelService _excelService;
    private readonly ResultadoContext _resultadoContext;

    public DezenaSorteioService(IExcelService excelService,
                                ResultadoContext resultadoContext)
    {
        _excelService = excelService;
        _resultadoContext = resultadoContext;
    }

    public async Task<bool> GerarDezenasPorSorteio()
    {

        var listaResultadosNumero1 = await RetornaSorteioDezena1(1);
        if (listaResultadosNumero1 != null)
        {
            var listaDezenaSorteio = new List<DezenaSorteio>();
            listaDezenaSorteio = GerarListaDezenaSorteio(listaResultadosNumero1, 1);
            await InserirDezenasPorSorteio(listaDezenaSorteio);
            listaResultadosNumero1 = null;

        }

        for (var numero1 = 2; numero1 <= 55; numero1++)
        {
            var listaResultadosDezena1 = await RetornaSorteioDezena1(numero1);
            if (listaResultadosDezena1 != null)
            {

                var listaDezenaSorteio = new List<DezenaSorteio>();
                listaDezenaSorteio = GerarListaDezenaSorteio(listaResultadosDezena1, numero1);
                await InserirDezenasPorSorteio(listaDezenaSorteio);
            }


        }

        for (var numero2 = 2; numero2 <= 56; numero2++)
        {
            var listaResultadosDezena2 = await RetornaSorteioDezena2(numero2);
            if (listaResultadosDezena2 != null)
            {
                var listaDezenaSorteio = new List<DezenaSorteio>();
                listaDezenaSorteio = GerarListaDezenaSorteio(listaResultadosDezena2, numero2);
                await InserirDezenasPorSorteio(listaDezenaSorteio);
            }


        }

        for (var numero3 = 3; numero3 <= 57; numero3++)
        {
            var listaResultadosDezena3 = await RetornaSorteioDezena3(numero3);
            if (listaResultadosDezena3 != null)
            {
                var listaDezenaSorteio = new List<DezenaSorteio>();
                listaDezenaSorteio = GerarListaDezenaSorteio(listaResultadosDezena3, numero3);
                await InserirDezenasPorSorteio(listaDezenaSorteio);
            }


        }

        for (var numero4 = 4; numero4 <= 58; numero4++)
        {
            var listaResultadosDezena4 = await RetornaSorteioDezena4(numero4);
            if (listaResultadosDezena4 != null)
            {
                var listaDezenaSorteio = GerarListaDezenaSorteio(listaResultadosDezena4, numero4);
                await InserirDezenasPorSorteio(listaDezenaSorteio);
            }


        }

        for (var numero5 = 5; numero5 <= 59; numero5++)
        {
            var listaResultadosDezena5 = await RetornaSorteioDezena5(numero5);
            if (listaResultadosDezena5 != null)
            {
                var listaDezenaSorteio = new List<DezenaSorteio>();
                listaDezenaSorteio = GerarListaDezenaSorteio(listaResultadosDezena5, numero5);
                await InserirDezenasPorSorteio(listaDezenaSorteio);
            }

        }

        for (var numero6 = 6; numero6 <= 60; numero6++)
        {
            var listaResultadosDezena6 = await RetornaSorteioDezena6(numero6);
            if (listaResultadosDezena6 != null)
            {
                var listaDezenaSorteio = new List<DezenaSorteio>();
                listaDezenaSorteio = GerarListaDezenaSorteio(listaResultadosDezena6, numero6);
                await InserirDezenasPorSorteio(listaDezenaSorteio);
            }

        }

        return true;
    }

    public List<DezenaSorteio> GerarListaDezenaSorteio(List<int> _listaResultado, int _dezenaProcurada)
    {
        var listaDezenaSorteio = new List<DezenaSorteio>();
        foreach (var item in _listaResultado)
        {
            var dezenaSorteio = new DezenaSorteio();
            dezenaSorteio.NumeroSorteio = item;
            dezenaSorteio.Dezena = _dezenaProcurada;
            listaDezenaSorteio.Add(dezenaSorteio);


        }

        return listaDezenaSorteio;
    }

    public async Task GerarResultadoExcel()
    {
        try
        {
            var listaAgrupada = await ListarDezenasAgrupadas();

            var workBook = _excelService.CriarWorkbook("MegaSena");
            foreach (var item in listaAgrupada)
            {
                var linhaPlanilha = 1;
                var difSorteio = 0;
                var sorteioAnterior = 0;
                var planilha = _excelService.CriarWoorksheet(workBook, item.Key.ToString());
                _excelService.PreencherTextoPlanilha(planilha, linhaPlanilha, 1, "Dezena");
                _excelService.PreencherTextoPlanilha(planilha, linhaPlanilha, 2, "Numero Sorteio");
                _excelService.PreencherTextoPlanilha(planilha, linhaPlanilha, 3, "Dif. Sorteio Anterior");


                foreach (var itemInterno in item)
                {
                    linhaPlanilha++;
                    _excelService.PreencherTextoPlanilha(planilha, linhaPlanilha, 1, itemInterno.Dezena.ToString());
                    _excelService.PreencherTextoPlanilha(planilha, linhaPlanilha, 2, itemInterno.NumeroSorteio.ToString());
                    difSorteio = itemInterno.NumeroSorteio - sorteioAnterior;
                    sorteioAnterior = itemInterno.NumeroSorteio;
                    _excelService.PreencherTextoPlanilha(planilha, linhaPlanilha, 3, difSorteio.ToString());
                }
            
            }
            _excelService.SalvarArquivo(workBook);
        }
        catch(Exception ex)
        {
            Util.LogErro(ex);
            throw(ex);
        }





    }

    public async Task<int> InserirDezenasPorSorteio(List<DezenaSorteio> _listaDezenaSorteio)
    {
        try
        {

            await _resultadoContext.DezenasSorteio.AddRangeAsync(_listaDezenaSorteio);
            return await _resultadoContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Util.LogErro(ex);
            throw ex;
        }
    }

    public async Task<List<IGrouping<int, DezenaSorteio>>> ListarDezenasAgrupadas()
    {
        List<DezenaSorteio> listaDezenaSorteio = await PegarDezenasPorSorteio();
        listaDezenaSorteio.OrderBy(lds => lds.NumeroSorteio);
        var dezenaGrupo = from dezena in listaDezenaSorteio
                          group dezena by dezena.Dezena into grupoDezena
                          select grupoDezena
                          ;
        return dezenaGrupo.ToList();
    }

    public async Task<List<DezenaSorteio>> PegarDezenasPorSorteio()
    {
        return await _resultadoContext.DezenasSorteio.OrderBy(r => r.Dezena).ThenBy(r => r.NumeroSorteio).ToListAsync();
    }

    public async Task<DezenaSorteio> VerificaSorteioCadastrado(int _numeroSorteio)
    {
        return await _resultadoContext.DezenasSorteio.Where(ds => ds.NumeroSorteio == _numeroSorteio).SingleOrDefaultAsync();
    }

    public async Task<List<int>> RetornaSorteioDezena1(int _dezenaProcura)
    {

        return await _resultadoContext.Resultados.Where(r => r.Dezena1 == _dezenaProcura).Select(r => r.NumeroSorteio).ToListAsync();
    }

    public async Task<List<int>> RetornaSorteioDezena2(int _dezenaProcura)
    {
        return await _resultadoContext.Resultados.Where(r => r.Dezena2 == _dezenaProcura).Select(r => r.NumeroSorteio).ToListAsync();
    }

    public async Task<List<int>> RetornaSorteioDezena3(int _dezenaProcura)
    {
        return await _resultadoContext.Resultados.Where(r => r.Dezena3 == _dezenaProcura).Select(r => r.NumeroSorteio).ToListAsync();
    }

    public async Task<List<int>> RetornaSorteioDezena4(int _dezenaProcura)
    {
        return await _resultadoContext.Resultados.Where(r => r.Dezena4 == _dezenaProcura).Select(r => r.NumeroSorteio).ToListAsync();
    }

    public async Task<List<int>> RetornaSorteioDezena5(int _dezenaProcura)
    {
        return await _resultadoContext.Resultados.Where(r => r.Dezena5 == _dezenaProcura).Select(r => r.NumeroSorteio).ToListAsync();
    }

    public async Task<List<int>> RetornaSorteioDezena6(int _dezenaProcura)
    {
        return await _resultadoContext.Resultados.Where(r => r.Dezena6 == _dezenaProcura).Select(r => r.NumeroSorteio).ToListAsync();
    }


}