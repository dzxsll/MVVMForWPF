using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM
{
    public class NotifyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise notify when property value changed
        /// </summary>
        /// <param name="propertyName">property name</param>
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetAndNotifyIfChanged<T>(string propertyName, ref T oldValue, T newValue)
        {
            if (oldValue == null && newValue == null) return;

            if (oldValue != null)
            {
                if (newValue != null)
                {
                    if (newValue.Equals(oldValue))
                        return;
                }
                else
                    return;
            }

            if (newValue != null)
            {
                if (oldValue != null)
                {
                    if (oldValue.Equals(newValue))
                        return;
                }
            }

            oldValue = newValue;
            NotifyPropertyChanged(propertyName);
        }
    }
}