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

All the following assume you have something like the following in your XAML:

`xmlns:gwg="clr-namespace:GamesWithGravitas.XamarinForms.Layout;assembly=GamesWithGravitas.XamarinForms.Layout">`
## Layout.ItemsSource - Layout.ItemTemplate
A pair of attached properties that allow databinding the children of any `Layout`.
```xml
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
```
## LayerLayout
A `Layout` that displays all its children on top of each other. Each child is laid out with the size of the largest.
```xml
<gwg:LayerLayout>
    <Image Source="my_background" />
    <Text Label="Overload text" />
</gwg:LayerLayout>
```
## WrapLayout
A `Layout` that behaves like a horizontal `StackLayout`, arranging children one after the other until they can no longer fit. Additional children are then placed on a new row.

Each child will get at least the size it requests. If the child has `HorizontalOptions` set to include `Expand` it will be given more space in the same way it would when used in a `StackLayout`. Each child is given height equal to to the tallest child on the same row.

_Example coming soon._
## UniformWrapLayout
A `Layout` that behaves like `WrapLayout`, except all children are the same width (`Expand` is ignored).

_Example coming soon._
## UniformGrid
A `Layout` that behaves like `Grid`, except all cells are square. It will attempt to give every child the same space as the largest child.

_Example coming soon._
# GamesWithGravitas.XamarinForms.SkiaSharp
![Available on NuGet](https://img.shields.io/nuget/v/GamesWithGravitas.XamarinForms.SkiaSharp.svg)
![Downloads](https://img.shields.io/nuget/dt/GamesWithGravitas.XamarinForms.SkiaSharp.svg)
## SKCanvasContentView - SKCanvasChildView
_Docs coming soon._
## SKCanvasAnimatedChildView
_Docs coming soon._
