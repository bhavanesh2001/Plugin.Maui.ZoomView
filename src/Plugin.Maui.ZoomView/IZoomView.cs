using System;
using System.Windows.Input;

namespace Plugin.Maui.ZoomView;

public interface IZoomView: IView
{
    public View Content {get; set;}

    public bool ZoomInOnDoubleTap {get; set;}

    public bool ZoomOutOnDoubleTap {get; set;}

    /// <summary>
    /// Resets the zoom and position to the initial state.
    /// </summary>
    public void Reset();

    /// <summary>
    /// Command to execute when the view is long pressed.
    /// </summary>
    public ICommand LongPressedCommand { get; set; }

}
