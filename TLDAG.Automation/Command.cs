using System;
using System.Management.Automation;

namespace TLDAG.Automation
{
    public abstract class Command : Cmdlet
    {
        protected abstract void Begin();
        protected abstract void Process();
        protected abstract void End();

        protected override void BeginProcessing() { Invoke(Begin); }
        protected override void ProcessRecord() { Invoke(Process); }
        protected override void EndProcessing() { Invoke(End); }

        protected virtual void Invoke(Action action)
        {
            try { action(); }
            catch (Exception ex) { ThrowTerminatingError(Errors.Create(ex)); }
        }
    }
}
