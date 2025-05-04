using System;

namespace Plugin.Maui.ZoomView;

public interface IZoomView: IView
{
    public View Content {get; set;}

    public bool ZoomInOnDoubleTap {get; set;}

    public bool ZoomOutOnDoubleTap {get; set;}

}
