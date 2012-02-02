using System.ComponentModel.Composition;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace Pretzel.Logic.Templating.Iis
{
    [PartCreationPolicy(CreationPolicy.Shared)]
    [SiteEngineInfo(Engine = "iis")]
    public class IisEngine : ISiteEngine
    {
        private SiteContext context;
        private IFileSystem fileSystem;

        public void Process()
        {
            var outputDirectory = Path.Combine(context.Folder, "_site");
            fileSystem.Directory.CreateDirectory(outputDirectory);

            IisDirectory virtualDirectory;
            using (var iisServer = new IisServer())
            {
                virtualDirectory = iisServer.GetVirtualDirectory(this.context.Folder);
            }

            var virtualPath = virtualDirectory.VirtualPath;
            foreach (var file in fileSystem.Directory.GetFiles(context.Folder, "*.*", SearchOption.AllDirectories))
            {
            var relativePath = file.Replace(context.Folder, "").TrimStart('\\');
            ////    if (relativePath.StartsWith("_")) continue;
            ////    if (relativePath.StartsWith(".")) continue;

            var outputFile = Path.Combine(outputDirectory, relativePath);

            var directory = Path.GetDirectoryName(outputFile);
            if (!fileSystem.Directory.Exists(directory))
                fileSystem.Directory.CreateDirectory(directory);

            ////var extension = Path.GetExtension(file);
            ////if (extension.IsImageFormat())
            ////{
            ////        fileSystem.File.Copy(file, outputFile, true);
            ////        continue;
            ////    }

            ////    var inputFile = fileSystem.File.ReadAllText(file);
            ////    if (!inputFile.StartsWith("---"))
            ////    {
            ////        fileSystem.File.WriteAllText(outputFile, inputFile);
            ////        continue;
            ////    }

            ////    // TODO: refine this step
            ////    // markdown file should not be treated differently
            ////    // output from markdown file should be sent to template pipeline
                
            ////    if (extension.IsMarkdownFile())
            ////    {
            ////        outputFile = outputFile.Replace(extension, ".html");

            ////        var pageContext = ProcessMarkdownPage(inputFile, outputFile, outputDirectory);

            ////        fileSystem.File.WriteAllText(pageContext.OutputPath, pageContext.Content);
            ////    }
            ////    else
            ////    {
            ////        RenderTemplate(inputFile.ExcludeHeader(), outputFile);
            ////    }
            }
        }

        public bool CanProcess(IFileSystem fileSystem, string directory)
        {
            using (var iisServer = new IisServer())
            {
                return iisServer.GetVirtualDirectory(directory) != null;
            }
        }

        public void Initialize(IFileSystem fileSystem, SiteContext context)
        {
            this.fileSystem = fileSystem;
            this.context = context;
        }
    }
}