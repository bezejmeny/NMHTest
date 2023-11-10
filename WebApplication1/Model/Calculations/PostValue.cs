using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model.Calculations
{
    public class PostValue
    {
        [Required(ErrorMessage = "Input parameter can not be null. Set specific value")]
        public double Input { get; set; }
    }
}
