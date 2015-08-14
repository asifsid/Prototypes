using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallaxEditorExtension
{
    public class ViewModel : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler _onPropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { _onPropertyChanged += value; }
            remove { _onPropertyChanged -= value; }
        }

        protected void InvokePropertyChanged(params string[] names)
        {
            foreach (var name in names)
            {
                _onPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
