using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

public static class Util
{
    private static string caminhoExe = string.Empty;
    public static List<T> DataTableToList<T>(this DataTable dt)
    {
        string nomeItem = "";
        try
        {
            var lobj = Activator.CreateInstance<List<T>>();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow it in dt.Rows)
                {
                    if (it == null)
                    {
                        break;
                    }

                    T obj = Activator.CreateInstance<T>();
                    if (obj == null)
                    {
                        break;

                    }

                    var props = obj.GetType().GetProperties();



                    foreach (var item in props)
                    {
                        nomeItem = item.Name;

                        for (int i = 0; i < it.ItemArray.Count(); i++)
                        {
                            if (item.Name.ToUpper().Equals(dt.Columns[i].ColumnName.Replace("_", "").ToUpper()))
                            {
                                if (item.PropertyType.Name.ToUpper().Equals("STRING") && it.ItemArray[i].GetType().Name != "DBNull")
                                {
                                    item.SetValue(obj, it.ItemArray[i], null);
                                    break;
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("INT32") && it.ItemArray[i].GetType().Name != "DBNull")
                                {
                                    item.SetValue(obj, Convert.ToInt32(it.ItemArray[i], null));
                                    break;
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("DATETIME") && it.ItemArray[i].GetType().Name != "DBNull")
                                {
                                    DateTimeFormatInfo brDtfi = new CultureInfo("pt-BR", false).DateTimeFormat;
                                    item.SetValue(obj, Convert.ToDateTime(it.ItemArray[i], brDtfi));
                                    break;
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("LONG") && it.ItemArray[i].GetType().Name != "DBNull")
                                {
                                    item.SetValue(obj, (long)(it.ItemArray[i]));
                                    break;
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("INT64") && it.ItemArray[i].GetType().Name != "DBNull")
                                {
                                    item.SetValue(obj, Convert.ToInt64(it.ItemArray[i], null));
                                    break;
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("DECIMAL") && it.ItemArray[i].GetType().Name != "DBNull")
                                {
                                    item.SetValue(obj, (decimal)(it.ItemArray[i]));
                                    break;
                                }
                            }
                        }
                    }
                    lobj.Add(obj);
                }
            }
            return lobj;
        }
        catch (Exception)
        {

            throw new Exception("Erro na conversão do tipo do Item " + nomeItem);
        }

    }


    public static T DataTableToModel<T>(this DataTable dt) where T : new()
    {
        string nomeItem = "";
        try
        {
            var obj = Activator.CreateInstance<T>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow it in dt.Rows)
                {
                    if (it == null)
                    {
                        break;
                    }

                    if (obj == null)
                    {
                        break;

                    }

                    var props = obj.GetType().GetProperties();

                    foreach (var item in props)
                    {
                        nomeItem = item.Name;
                        for (int i = 0; i < it.ItemArray.Count(); i++)
                        {
                            if (item.Name.ToUpper().Equals(dt.Columns[i].ColumnName.Replace("_", "").ToUpper()))
                            {
                                if (item.PropertyType.Name.ToUpper().Equals("STRING") && it.ItemArray[i].GetType().Name != "DBNull")
                                {
                                    item.SetValue(obj, it.ItemArray[i], null);
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("INT"))
                                {
                                    item.SetValue(obj, Convert.ToInt32(it.ItemArray[i]), null);
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("DATETIME") && it.ItemArray[i].ToString() != "")
                                {
                                    item.SetValue(obj, Convert.ToDateTime(it.ItemArray[i]), null);
                                }
                                else if (item.PropertyType.Name.ToUpper().Contains("LONG"))
                                {
                                    item.SetValue(obj, (long)(it.ItemArray[i]), null);
                                }
                            }

                        }
                    }

                }
            }
            return obj;
        }
        catch (Exception)
        {
            Console.Write(nomeItem);
            throw;
        }

    }

    public static void LogErro(Exception ex)
    {
        try
        {
            var caminhoExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (String.IsNullOrEmpty(caminhoExe))
            {
                return;
            }
            string caminhoArquivo = Path.Combine(caminhoExe, "MegaSena.txt");
            if (!File.Exists(caminhoArquivo))
            {
                FileStream arquivo = File.Create(caminhoArquivo);
                arquivo.Close();
            }
            using (StreamWriter w = File.AppendText(caminhoArquivo))
            {
                AppendLog(ex, w);
            }
        }
        catch
        {
        }
    }

    private static void AppendLog(Exception _exception, TextWriter txtWriter)
    {
        try
        {
            string Mensagem = string.Format("{0}{0}=== {1} ==={0}{2}{0}{3}{0}{4}{0}{5}", Environment.NewLine, DateTime.Now, _exception.Message, _exception.Source, _exception.InnerException, _exception.StackTrace);
            txtWriter.Write("\r\nLog Entrada : ");
            txtWriter.WriteLine($"  :{Mensagem}");
            txtWriter.WriteLine("------------------------------------");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    
}