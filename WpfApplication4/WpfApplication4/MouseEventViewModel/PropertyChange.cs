using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseOverEventMVVM
{
    class PropertyChange
    {
        public static void RaisePropertyChanged(object sender, string propertyName, PropertyChangedEventHandler handler)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
