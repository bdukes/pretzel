using System.Linq;

namespace Pretzel.Logic.Templating.Iis
{
    public class IisDirectory
    {
        public IisDirectory(int port, string host, string virtualPath, string physicalPath)
        {
            this.Port = port;
            this.Host = host;
            this.VirtualPath = virtualPath;
            this.PhysicalPath = physicalPath;
        }

        public int Port { get; private set; }
        public string Host { get; private set; }
        public string VirtualPath { get; private set; }
        public string PhysicalPath { get; private set; }
    }
}