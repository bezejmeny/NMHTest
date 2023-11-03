namespace WebApplication1
{
    public class CalculationOutput
    {
        public CalculationOutput(decimal computedValue, decimal? previousValue) { 
            ComputedValue = computedValue;
            PreviousValue = previousValue;
        }

        public decimal ComputedValue { get; private set; }
        public decimal? PreviousValue { get; private set; }
    }
}
