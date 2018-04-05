using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GamesWithGravitas.XamarinForms.SkiaSharp
{
    public abstract class SKCanvasAnimatedChildView : SKCanvasChildView
    {
        protected double Parameter { get; private set; }

        private CancellationTokenSource _tokenSource;

        protected Task AnimateAsync(int duration, int delay = 0) => AnimateAsync(duration, delay, CancellationToken.None);

        protected Stopwatch Stopwatch;

        protected int Duration;

        protected CancellationToken Token;

        protected async Task AnimateAsync(int duration, int delay, CancellationToken token)
        {
            _tokenSource?.Cancel();
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            Token = _tokenSource.Token;
            if (delay > 0)
            {
                await Task.Delay(delay, token);
            }
            if (token.IsCancellationRequested)
            {
                return;
            }
            Stopwatch = Stopwatch.StartNew();
            Parameter = 0;
            Duration = duration;
            CanvasView?.InvalidateSurface();
        }

		public override void Paint(SKSurface surface, SKImageInfo info)
        {
            Parameter = Stopwatch.ElapsedMilliseconds / (float)Duration;
            if (Parameter > 1)
            {
                Parameter = 1;
                return;
            }
            if (Token.IsCancellationRequested)
            {
                Stopwatch.Stop();
                return;
            }
            CanvasView?.InvalidateSurface();
		}
	}
}
