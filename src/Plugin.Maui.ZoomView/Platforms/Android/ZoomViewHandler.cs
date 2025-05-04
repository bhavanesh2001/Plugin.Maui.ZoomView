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

        if (handler.PlatformView is not null && view.Content is not null)
        {
            handler.PlatformView.AddView(view.Content.ToPlatform(handler.MauiContext));
        }
    }

    public static void MapZoomOnDoubleTap(ZoomViewHandler handler, IZoomView view)
    {
        if (handler is not null && handler.PlatformView is not null)
        {
            handler.PlatformView.SetZoomOnDoubleTap(view.ZoomInOnDoubleTap, view.ZoomOutOnDoubleTap);
        }
    }

}
