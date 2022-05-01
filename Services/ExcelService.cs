
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

public class ExcelService : IExcelService
{
    
    
    public bool VerificarExistenciaDiretorio(string caminho)
    {
        return Directory.Exists(caminho);
    }
    public void CriarDiretorio(string caminho)
    {
        Directory.CreateDirectory(caminho);
    }

    public void EscreverArquivoDiretorio(string _caminho, ImportExcel _arquivo)
    {
        try
        {
            using (FileStream stream = new FileStream(Path.Combine(_caminho, _arquivo.file.FileName), FileMode.Create))
            {
                _arquivo.file.CopyTo(stream);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    public void ApagarArquivoDiretorio(string _caminho) {
        try{
            File.Delete(_caminho);
        }
        catch(Exception ex) {
            Util.LogErro(ex);
            throw ex;
        }
        
    }

    public MemoryStream EscreverArquivoMemoria(ImportExcel _arquivo)
    {
        var memoryStream = new MemoryStream();
        _arquivo.file.CopyTo(memoryStream);
        return memoryStream;
        /*var workBook = CreateWorkbook(parameters);
        workBook.Write(memoryStream);
        return File. (memoryStream, "attachment;filename=myfile.xls", "myfile.xls");*/
        

        

    }

    

    public List<Resultado> PegarDadosExcel(string _caminho) {
        string excelConnectionString = @"Provider='Microsoft.ACE.OLEDB.12.0';Data Source='" + _caminho + "';Extended Properties='Excel 12.0 Xml;IMEX=1'";
        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
  
        //Sheet Name
        excelConnection.Open();
        string tableName = excelConnection.GetSchema("Tables").Rows[0]["TABLE_NAME"].ToString();
        excelConnection.Close();
        //End
  
        OleDbCommand cmd = new OleDbCommand("Select * from [" + tableName + "]", excelConnection);
        excelConnection.Open();

        OleDbDataAdapter ad = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        
        ad.SelectCommand = cmd;
        ad.Fill(dt);
        excelConnection.Close();

        return Util.DataTableToList<Resultado>(dt);


    }
}