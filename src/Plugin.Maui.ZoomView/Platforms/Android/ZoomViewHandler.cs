using Android.Views;
using Microsoft.Maui.Platform;
using Plugin.Maui.ZoomView.Platforms.Android;

namespace Plugin.Maui.ZoomView;

public partial class ZoomViewHandler
{
    protected override PlatformZoomView CreatePlatformView()
    {
       return new PlatformZoomView(Context);
    }
    public static void MapContent(ZoomViewHandler handler, ZoomView view)
    {
        if(handler.MauiContext is null) throw new InvalidOperationException("MauiContext can not be null");
        
        if (handler.PlatformView is not null && view.Content is not null)
        {
            handler.PlatformView.AddView(view.Content.ToPlatform(handler.MauiContext));
        }
    }
}
