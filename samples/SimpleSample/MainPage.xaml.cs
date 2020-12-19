// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace SimpleSample
{
    /// <summary>
    /// Draws some graphics using Win2D
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            canvasControl.SizeChanged += CanvasControl_SizeChanged;
        }

        private void CanvasControl_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            CanvasSwapChain currentSwapChain = canvasControl.SwapChain;
            if (currentSwapChain == null)
            {
                CanvasSwapChain swapChain = new CanvasSwapChain(new CanvasDevice(), (float)e.NewSize.Width * 2, (float)e.NewSize.Height * 2, 96 * canvasControl.CompositionScaleX);
                canvasControl.SwapChain = swapChain;
            } else if (currentSwapChain.Size.Width < e.NewSize.Width || currentSwapChain.Size.Height < e.NewSize.Height)
            {
                canvasControl.SwapChain.ResizeBuffers((float)e.NewSize.Width * 2, (float)e.NewSize.Height * 2, 96 * canvasControl.CompositionScaleX);
            }
            var ds = canvasControl.SwapChain.CreateDrawingSession(Colors.AliceBlue);
            ds.Dispose();
            canvasControl.SwapChain.Present();
        }

        void canvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawEllipse(155, 115, 80, 30, Colors.Black, 3);
            args.DrawingSession.DrawText("Hello, world!", 100, 100, Colors.Yellow);
        }

        private CoreIndependentInputSource coreInput;

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // CanvasSwapChain swapChain = new CanvasSwapChain(new CanvasDevice(), 800, 600, 96);

            // canvasControl.SwapChain = swapChain;

            // using (CanvasDrawingSession ds = swapChain.CreateDrawingSession(Colors.AliceBlue))
            // {
            // }

            // swapChain.Present();

            var a = ThreadPool.RunAsync((_) =>
            {
                coreInput = canvasControl.CreateCoreIndependentInputSource(Windows.UI.Core.CoreInputDeviceTypes.Mouse);

                coreInput.PointerMoved += CoreInput_PointerMoved;

                coreInput.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
            }, WorkItemPriority.High);
        }

        private void CoreInput_PointerMoved(object sender, PointerEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Moved {0} {1}", args.CurrentPoint.Position.X, args.CurrentPoint.Position.Y);
        }
    }
}
