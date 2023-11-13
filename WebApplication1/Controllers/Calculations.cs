using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Messaging;
using WebApplication1.Model.Calculations;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Home : ControllerBase
    {
        private IMessenger _messenger;
        private IGlobalKeyValueStorage _storage;
        const int DEFAULT_VALUE = 2;
        const int MAX_LIFE_SECONDS = 15;

        public Home(IMessenger messenger, IGlobalKeyValueStorage storage)
        {
            _messenger = messenger;
            _storage = storage;
        }

        [HttpPost(Name = "Calculation")]
        public IActionResult Calculation([FromBody] PostValue value, [Required(ErrorMessage = "Non null key has to be provided")]int key)
        {
            if(!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new JsonResult(new { Errors = ModelState.Values.SelectMany(x => x.Errors).Select(y => y.ErrorMessage) });
            }
            var calculationOutput = HandleIncomingData(key, value.Input);
            _messenger.SendAsJson(new Message(value.Input, calculationOutput.Item2, calculationOutput.Item1));
            return Ok();
        }

        private Tuple<double, double?> HandleIncomingData(int key, double input)
        {
            var computedValue = ComputeValue(key, input);
            var previousValue = AddOrUpdateStorage(key, computedValue);
            return new Tuple<double, double?>(computedValue, previousValue);
        }

        private double? AddOrUpdateStorage(int key, double value)
        {
            double? previousValue;
            if (!_storage.ContainsKey(key))
            {
                previousValue = null;
                _storage.Add(key, new ComplexValue() { Value = value, TimeStamp = DateTime.Now });
            }
            else
            {
                previousValue = _storage.Get(key).Value;
                _storage.Update(key, new ComplexValue() { Value = value, TimeStamp = DateTime.Now });
            }
            return previousValue;
        }

        private double ComputeValue(int key, double input)
        {
            if (!_storage.ContainsKey(key))
            {
                return DEFAULT_VALUE;
            }
            else if (_storage.ContainsKey(key) && _storage.Get(key).TimeStamp < DateTime.Now.AddSeconds(-MAX_LIFE_SECONDS))
            {
                return DEFAULT_VALUE;
            }
            else
            {
                var previousValue = _storage.Get(key).Value;
                double computedValue = input / previousValue;
                computedValue = Math.Log(computedValue);
                computedValue = Math.Cbrt(computedValue);
                return computedValue;
            }
        }
    }
}
