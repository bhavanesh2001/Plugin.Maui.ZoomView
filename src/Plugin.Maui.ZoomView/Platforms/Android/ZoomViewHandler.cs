using Microsoft.Maui.Platform;
using Plugin.Maui.ZoomView.Platforms.Android;

namespace Plugin.Maui.ZoomView;

public partial class ZoomViewHandler
{
    protected override PlatformZoomView CreatePlatformView()
    {
        return new PlatformZoomView(Context);
    }
    public static void MapContent(ZoomViewHandler handler, IZoomView view)
    {
        if (handler.MauiContext is null) throw new InvalidOperationException("MauiContext can not be null");

        if (handler.IsConnected() && view.Content is not null)
        {
            handler.PlatformView.AddView(view.Content.ToPlatform(handler.MauiContext));
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
            handler.PlatformView?.ResetZoom();
        }
    }
}
