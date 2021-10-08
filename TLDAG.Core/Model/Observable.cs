using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TLDAG.Core.Model
{
    public class Observable : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanging([CallerMemberName] string? name = null)
            { PropertyChanging?.Invoke(this, new(name)); }

        protected void RaisePropertyChanged([CallerMemberName] string? name = null)
            { PropertyChanged?.Invoke(this, new(name)); }
    }
}
