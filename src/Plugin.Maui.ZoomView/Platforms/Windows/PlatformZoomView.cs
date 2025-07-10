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
        
        // Enable smooth zooming
        IsZoomChainingEnabled = false;
        IsScrollInertiaEnabled = true;
        IsZoomInertiaEnabled = true;
        
        SetupDoubleTapGesture();
    }

    public void ResetZoom()
    {
        // Reset zoom and scroll position with animation
        ChangeView(0.0, 0.0, 1.0f, false);
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
        
        if (currentZoom > 1.0f + 0.01f && _zoomOutOnDoubleTap) // Small tolerance for float comparison
        {
            // Zoom out to default
            ResetZoom();
        }
        else if (currentZoom <= 1.0f + 0.01f && _zoomInOnDoubleTap) // Small tolerance for float comparison
        {
            // Zoom in to the tapped location
            var position = e.GetPosition(this);
            var targetZoom = Math.Min(2.0f, MaxZoomFactor);
            
            // Calculate the position to center the zoom on the tap location
            var centerX = Math.Max(0, position.X - (ActualWidth / 2 / targetZoom));
            var centerY = Math.Max(0, position.Y - (ActualHeight / 2 / targetZoom));
            
            ChangeView(centerX, centerY, targetZoom, false);
        }
        
        // Mark the event as handled to prevent other controls from processing it
        e.Handled = true;
    }

    public void Disconnect()
    {
        if (_doubleTapHandler != null)
        {
            DoubleTapped -= _doubleTapHandler;
            _doubleTapHandler = null;
        }
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        
        // Ensure content is properly sized when the template is applied
        if (Content is FrameworkElement content)
        {
            content.SizeChanged += OnContentSizeChanged;
        }
    }

    private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Update zoom bounds based on content size
        UpdateZoomBounds();
    }

    private void UpdateZoomBounds()
    {
        if (Content is FrameworkElement content && content.ActualWidth > 0 && content.ActualHeight > 0)
        {
            // Calculate minimum zoom to fit content
            var scaleX = ActualWidth / content.ActualWidth;
            var scaleY = ActualHeight / content.ActualHeight;
            var minScale = Math.Min(scaleX, scaleY);
            
            // Set minimum zoom factor to ensure content can always fit
            MinZoomFactor = (float)Math.Max(0.1, Math.Min(1.0, minScale));
        }
    }
}