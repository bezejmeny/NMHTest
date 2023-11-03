namespace WebApplication1
{
    public class ComplexValue
    {
        public decimal Value { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public static class GlobalKeyValueStorage
    {
        public static Dictionary<int, ComplexValue> Storage { get; set; } = new Dictionary<int, ComplexValue>();
    }
}
