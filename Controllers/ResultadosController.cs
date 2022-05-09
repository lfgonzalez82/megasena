
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class ResultadosController : Controller
{
    private readonly IExcelService _excelService;
    private readonly IResultadosService _resultadoService;
    private readonly IDezenaSorteioService _dezenaSorteioService;

    public ResultadosController(IExcelService excelService, 
                                IResultadosService resultadosService,
                                IDezenaSorteioService dezenaSorteioService)
    {
        _excelService = excelService;
        _resultadoService = resultadosService;
        _dezenaSorteioService = dezenaSorteioService;
    }


    public IActionResult Index(int? pageNumber)
    {
        int pageSize = 20;
        return View(_resultadoService.RetornarResultados(pageNumber, pageSize).Result);
        
        
    }

    public IActionResult Inserir()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InserirResultados(Resultado _resultado)
    {
        if (ModelState.IsValid)
        {

            var resultado = await _resultadoService.ValidarExistenciaResultado(_resultado);

            if (resultado == null)
            {
                await _resultadoService.InserirResultado(_resultado);
            }
        }
        return RedirectToAction("Index");
    }

    public ActionResult CarregarDadosPlanilha()
    {
        return View();
    }

    [HttpPost]
    public ActionResult CarregarDadosPlanilha(ImportExcel  importExcel)
    {
        _resultadoService.CarregarDadosPlanilhaExcel(importExcel);

        ViewBag.Result = "Successfully Imported";
        return RedirectToAction("Index");
    }

    public IActionResult GerarDezenasPorSorteio() 
    {
        _dezenaSorteioService.GerarDezenasPorSorteio();
        return RedirectToAction("Index");
    }

    public IActionResult ListaDezenasAgrupadas()
    {
        return View(_dezenaSorteioService.ListarDezenasAgrupadas().Result);
    }

    public IActionResult GerarResultadoExcel() 
    {
        _dezenaSorteioService.GerarResultadoExcel();
        return RedirectToAction("ListarDezenasAgrupadas");
    }

    

}