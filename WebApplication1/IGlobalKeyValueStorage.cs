namespace WebApplication1
{
    public interface IGlobalKeyValueStorage
    {
        bool ContainsKey(int key);
        void Add(int key, ComplexValue value);
        ComplexValue Get(int key);
        void Update(int key, ComplexValue value);
    }
}
