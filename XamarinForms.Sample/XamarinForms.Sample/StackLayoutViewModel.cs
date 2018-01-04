using System;
using System.Collections.Generic;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public class StackLayoutViewModel
    {
        public StackLayoutViewModel()
        {
        }

        public List<string> Items { get; } = new List<string>()
        {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5",
        };
    }
}
