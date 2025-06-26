using System.IO;
using System.Reflection;

namespace Lines98.Utils
{
    class PathHelper
    {
        private static string _appDirCache;

        public static string GetAppDir()
        {
            return _appDirCache ??
                   (_appDirCache =
                       Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName));
        }

        public static string GetAppFile(string filename)
        {
            return GetAppDir() + "\\" + filename;
        }
    }
}
