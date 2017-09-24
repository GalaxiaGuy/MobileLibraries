using GamesWithGravitas.XamarinForms.SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;

namespace GamesWithGravitas.XamarinForms.Sample
{
    public class CompositeShaderChildView : SKCanvasChildView
    {
        public override void Paint(SKSurface surface, SKImageInfo info)
        {
            var canvas = surface.Canvas;
            var colors = new SKColor[] { new SKColor(0, 255, 255), new SKColor(255, 0, 255), new SKColor(255, 255, 0), new SKColor(0, 255, 255) };
            var sweep = SKShader.CreateSweepGradient(new SKPoint(128, 128), colors, null);
            var turbulence = SKShader.CreatePerlinNoiseTurbulence(0.05f, 0.05f, 4, 0);
            var shader = SKShader.CreateCompose(sweep, turbulence);
            var paint = new SKPaint() { Shader = shader };
            canvas.DrawPaint(paint);
        }
    }
}
