

using Microsoft.Maui.Platform;
using Plugin.Maui.ZoomView.Platforms.iOS;
using UIKit;

namespace Plugin.Maui.ZoomView;

public partial class ZoomViewHandler
{
    protected override PlatformZoomView CreatePlatformView()
    {
       return new PlatformZoomView();
    }

    
    public static void MapContent(ZoomViewHandler handler, ZoomView view)
    {
        if(handler.MauiContext is null) throw new InvalidOperationException("MauiContext can not be null");
        
        if (handler.PlatformView is not null && view.Content is not null)
        {
            var content = view.Content.ToPlatform(handler.MauiContext);
            handler.PlatformView.AddSubview(content);
        }
        
    }
}
