using GamesWithGravitas.XamarinForms.SkiaSharp;
using System;
using SkiaSharp;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public class RingView : SKCanvasChildView
    {
        public RingView()
        {
        }

        public override void Paint(SKSurface surface, SKImageInfo info)
        {
            var canvas = surface.Canvas;
            var width = canvas.LocalClipBounds.Width;
            var height = canvas.LocalClipBounds.Height;

            var paint = new SKPaint { Color = Color.Black.ToSKColor(), IsStroke = true, StrokeWidth=0.075f*width };
            canvas.Clear(Color.Blue.ToSKColor());
            canvas.DrawCircle(width / 2, height / 2, width / 3, paint);
        }
    }
}
