using GamesWithGravitas.XamarinForms.SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public class BlueBoxChildView : SKCanvasChildView
    {
        public BlueBoxChildView()
        {
            WidthRequest = 20;
            HeightRequest = 20;
            ProvideOwnCanvasView = true;
        }

        public override void Paint(SKSurface surface, SKImageInfo info)
        {
            var paint = new SKPaint { Color = Color.Black.ToSKColor(), IsStroke = true, StrokeWidth=4 };
            surface.Canvas.Clear(Color.Blue.ToSKColor());
            surface.Canvas.DrawRect(new SKRect(0, 0, (float)Width, (float)Height), paint);
        }
    }
}
