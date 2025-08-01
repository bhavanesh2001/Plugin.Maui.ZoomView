using System;
using Microsoft.Maui.Handlers;
#if ANDROID
using PlatformView = Plugin.Maui.ZoomView.Platforms.Android.PlatformZoomView;
#elif IOS
using PlatformView = Plugin.Maui.ZoomView.Platforms.iOS.PlatformZoomView;
#elif WINDOWS
using PlatformView = Plugin.Maui.ZoomView.Platforms.Windows.PlatformZoomView;
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
  };

  internal static CommandMapper<IZoomView, ZoomViewHandler> CommandMapper = new(ViewCommandMapper)
  {
    [nameof(IZoomView.Reset)] = MapReset
  };

  public ZoomViewHandler() : base(PropertyMapper, CommandMapper)
  {
  }
}
