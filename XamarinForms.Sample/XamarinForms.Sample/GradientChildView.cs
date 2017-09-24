using GamesWithGravitas.XamarinForms.SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public class GradientChildView : SKCanvasChildView
    {
        public override void Paint(SKSurface surface, SKImageInfo info)
        {
            var canvas = surface.Canvas;
            var colors = new SKColor[] { new SKColor(0, 255, 255), new SKColor(255, 0, 255), new SKColor(255, 255, 0), new SKColor(0, 255, 255) };
            var shader = SKShader.CreateLinearGradient(new SKPoint(0, 0), new SKPoint(255, 255), colors, null, SKShaderTileMode.Clamp);
            var paint = new SKPaint() { Shader = shader };
            canvas.DrawPaint(paint);
        }
    }
}
