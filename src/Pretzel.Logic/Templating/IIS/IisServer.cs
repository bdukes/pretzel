using System;
using System.Linq;

using Microsoft.Web.Administration;

namespace Pretzel.Logic.Templating.Iis
{
    public sealed class IisServer : IDisposable
    {
        private readonly ServerManager serverManager = new ServerManager(true, null);

        public IisDirectory GetVirtualDirectory(string directory)
        {
            // TODO: IIS 6 compatibility (perhaps from http://www.codeproject.com/Articles/31293/Programmatically-Manage-IIS)
            // TODO: UAC elevation/don't require elevated privileges (perhaps from http://msdn.microsoft.com/en-us/library/aa970890.aspx)
            // TODO: expand environment variables in PhysicalPath
            // TODO: find deepest virtual directory (i.e. prefer C:\sites\MySite over C:\sites)
            return (from s in this.serverManager.Sites
                    from a in s.Applications
                    from v in a.VirtualDirectories
                    where directory.StartsWith(v.PhysicalPath, StringComparison.OrdinalIgnoreCase)
                    let binding = s.Bindings.First() // TODO: pick a local binding
                    select new IisDirectory(binding.EndPoint.Port, binding.Host, v.Path, v.PhysicalPath))
                .FirstOrDefault();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.serverManager.Dispose();
        }
    }
}