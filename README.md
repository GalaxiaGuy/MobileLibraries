# GamesWithGravitas.NotifyPropertyChangedBase
## NotifyPropertyChangedBase
An implementation of `INotifyPropertyChanged` that provides  `SetProperty` method that triggers the `PropertyChanged` event.

`SetProperty` is overloaded for many types to avoid boxing, and includes an optional argument for providing multiple dependent properties that will change at the same time.

Additionally returns a `bool` indicating whether the value was actually changed.

Available on [NuGet](https://www.nuget.org/packages/GamesWithGravitas.NotifyPropertyChangedBase/). Supports `.NETStandard 1.0`.

```csharp
public class PersonViewModel : NotifyPropertyChangedBase
{
    private string _firstName;
    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value, otherProperties: nameof(FullName));
    }

    private string _lastName;
    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value, otherProperties: nameof(FullName));
    }

    public string FullName => $"{FirstName} {LastName}";

    private string _telephoneNumber;
    public string TelephoneNumber
    {
        get => _telephoneNumber;
        set
        {
            if (SetProperty(ref _telephoneNumber, value))
            {
                ValidateTelephoneNumber();
            }
        }
    }

    public void ValidateTelephoneNumber()
    {
        ...
    }
}
```
# GamesWithGravitas.XamarinForms.Layout
## Layout.ItemsSource - Layout.ItemTemplate
Docs coming soon.
## LayerLayout
Docs coming soon.
## WrapLayout
Docs coming soon.
## UniformWrapLayout
Docs coming soon.
## UniformGridLayout
Docs coming soon.
# GamesWithGravitas.XamarinForms.SkiaSharp
## SKCanvasContentView - SKCanvasChildView
Docs coming soon.
## SKCanvasAnimatedChildView
Docs coming soon.
