using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

public class FileExt : ValidationAttribute
{
    public string Allow;
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var arquivo = (IFormFile)value;
        if (arquivo != null)
        {
            //string extension = Path ((Path. )value).FileName.Split('.')[1];
            var extension = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (Allow.Contains(extension))
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
        else
            return ValidationResult.Success;
    }
}