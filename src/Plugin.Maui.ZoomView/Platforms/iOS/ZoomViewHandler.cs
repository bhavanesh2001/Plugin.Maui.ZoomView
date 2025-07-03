    
using Microsoft.Maui.Platform;
using Plugin.Maui.ZoomView.Platforms.iOS;
using UIKit;

namespace Plugin.Maui.ZoomView
{

    public partial class ZoomViewHandler
    {
        private EventHandler? _longPressedHandler;
        protected override PlatformZoomView CreatePlatformView()
        {
            return new PlatformZoomView();
        }

        protected override void DisconnectHandler(PlatformZoomView platformView)
        {
            platformView.Disconnect();
            base.DisconnectHandler(platformView);
        }


        public static void MapContent(ZoomViewHandler handler, IZoomView view)
        {
            if (handler.MauiContext is null) throw new InvalidOperationException("MauiContext can not be null");

            if (handler.PlatformView is not null && view.Content is not null)
            {
                var content = view.Content.ToPlatform(handler.MauiContext);
                handler.PlatformView.AddSubview(content);
            }

        }

        public static void MapZoomOnDoubleTap(ZoomViewHandler handler, IZoomView view)
        {
            if (handler is not null && handler.PlatformView is not null)
            {
                handler.PlatformView.SetupDoubleTapGesture(view.ZoomInOnDoubleTap, view.ZoomOutOnDoubleTap);
            }
        }

        public static void MapLongPressedCommand(ZoomViewHandler handler, IZoomView view)
        {
            if (handler?.PlatformView == null)
                return;

            // Remove previous event handler if any
            if (handler._longPressedHandler != null)
            {
                handler.PlatformView.LongPressed -= handler._longPressedHandler;
                handler._longPressedHandler = null;
            }

            if (view.LongPressedCommand != null)
            {
                handler._longPressedHandler = (s, e) =>
                {
                    if (view.LongPressedCommand.CanExecute(null))
                        view.LongPressedCommand.Execute(null);
                };
                handler.PlatformView.LongPressed += handler._longPressedHandler;
            }
        }
        public static void MapReset(ZoomViewHandler handler, IZoomView view)
        {
            if (handler?.PlatformView == null)
                return;
                
            handler.PlatformView?.ResetZoom();
        }
    }
}
