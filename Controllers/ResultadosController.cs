
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class ResultadosController : Controller
{
    private readonly IExcelService _excelService;
    private readonly IResultadosService _resultadoService;
    public ResultadosController(IExcelService excelService, IResultadosService resultadosService)
    {
        _excelService = excelService;
        _resultadoService = resultadosService;
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

}