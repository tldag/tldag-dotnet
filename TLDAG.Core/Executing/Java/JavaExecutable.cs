using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.IO.SearchOption;
using static TLDAG.Core.Exceptions.Errors;
using static TLDAG.Core.Executing.Java.JavaUtilities;

namespace TLDAG.Core.Executing.Java
{
    public static class JavaExecutable
    {
        public static IEnumerable<Executable> FindAllJava() => JavaExecutables.Create();

        public static bool TryFindJava(out Executable executable)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            executable = FindAllJava().FirstOrDefault();
#pragma warning restore CS8601 // Possible null reference assignment.

            return executable is not null;
        }

        public static Executable FindJava()
        {
            if (TryFindJava(out Executable executable))
                return executable;

            throw FileNotFound(JavaExecutableName);
        }
    }

    public class JavaExecutables : IEnumerable<Executable>
    {
        public IEnumerator<Executable> GetEnumerator() => new JavaExecutablesEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => new JavaExecutablesEnumerator();

        public static JavaExecutables Create() => new();
    }

    public class JavaExecutablesEnumerator : IEnumerator<Executable>
    {
        private Executable? current = null;
        public Executable Current => Contract.State.NotNull(current, "");
        object IEnumerator.Current => Contract.State.NotNull(current, "");

        private int state = 0;
        private IEnumerator<Executable>? executables = null;

        public void Dispose() { GC.SuppressFinalize(this); state = 3; executables = null; }
        public void Reset() { state = 0; executables = null; }

        public bool MoveNext()
        {
            if (state == 3)
                return false;

            if (state == 0)
            {
                state = 1;

                if (MoveNextInDirectory(GetSdkDirectory()))
                    return true;
            }

            if (state == 1)
            {
                state = 2;

                if (MoveNextInDirectory(GetJreDirectory()))
                    return true;
            }

            if (state == 2)
            {
                if (executables is null)
                    executables = Executables.FindAllExecutables("java").GetEnumerator();

                if (executables.MoveNext())
                {
                    current = executables.Current;
                    return true;
                }

                executables = null;
                state = 3;
            }

            return false;
        }

        private bool MoveNextInDirectory(DirectoryInfo? directory)
        {
            if (directory is null) return false;

            FileInfo? file = directory.EnumerateFiles(JavaExecutableName, AllDirectories).FirstOrDefault();

            if (file is not null)
            {
                current = new(file);
                return true;
            }

            return false;
        }
    }
}
