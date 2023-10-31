namespace WebApplication1
{
    public class ComputationResponse
    {
        public ComputationResponse(int key, decimal? originalValue, decimal newValue)
        {
            input_value = key;
            previous_value = originalValue;
            computed_value = newValue;

        }
        public decimal computed_value { get; set; }
        public int input_value { get; set; }
        public decimal? previous_value { get; set; }
    }
}