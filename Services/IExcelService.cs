using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IExcelService 
{
    bool VerificarExistenciaDiretorio(string _caminho);
    void CriarDiretorio(string _caminho);
    void EscreverArquivoDiretorio(string _caminho, ImportExcel _arquivo);

    void ApagarArquivoDiretorio(string _caminho);

    MemoryStream EscreverArquivoMemoria(ImportExcel _arquivo);

    List<Resultado> PegarDadosExcel(string _caminho);



}