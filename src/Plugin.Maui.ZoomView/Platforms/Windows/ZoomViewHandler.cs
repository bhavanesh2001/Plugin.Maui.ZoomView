using Microsoft.Maui.Platform;
using Plugin.Maui.ZoomView.Platforms.Windows;

namespace Plugin.Maui.ZoomView;

public partial class ZoomViewHandler
{
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
        if (handler.MauiContext is null) 
            throw new InvalidOperationException("MauiContext cannot be null");

        if (handler.IsConnected() && view.Content is not null)
        {
            var content = view.Content.ToPlatform(handler.MauiContext);
            handler.PlatformView.Content = content;
        }
    }

    public static void MapZoomOnDoubleTap(ZoomViewHandler handler, IZoomView view)
    {
        if (handler.IsConnected())
        {
            handler.PlatformView.SetZoomOnDoubleTap(view.ZoomInOnDoubleTap, view.ZoomOutOnDoubleTap);
        }
    }

    public static void MapReset(ZoomViewHandler handler, IZoomView view, object? args)
    {
        if (handler.IsConnected())
        {
            handler.PlatformView.ResetZoom();
        }
    }
}