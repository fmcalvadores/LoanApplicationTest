using Microsoft.Extensions.Logging;

namespace Loan.Application.Services
{
    public class BaseService
    {
        protected readonly ILogger _logger;
        public BaseService(ILogger logger)
        {
            _logger = logger;
        }

    }
}
