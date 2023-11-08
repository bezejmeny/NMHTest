namespace WebApplication1
{
    public class CalculationOutput
    {
        public CalculationOutput(double computedValue, double? previousValue) { 
            ComputedValue = computedValue;
            PreviousValue = previousValue;
        }

        public double ComputedValue { get; private set; }
        public double? PreviousValue { get; private set; }
    }
}
