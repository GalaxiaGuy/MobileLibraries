using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GamesWithGravitas
{
    public partial class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            foreach (var propertyName in propertyNames)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string whichProperty = null, params string[] otherProperties) where T : class
        {
            if (field == null && value == null)
            {
                return false;
            }
            else if (field != null && field.Equals(value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

        protected bool SetEnumProperty<T>(ref T field, T value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field.Equals(value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

        protected bool SetProperty<T1, T2, T3, T4, T5, T6, T7, TRest>(ref ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> field, ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> value, [CallerMemberName] string whichProperty = null, params string[] otherProperties) where TRest : struct
        {
            if (field.Equals(value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }
    }
}
