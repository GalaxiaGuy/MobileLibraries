using System;
using System.Runtime.CompilerServices;

namespace GamesWithGravitas
{
	public partial class NotifyPropertyChangedBase
	{

	    protected bool SetProperty(ref bool field, bool value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref byte field, byte value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref short field, short value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref int field, int value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref long field, long value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref float field, float value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref double field, double value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref decimal field, decimal value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref TimeSpan field, TimeSpan value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref DateTime field, DateTime value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
            {
                return false;
            }
            field = value;
            OnPropertyChanged(whichProperty);
            OnPropertyChanged(otherProperties);
            return true;
        }

	    protected bool SetProperty(ref DateTimeOffset field, DateTimeOffset value, [CallerMemberName] string whichProperty = null, params string[] otherProperties)
        {
            if (field == value)
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