# GamesWithGravitas.NotifyPropertyChangedBase
![Available on NuGet](https://img.shields.io/nuget/v/GamesWithGravitas.NotifyPropertyChangedBase.svg)
![Downloads](https://img.shields.io/nuget/dt/GamesWithGravitas.NotifyPropertyChangedBase.svg)
## NotifyPropertyChangedBase
An implementation of `INotifyPropertyChanged` that provides  `SetProperty` method that triggers the `PropertyChanged` event.

`SetProperty` performs equality checking and is overloaded for many types to avoid boxing It includes an optional argument for providing multiple dependent properties that will change at the same time.

Additionally returns a `bool` indicating whether the value was actually changed.

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
![Available on NuGet](https://img.shields.io/nuget/v/GamesWithGravitas.XamarinForms.Layout.svg)
![Downloads](https://img.shields.io/nuget/dt/GamesWithGravitas.XamarinForms.Layout.svg)
## Layout.ItemsSource - Layout.ItemTemplate
A pair of attached properties that allow databinding the children of any `Layout`.
```xml
<ContentPage
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    xmlns:gwg="clr-namespace:GamesWithGravitas.XamarinForms.Layout;assembly=GamesWithGravitas.XamarinForms.Layout"
    x:Class="Sample.MyPage">
>
    <StackLayout gwg:Layout.ItemsSource="{Binding People}">
        <gwg:Layout.ItemTemplate>
            <DataTemplate>
                <StackLayout>
                    <Label Text="{Binding FirstName}"/>
                    <Label Text="{Binding LastName}"/>
                </StakLayout>
            </DataTemplate>
        </gwg:Layout.ItemTemplate>
    </StackLayout>
</ContentPage>
```
## LayerLayout
Docs coming soon.
## WrapLayout
Docs coming soon.
## UniformWrapLayout
Docs coming soon.
## UniformGridLayout
Docs coming soon.
# GamesWithGravitas.XamarinForms.SkiaSharp
![Available on NuGet](https://img.shields.io/nuget/v/GamesWithGravitas.XamarinForms.SkiaSharp.svg)
![Downloads](https://img.shields.io/nuget/dt/GamesWithGravitas.XamarinForms.SkiaSharp.svg)
## SKCanvasContentView - SKCanvasChildView
Docs coming soon.
## SKCanvasAnimatedChildView
Docs coming soon.
