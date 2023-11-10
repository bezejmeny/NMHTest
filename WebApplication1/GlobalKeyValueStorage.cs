namespace WebApplication1
{
    public class ComplexValue
    {
        public double Value { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class GlobalKeyValueStorage : IGlobalKeyValueStorage
    {
        private readonly object _lock = new object();
        private Dictionary<int, ComplexValue> _storage;
        public GlobalKeyValueStorage()
        {
            _storage = new Dictionary<int, ComplexValue>();
        }

        public bool ContainsKey(int key)
        {
            lock (_lock)
            {
                return _storage.ContainsKey(key);
            }
        }

        public void Add(int key, ComplexValue value)
        {
            lock (_lock) {
                _storage.Add(key, value);
            }
        }

        public ComplexValue Get(int key)
        {
            lock (_lock)
            {
                return _storage[key];
            }
        }

        public void Update(int key, ComplexValue value) {
            lock (_lock)
            {
                _storage[key] = value;
            }
        }
    }
}
