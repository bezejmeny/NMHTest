using Microsoft.AspNetCore.Mvc;
using WebApplication1.Messaging;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Home : ControllerBase
    {
        private IMessenger _messenger;
        const int DEFAULT_VALUE = 2;
        const int MAX_LIFE_SECONDS = 15;

        public Home(IMessenger messenger)
        {
            _messenger = messenger;
        }

        [HttpPost(Name = "Calculation")]
        public void Calculation([FromBody] double input, int key)
        {
            var calculationOutput = HandleIncomingData(key, input);
            _messenger.SendAsJson(new Message(input, calculationOutput.PreviousValue, calculationOutput.ComputedValue));
        }

        private CalculationOutput HandleIncomingData(int key, double input)
        {
            var computedValue = ComputeValue(key, input);
            var previousValue = AddOrUpdateStorage(key, computedValue);
            return new CalculationOutput(computedValue, previousValue);
        }

        private double? AddOrUpdateStorage(int key, double value)
        {
            double? previousValue;
            if (!GlobalKeyValueStorage.Storage.ContainsKey(key))
            {
                previousValue = null;
                GlobalKeyValueStorage.Storage.Add(key, new ComplexValue() { Value = value, TimeStamp = DateTime.Now });
            }
            else
            {
                previousValue = GlobalKeyValueStorage.Storage[key].Value;
                GlobalKeyValueStorage.Storage[key] = new ComplexValue() { Value = value, TimeStamp = DateTime.Now };
            }
            return previousValue;
        }

        private double ComputeValue(int key, double input)
        {
            if (!GlobalKeyValueStorage.Storage.ContainsKey(key))
            {
                return DEFAULT_VALUE;
            }
            else if (GlobalKeyValueStorage.Storage.ContainsKey(key) && GlobalKeyValueStorage.Storage[key].TimeStamp < DateTime.Now.AddSeconds(-MAX_LIFE_SECONDS))
            {
                return DEFAULT_VALUE;
            }
            else
            {
                var previousValue = GlobalKeyValueStorage.Storage[key].Value;
                double computedValue = input / previousValue;
                computedValue = Math.Log(computedValue);
                computedValue = Math.Cbrt(computedValue);
                return computedValue;
            }
        }
    }
}
