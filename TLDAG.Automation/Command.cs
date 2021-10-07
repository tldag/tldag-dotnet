using System;
using System.Management.Automation;

namespace TLDAG.Automation
{
    public abstract class Command : Cmdlet
    {
        protected override void ProcessRecord()
        {
            try
            {
                Process();
            }
            catch (Exception ex)
            {
                ThrowTerminatingError(Errors.Create(ex));
            }
        }

        protected abstract void Process();
    }
}
