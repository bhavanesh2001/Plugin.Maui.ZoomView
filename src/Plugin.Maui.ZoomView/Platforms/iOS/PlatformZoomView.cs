using CoreGraphics;
using NetworkExtension;
using UIKit;

namespace Plugin.Maui.ZoomView.Platforms.iOS;

public class PlatformZoomView : UIScrollView
{
    UITapGestureRecognizer _doubleTap;
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
        ViewForZoomingInScrollView += GetViewForZooming;
        _doubleTap = new UITapGestureRecognizer(OnDoubleTapped)
        {
            CancelsTouchesInView = false,
            ShouldRecognizeSimultaneously = (tap,otherTap) => true,
            NumberOfTapsRequired = 2
        };
        AddGestureRecognizer(_doubleTap);

    }

    private void OnDoubleTapped(UIGestureRecognizer gesture)
    {
        var view = gesture.View;
        
        if(view is PlatformZoomView platformZoomView)
        {

            if(platformZoomView.ZoomScale > 1f)
            {
                platformZoomView.SetZoomScale(1f,true);
            }
            else if(platformZoomView.ZoomScale == 1f)
            {
                var tapLocationInView = gesture.LocationInView(view);
                var scrollViewSize = platformZoomView.Bounds.Size;
                var width = scrollViewSize.Width / 5.0f;
                var height = scrollViewSize.Height / 5.0f;
                var x = tapLocationInView.X - (width / 2f);
                var y = tapLocationInView.Y - (height / 2f);
                var zoomRect = new CGRect(x, y, width, height);
                platformZoomView.ZoomToRect(zoomRect,true);
            }
        }
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        var content = GetViewForZooming(this);
        if(content is not null && ZoomScale == 1f)
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
    public UIView GetViewForZooming(UIScrollView sv)
    {
        return sv.Subviews.FirstOrDefault();
    }

    private void ZoomAdjustmentToCenter(object? sender, EventArgs e)
    {
        nfloat offsetX = new nfloat(Math.Max((this.Bounds.Size.Width - this.ContentSize.Width) * 0.5, 0.0));
        nfloat offsetY = new nfloat(Math.Max((this.Bounds.Size.Height - this.ContentSize.Height) * 0.5, 0.0));
        this.ContentInset = new UIEdgeInsets(offsetY, offsetX, 0, 0);
    }
}
