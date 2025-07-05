using CoreGraphics;
using NetworkExtension;
using UIKit;

namespace Plugin.Maui.ZoomView.Platforms.iOS;

public class PlatformZoomView : UIScrollView
{
    ZoomViewDoubleTap? _doubleTap;

    public bool ZoomInOnDoubleTap;
    public bool  ZoomOutOnDoubleTap;
    public PlatformZoomView()
    {
        this.DidZoom += ZoomAdjustmentToCenter;
        MaximumZoomScale = 10f;
        MinimumZoomScale = 1f;
        ShowsHorizontalScrollIndicator = false;
        ShowsVerticalScrollIndicator = false;
        Bounces = false;
        DelaysContentTouches = false;
        BouncesZoom = false;
        ViewForZoomingInScrollView += GetViewForZooming!;
    }
    public void ResetZoom()
    {
        SetZoomScale(1f, true);
        // Optionally, reset content offset if needed
        SetContentOffset(CGPoint.Empty, true);
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        var content = GetViewForZooming(this);
        if (content is not null && ZoomScale == 1f)
        {
            content.Frame = new CGRect(0, 0, this.Bounds.Width, this.Bounds.Height);
        }
    }

    public override CGSize SizeThatFits(CGSize size)
    {
        var content = GetViewForZooming(this);
        if (content != null)
        {
            return content.SizeThatFits(size);
        }
        return new CGSize(0, 0);
    }
    public UIView? GetViewForZooming(UIScrollView sv)
    {
        return sv.Subviews.FirstOrDefault();
    }

    private void ZoomAdjustmentToCenter(object? sender, EventArgs e)
    {
        nfloat offsetX = new nfloat(Math.Max((this.Bounds.Size.Width - this.ContentSize.Width) * 0.5, 0.0));
        nfloat offsetY = new nfloat(Math.Max((this.Bounds.Size.Height - this.ContentSize.Height) * 0.5, 0.0));
        this.ContentInset = new UIEdgeInsets(offsetY, offsetX, 0, 0);
    }

    public void SetupDoubleTapGesture(bool zoomInOnDoubleTap, bool zoomOutOnDoubleTap)
    {
        if(_doubleTap is null)
        {
            _doubleTap = new ZoomViewDoubleTap();
            ZoomInOnDoubleTap =  zoomInOnDoubleTap;
            ZoomOutOnDoubleTap =zoomOutOnDoubleTap;
            AddGestureRecognizer(_doubleTap);
        }
        else
        {
            ZoomInOnDoubleTap =  zoomInOnDoubleTap;
            ZoomOutOnDoubleTap =zoomOutOnDoubleTap;
        }
    }

    public void Disconnect()
    {
        if (_doubleTap is not null)
        {
             RemoveGestureRecognizer(_doubleTap);
            _doubleTap.ShouldReceiveTouch = null;
            _doubleTap.Dispose();
            _doubleTap = null;
        }
    }
}

internal class ZoomViewDoubleTap : UITapGestureRecognizer
{
    public ZoomViewDoubleTap() : base(OnDoubleTapped)
    {
        NumberOfTapsRequired = 2;
        CancelsTouchesInView = false;
        ShouldRecognizeSimultaneously = (gesture, otherGesture) => true;

    }

    private static void OnDoubleTapped(UIGestureRecognizer gesture)
    {
        if (gesture is not ZoomViewDoubleTap doubleTap)
            return;

        if (gesture.View is not PlatformZoomView zoomView)
            return;

        if (zoomView.ZoomScale > 1f && zoomView.ZoomOutOnDoubleTap)
        {
            zoomView.SetZoomScale(1f, true);
        }
        else if (zoomView.ZoomScale == 1f && zoomView.ZoomInOnDoubleTap)
        {
            var tapLocationInView = gesture.LocationInView(zoomView);
            var scrollViewSize = zoomView.Bounds.Size;
            var width = scrollViewSize.Width / 5.0f;
            var height = scrollViewSize.Height / 5.0f;
            var x = tapLocationInView.X - (width / 2f);
            var y = tapLocationInView.Y - (height / 2f);
            var zoomRect = new CGRect(x, y, width, height);
            zoomView.ZoomToRect(zoomRect, true);
        }
    }

}