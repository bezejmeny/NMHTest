using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model.Calculations
{
    public class QueryValue
    {
        [Required(ErrorMessage = "Non null key has to be provided")]
        public int Key { get; set; }
    }
}
