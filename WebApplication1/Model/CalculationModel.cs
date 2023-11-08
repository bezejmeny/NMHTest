using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class CalculationModel
    {
        [Required]
        public double? Input { get; set; }
    }
}
