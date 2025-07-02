using System;
using Microsoft.Maui.Handlers;
#if ANDROID
using PlatformView = Plugin.Maui.ZoomView.Platforms.Android.PlatformZoomView;
#elif IOS
using PlatformView = Plugin.Maui.ZoomView.Platforms.iOS.PlatformZoomView;
#else
using PlatformView = Plugin.Maui.ZoomView.PlatformZoomView;
#endif



namespace Plugin.Maui.ZoomView;

public partial class ZoomViewHandler : ViewHandler<IZoomView, PlatformView>
{
  public static IPropertyMapper<IZoomView, ZoomViewHandler> PropertyMapper = new PropertyMapper<IZoomView, ZoomViewHandler>(ViewHandler.ViewMapper)
  {
    [nameof(IZoomView.Content)] = MapContent,
    [nameof(IZoomView.ZoomInOnDoubleTap)] = MapZoomOnDoubleTap,
    [nameof(IZoomView.ZoomOutOnDoubleTap)] = MapZoomOnDoubleTap,
    ["Reset"] = MapReset,
    [nameof(IZoomView.LongPressedCommand)] = MapLongPressedCommand,
  };

   

    public ZoomViewHandler() : base(PropertyMapper, null)
    {
    }
    public static void MapReset(ZoomViewHandler handler, IZoomView view)
    {
        handler?.PlatformView?.ResetZoom();
    }

}
