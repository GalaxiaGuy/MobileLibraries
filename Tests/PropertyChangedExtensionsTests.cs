using GamesWithGravitas.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace GamesWithGravitas
{
	public class PropertyChangedExtensionsTests
	{
        [Theory]
		[InlineData("Foo")]
		[InlineData("Bar")]
		[InlineData("Baz")]
		public void SinglePropertyFired(string propertyName)
		{
			var notifier = new Notifier();
			var result = notifier.ListenForPropertyChanged(propertyName);

			notifier.RaisePropertyChanged(propertyName);

			Assert.True(result.AllTrue);
			Assert.Equal(1, result.Results.Count);
			Assert.Equal(1, result.Successes.Count());
			Assert.Contains(propertyName, result.Results.Keys);
		}

        [Theory]
        [InlineData("Foo", "Bar")]
		[InlineData("Bar", "Baz")]
		public void SeveralPropertiesAllFired(params string[] propertyNames)
		{
			var notifier = new Notifier();
			var result = notifier.ListenForPropertyChanged(propertyNames);

			foreach (var propertyName in propertyNames)
			{
				notifier.RaisePropertyChanged(propertyName);
			}

			Assert.True(result.AllTrue);
			Assert.Equal(propertyNames.Length, result.Results.Count);
			Assert.Equal(propertyNames.Length, result.Successes.Count());
			foreach (var propertyName in propertyNames)
			{
				Assert.Contains(propertyName, result.Results.Keys);
				Assert.Contains(propertyName, result.Successes.ToList());
			}
		}

        [Theory]
        [InlineData("Foo")]
		[InlineData("Bar")]
		[InlineData("Baz")]
		public void SinglePropertyNotFired(string propertyName)
		{
			var notifier = new Notifier();
			var result = notifier.ListenForPropertyChanged(propertyName);

			Assert.False(result.AllTrue);
			Assert.Equal(1, result.Results.Count);
			Assert.Equal(0, result.Successes.Count());
		}

        [Theory]
        [InlineData("Foo", "Bar")]
		[InlineData("Bar", "Baz")]
		public void SeveralPropertiesOnlyOneFired(string firedPropertyName, params string[] notFiredPropertyNames)
		{
			var notifier = new Notifier();
			var result = notifier.ListenForPropertyChanged(notFiredPropertyNames.Concat(new string[] { firedPropertyName }).ToArray());

			notifier.RaisePropertyChanged(firedPropertyName);

			Assert.False(result.AllTrue);
			Assert.Equal(notFiredPropertyNames.Length + 1, result.Results.Count);
			Assert.Equal(1, result.Successes.Count());
			Assert.Contains(firedPropertyName, result.Results.Keys);
			Assert.Contains(firedPropertyName, result.Successes.ToList());
			foreach (var propertyName in notFiredPropertyNames)
			{
				Assert.Contains(propertyName, result.Results.Keys);
			}
		}

        [Theory]
        [InlineData("Foo")]
		[InlineData("Foo", "Bar")]
		[InlineData("Foo", "Bar", "Baz")]
		public void FailMessageIsCorrect(params string[] propertyNames)
		{
			var notifier = new Notifier();
			var result = notifier.ListenForPropertyChanged(propertyNames);

			foreach (var propertyName in propertyNames)
			{
				Assert.True(result.FailMessage.Contains(propertyName));
			}
		}

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        [InlineData("Baz")]
        [InlineData("Foo", "Bar")]
        [InlineData("Foo", "Bar", "Baz")]
        public void DisposingUnsubscribes(params string[] propertyNames)
        {
            var notifier = new Notifier();
            var listener = notifier.ListenForPropertyChanged(propertyNames);
            listener.Dispose();

            Assert.True(notifier.IsPropertyChangedNull);
            foreach (var propertyName in propertyNames)
            {
                notifier.RaisePropertyChanged(propertyName);
            };
            Assert.False(listener.AllTrue);
        }

        public class Notifier : INotifyPropertyChanged
		{
			public void RaisePropertyChanged(string propertyName)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

			public event PropertyChangedEventHandler PropertyChanged;

            public bool IsPropertyChangedNull => PropertyChanged == null;
		}
	}
}
