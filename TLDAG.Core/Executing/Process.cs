using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TLDAG.Core.Executing
{
    public static class ProcessExtensions
    {
        public static async Task WaitExit(this Process process, CancellationToken cancellationToken = default)
        {
#if NET5_0_OR_GREATER
            await process.WaitForExitAsync(cancellationToken);
#else
            while (!process.HasExited)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(10);
            }
#endif
        }

        public static ProcessStartInfo SetArguments(this ProcessStartInfo startInfo, IEnumerable<string> arguments)
        {
#if NET5_0_OR_GREATER
            startInfo.ArgumentList.Clear();
            foreach (string argument in arguments) startInfo.ArgumentList.Add(argument);
#else
            startInfo.Arguments = string.Join(" ", arguments);
#endif
            return startInfo;
        }
    }
}