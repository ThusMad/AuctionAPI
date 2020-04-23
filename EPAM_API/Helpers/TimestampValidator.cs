using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPAM_API.Helpers
{
    public static class TimestampValidator
    {
        private const int DefaultWindow = 100;

        public static bool Validate(long timestamp, int? recvWindow)
        {
            var serverTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var window = recvWindow ?? DefaultWindow;

            return serverTime - timestamp < window;
        }
    }
}
