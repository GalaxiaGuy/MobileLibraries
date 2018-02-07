using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GamesWithGravitas.Extensions
{
    public static class PropertyChangedExtensions
    {
        public static PropertyChangedListener ListenForPropertyChanged(this INotifyPropertyChanged target, params string[] properties)
        {
            var listener = new PropertyChangedListener(properties);
            listener.Subscribe(target);
            return listener;
        }

        public class PropertyChangedListener : IDisposable
        {
            public PropertyChangedListener(IEnumerable<string> properties)
            {
                Results = properties.ToDictionary(x => x, x => false);
            }

            private List<INotifyPropertyChanged> _targets = new List<INotifyPropertyChanged>();

            public Dictionary<string, bool> Results;

            public IEnumerable<string> Successes => Results.Where(x => x.Value).Select(x => x.Key);

            public bool AllTrue => Results.Values.All(x => x);

            public string FailMessage => string.Join(" ", Results.Where(x => !x.Value).Select(x => string.Format("PropertyChanged not called for {{{0}}}.", x.Key)));

            public void Subscribe(INotifyPropertyChanged target)
            {
                target.PropertyChanged += TargetPropertyChanged;
                _targets.Add(target);
            }

            public void Unsubscribe(INotifyPropertyChanged target)
            {
                target.PropertyChanged -= TargetPropertyChanged;
                _targets.Remove(target);
            }

            private void TargetPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (Results.ContainsKey(e.PropertyName))
                {
                    Results[e.PropertyName] = true;
                }
            }

            public void Dispose()
            {
                foreach (var target in _targets.ToList())
                {
                    Unsubscribe(target);
                }
            }
        }
    }
}
