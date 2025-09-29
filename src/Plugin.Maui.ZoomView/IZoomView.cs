using System;

namespace Plugin.Maui.ZoomView;

public interface IZoomView: IView
{
    /// <summary>
    /// Gets or sets the content of the view.
    /// </summary>
    public View Content {get; set;}


    /// <summary>
    /// Gets or sets a value indicating whether a double-tap gesture should zoom into the content
    /// </summary>
    public bool ZoomInOnDoubleTap {get; set;}


    /// <summary>
    /// Gets or sets a value indicating whether a double-tap gesture should reset the zoom level
    /// </summary>
    public bool ZoomOutOnDoubleTap {get; set;}
    
    /// <summary>
    /// Gets or sets the current zoom level applied to the content.
    /// A value of <c>1.0f</c> represents the default (no zoom) scale.
    /// Values greater than 1.0 zoom in.
    /// </summary>
    public float Zoom {get; set;}


    /// <summary>
    /// Resets the zoom and position to the initial state.
    /// </summary>
    public void Reset();
}
