using Softjourn.SJCoins.Core.Resources;

namespace Softjourn.SJCoins.Core.Common.Utils
{
    public static class NetworkErrorUtils
    {
        public static string GetErrorMessage(int code)
        {
            switch (code)
            {
                case 400:
                    return ServerErrorResources.server_error_400;
                case 401:
                    return ServerErrorResources.server_error_401;
                case 404:
                    return ServerErrorResources.server_error_404;
                case 40401:
                    return ServerErrorResources.server_error_40401;
                case 40402:
                    return ServerErrorResources.server_error_40402;
                case 40403:
                    return ServerErrorResources.server_error_40403;
                case 40404:
                    return ServerErrorResources.server_error_40404;
                case 40405:
                    return ServerErrorResources.server_error_40405;
                case 40406:
                    return ServerErrorResources.server_error_40406;
                case 40407:
                    return ServerErrorResources.server_error_40407;
                case 40408:
                    return ServerErrorResources.server_error_40408;
                case 40409:
                    return ServerErrorResources.server_error_40409;
                case 40410:
                    return ServerErrorResources.server_error_40410;
                case 409:
                    return ServerErrorResources.server_error_409;
                case 40901:
                    return ServerErrorResources.server_error_40901;
                case 40902:
                    return ServerErrorResources.server_error_40902;
                case 40903:
                    return ServerErrorResources.server_error_40903;
                case 40906:
                    return ServerErrorResources.server_error_40906;
                case 509:
                    return ServerErrorResources.server_error_509;
                case 50901:
                    return ServerErrorResources.server_error_50901;
                default:
                    return ServerErrorResources.server_error_other;
            }
        }
    }
}
