using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using Windows.Foundation;

namespace Plugin.Maui.ZoomView.Platforms.Windows;

public class PlatformZoomView : ScrollViewer
{
    private bool _zoomInOnDoubleTap = true;
    private bool _zoomOutOnDoubleTap = true;
    private DoubleTappedEventHandler? _doubleTapHandler;

    public PlatformZoomView()
    {
        ZoomMode = ZoomMode.Enabled;
        HorizontalScrollMode = ScrollMode.Enabled;
        VerticalScrollMode = ScrollMode.Enabled;
        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        IsTabStop = false;
        
        // Set zoom constraints
        MinZoomFactor = 1.0f;
        MaxZoomFactor = 10.0f;
        
        SetupDoubleTapGesture();
    }

    public void ResetZoom()
    {
        ChangeView(null, null, 1.0f, true);
    }

    public void SetZoomOnDoubleTap(bool zoomInOnDoubleTap, bool zoomOutOnDoubleTap)
    {
        _zoomInOnDoubleTap = zoomInOnDoubleTap;
        _zoomOutOnDoubleTap = zoomOutOnDoubleTap;
    }

    private void SetupDoubleTapGesture()
    {
        _doubleTapHandler = OnDoubleTapped;
        DoubleTapped += _doubleTapHandler;
    }

    private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        var currentZoom = ZoomFactor;
        
        if (currentZoom > 1.0f && _zoomOutOnDoubleTap)
        {
            // Zoom out to default
            ResetZoom();
        }
        else if (currentZoom == 1.0f && _zoomInOnDoubleTap)
        {
            // Zoom in to the tapped location
            var position = e.GetPosition(this);
            var targetZoom = 2.0f;
            
            // Calculate the position to center the zoom on the tap location
            var centerX = position.X - (ActualWidth / 2 / targetZoom);
            var centerY = position.Y - (ActualHeight / 2 / targetZoom);
            
            ChangeView(centerX, centerY, targetZoom, true);
        }
    }

    public void Disconnect()
    {
        if (_doubleTapHandler != null)
        {
            DoubleTapped -= _doubleTapHandler;
            _doubleTapHandler = null;
        }
    }
}