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

    public static void MapLongPressedCommand(ZoomViewHandler handler, IZoomView view)
    {
        if (handler?.PlatformView == null)
            return;

        // Remove previous event handler if any
        if (handler._previousLongPressedHandler != null)
        {
            handler.PlatformView.LongPressed -= handler._previousLongPressedHandler;
        }

        // Add new event handler if command is not null
        if (view.LongPressedCommand != null)
        {
            EventHandler newHandler = (s, e) =>
            {
                if (view.LongPressedCommand.CanExecute(null))
                    view.LongPressedCommand.Execute(null);
            };
            handler.PlatformView.LongPressed += newHandler;
            handler._previousLongPressedHandler = newHandler;
        }
    }
}
