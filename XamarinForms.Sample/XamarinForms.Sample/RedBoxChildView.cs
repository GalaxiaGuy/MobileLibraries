using GamesWithGravitas.XamarinForms.SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public class RedBoxChildView : SKCanvasChildView
    {
        public override void Paint(SKSurface surface, SKImageInfo info)
        {
            var paint = new SKPaint { Color = Color.Black.ToSKColor(), IsStroke = true, StrokeWidth=4 };
            surface.Canvas.Clear(Color.Red.ToSKColor());
            surface.Canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), paint);
        }
    }
}
