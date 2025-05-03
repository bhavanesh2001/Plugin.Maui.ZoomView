using System;
using Microsoft.Maui.Handlers;
#if ANDROID
using PlatformView = Plugin.Maui.ZoomView.Platforms.Android.PlatformZoomView;
#elif IOS
using PlatformView = Plugin.Maui.ZoomView.Platforms.iOS.PlatformZoomView;
#endif


namespace Plugin.Maui.ZoomView;

public partial class ZoomViewHandler : ViewHandler<ZoomView, PlatformView>
{
     public static IPropertyMapper<ZoomView, ZoomViewHandler> PropertyMapper = new PropertyMapper<ZoomView, ZoomViewHandler>(ViewHandler.ViewMapper)
     {
            [nameof(ZoomView.Content)] = MapContent,
      };
    public ZoomViewHandler() : base(PropertyMapper, null)
    {
    }
}
