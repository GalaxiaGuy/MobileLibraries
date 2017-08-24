using System;
using GamesWithGravitas.Extensions;
using Xunit;

namespace GamesWithGravitas
{
    public class NotifyPropertyChangedBaseTests
    {
        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void SettingAPropertyWorks(string propertyValue)
        {
            var notifier = new Notifier();
            var listener = notifier.ListenForPropertyChanged(nameof(Notifier.StringProperty));

            notifier.StringProperty = propertyValue;

            Assert.Equal(propertyValue, notifier.StringProperty);
            Assert.True(listener.AllTrue);
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void SettingADependentPropertyWorks(string propertyValue)
        {
            var notifier = new Notifier();
            var listener = notifier.ListenForPropertyChanged(nameof(Notifier.DependentProperty), nameof(Notifier.DerivedProperty));

            notifier.DependentProperty = propertyValue;

            Assert.Equal(propertyValue, notifier.DependentProperty);
            Assert.Equal(propertyValue + "!", notifier.DerivedProperty);
            Assert.True(listener.AllTrue);
        }
    }

    public class Notifier : NotifyPropertyChangedBase
    {
        private string _stringProperty;
        public string StringProperty
        {
            get => _stringProperty;
            set => SetProperty(ref _stringProperty, value);
        }

        private string _dependentProperty;
        public string DependentProperty
        {
            get => _dependentProperty;
            set => SetProperty(ref _dependentProperty, value, otherProperties: nameof(DerivedProperty));
        }

        public string DerivedProperty => DependentProperty + "!";
    }
}
