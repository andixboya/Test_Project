using System.ComponentModel.DataAnnotations;

namespace Stopify.Web.InputModels
{
    public class ProductTypeCreateInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
