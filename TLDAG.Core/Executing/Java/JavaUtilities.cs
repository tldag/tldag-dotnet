using System.IO;

namespace TLDAG.Core.Executing.Java
{
    public static class JavaUtilities
    {
        public static readonly string JavaExecutableName = Platform.IsWindows ? "java.exe" : "java";

        public static DirectoryInfo? GetSdkDirectory() => Env.GetDirectory("JAVA_HOME");
        public static DirectoryInfo? GetJreDirectory() => Env.GetDirectory("JRE_HOME");
    }
}
