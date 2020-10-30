using System;
using System.Runtime.CompilerServices;

namespace GamesWithGravitas
{
	public partial class NotifyPropertyChangedBase
	{

	    protected bool SetProperty<T1>(ref ValueTuple<T1> field, ValueTuple<T1> value, [CallerMemberName] string whichProperty = "", params string[] otherProperties)
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

	    protected bool SetProperty<T1, T2>(ref ValueTuple<T1, T2> field, ValueTuple<T1, T2> value, [CallerMemberName] string whichProperty = "", params string[] otherProperties)
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

	    protected bool SetProperty<T1, T2, T3>(ref ValueTuple<T1, T2, T3> field, ValueTuple<T1, T2, T3> value, [CallerMemberName] string whichProperty = "", params string[] otherProperties)
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

	    protected bool SetProperty<T1, T2, T3, T4>(ref ValueTuple<T1, T2, T3, T4> field, ValueTuple<T1, T2, T3, T4> value, [CallerMemberName] string whichProperty = "", params string[] otherProperties)
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

	    protected bool SetProperty<T1, T2, T3, T4, T5>(ref ValueTuple<T1, T2, T3, T4, T5> field, ValueTuple<T1, T2, T3, T4, T5> value, [CallerMemberName] string whichProperty = "", params string[] otherProperties)
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

	    protected bool SetProperty<T1, T2, T3, T4, T5, T6>(ref ValueTuple<T1, T2, T3, T4, T5, T6> field, ValueTuple<T1, T2, T3, T4, T5, T6> value, [CallerMemberName] string whichProperty = "", params string[] otherProperties)
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

	    protected bool SetProperty<T1, T2, T3, T4, T5, T6, T7>(ref ValueTuple<T1, T2, T3, T4, T5, T6, T7> field, ValueTuple<T1, T2, T3, T4, T5, T6, T7> value, [CallerMemberName] string whichProperty = "", params string[] otherProperties)
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