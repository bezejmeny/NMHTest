namespace WebApplication1.Messaging
{
    public class Message
    {
        public Message()
        {
        }

        public Message(decimal inputValue, decimal? previousValue, decimal computedValue)
        {
            input_value = inputValue;
            previous_value = previousValue;
            computed_value = computedValue;

        }
        public decimal computed_value { get; set; }
        public decimal input_value { get; set; }
        public decimal? previous_value { get; set; }
    }
}