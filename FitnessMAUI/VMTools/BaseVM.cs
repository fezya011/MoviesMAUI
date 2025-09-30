using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FitnessMAUI.VMTools
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void Signal([CallerMemberName] string name = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
