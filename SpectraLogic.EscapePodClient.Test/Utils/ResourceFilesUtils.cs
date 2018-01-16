using System.IO;
using System.Reflection;

namespace SpectraLogic.EscapePodClient.Test.Utils
{
    internal static class ResourceFilesUtils
    {
        internal static string Read(string resource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var textStreamReader = new StreamReader(assembly.GetManifestResourceStream(resource));
            return textStreamReader.ReadToEnd();
        }
    }
}
