using Microsoft.Extensions.Configuration;

namespace NetCore.Services
{
    public interface IGreeter
    {
        string GetGreeter();
    }

    public class Greeter : IGreeter
    {
        private string _greeter;

        public Greeter(IConfiguration configuration)
        {
            _greeter = configuration["greeting"];
        }

        public string GetGreeter()
        {
            return _greeter;
        }
    }
}