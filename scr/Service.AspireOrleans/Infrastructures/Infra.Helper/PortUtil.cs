using System.Net;
using System.Net.Sockets;

namespace Infra.Helper
{
    public class PortUtil
    {
        public static int GetAvailablePort(IPAddress ip)
        {
            TcpListener l = new(ip, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();

            return port;
        }
    }
}
