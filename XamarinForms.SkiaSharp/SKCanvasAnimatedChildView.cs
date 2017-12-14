using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GamesWithGravitas.XamarinForms.SkiaSharp
{
    public abstract class SKCanvasAnimatedChildView : SKCanvasChildView
    {
        protected double Parameter { get; private set; }

        protected int FrameRate
        {
            set => FrameDuration = 1000 / value;
        }

        private CancellationTokenSource _tokenSource;

        protected int FrameDuration { get; private set; } = 33;

        protected Task AnimateAsync(int duration, int delay = 0) => AnimateAsync(duration, delay, CancellationToken.None);

        protected async Task AnimateAsync(int duration, int delay, CancellationToken token)
        {
            _tokenSource?.Cancel();
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
            token = _tokenSource.Token;
            if (delay > 0)
            {
                await Task.Delay(delay, token);
            }
            if (token.IsCancellationRequested)
            {
                return;
            }
            var stopwatch = Stopwatch.StartNew();
            Parameter = 0;
            while (true)
            {
                CanvasView?.InvalidateSurface();
                Parameter = stopwatch.ElapsedMilliseconds / (float)duration;
                if (Parameter > 1)
                {
                    Parameter = 1;
                    break;
                }
                await Task.Delay(FrameDuration, token);
                if (token.IsCancellationRequested)
                {
                    stopwatch.Stop();
                    return;
                }
            }
            stopwatch.Stop();
        }
    }
}
