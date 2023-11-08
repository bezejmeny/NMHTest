namespace WebApplication1.Messaging
{
    public class Message
    {
        public Message()
        {
        }

        public Message(double inputValue, double? previousValue, double computedValue)
        {
            input_value = inputValue;
            previous_value = previousValue;
            computed_value = computedValue;

        }
        public double computed_value { get; set; }
        public double input_value { get; set; }
        public double? previous_value { get; set; }
    }
}