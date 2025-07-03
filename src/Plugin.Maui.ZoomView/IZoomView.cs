using System;
using System.Windows.Input;

namespace Plugin.Maui.ZoomView;

public interface IZoomView : IView
{
    /// <summary>
    /// Gets or sets the content of the view.
    /// </summary>
    public View Content { get; set; }


    /// <summary>
    /// Gets or sets a value indicating whether a double-tap gesture should zoom into the content
    /// </summary>
    public bool ZoomInOnDoubleTap { get; set; }


    /// <summary>
    /// Gets or sets a value indicating whether a double-tap gesture should reset the zoom level
    /// </summary>
    public bool ZoomOutOnDoubleTap { get; set; }


    /// <summary>
    /// Resets the zoom and position to the initial state.
    /// </summary>
    public void Reset();


    /// <summary>
    /// Command to execute when the view is long pressed.
    /// </summary>
    public ICommand LongPressedCommand { get; set; }

}
