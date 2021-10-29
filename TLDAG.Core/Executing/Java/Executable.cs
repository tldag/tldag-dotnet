using System.IO;
using System.Linq;
using static System.IO.SearchOption;
using static TLDAG.Core.Exceptions.Errors;

namespace TLDAG.Core.Executing.Java
{
    public static class JavaExecutables
    {
        public static readonly string JavaExecutableName = Platform.IsWindows ? "java.exe" : "java";

        public static DirectoryInfo? GetSdkDirectory() => Env.GetDirectory("JAVA_HOME");
        public static DirectoryInfo? GetJreDirectory() => Env.GetDirectory("JRE_HOME");

        public static Executable Find()
        {
            if (TryFind(out Executable executable))
                return executable;

            throw FileNotFound(JavaExecutableName);
        }

        public static bool TryFind(out Executable executable)
        {
            FileInfo? file;

            file = GetSdkDirectory()?.EnumerateFiles(JavaExecutableName, AllDirectories).FirstOrDefault();
            file ??= GetJreDirectory()?.EnumerateFiles(JavaExecutableName, AllDirectories).FirstOrDefault();

            if (file is not null)
            {
                executable = new(file);
                return true;
            }

            return Executables.TryFind("java", out executable);
        }
    }
}
