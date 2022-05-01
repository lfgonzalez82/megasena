using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class ImportExcel
{
    [Required(ErrorMessage = "Please select file")]
    [FileExt(Allow = ".xls,.xlsx", ErrorMessage = "Only excel file")]
    public IFormFile file { get; set; }
}