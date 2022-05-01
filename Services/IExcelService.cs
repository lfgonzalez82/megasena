using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Http;

public interface IExcelService 
{
    bool VerificarExistenciaDiretorio(string _caminho);
    void CriarDiretorio(string _caminho);
    void EscreverArquivoDiretorio(string _caminho, ImportExcel _arquivo);

    MemoryStream EscreverArquivoMemoria(ImportExcel _arquivo);

    List<Resultado> PegarDadosExcel(string _caminho);



}