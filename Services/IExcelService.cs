using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

public interface IExcelService 
{
    bool VerificarExistenciaDiretorio(string _caminho);
    void CriarDiretorio(string _caminho);
    void EscreverArquivoDiretorio(string _caminho, ImportExcel _arquivo);

    void ApagarArquivoDiretorio(string _caminho);

    MemoryStream EscreverArquivoMemoria(ImportExcel _arquivo);

    List<Resultado> PegarDadosExcel(string _caminho);

    IXLWorkbook CriarWorkbook(string _nome);

    IXLWorksheet CriarWoorksheet(IXLWorkbook _workbook, string _nome);

    void PreencherTextoPlanilha(IXLWorksheet _planilha, int _linhaCelula, int _colunaCelula , string _valorCelula);

    void SalvarArquivo(IXLWorkbook _workbook);



}