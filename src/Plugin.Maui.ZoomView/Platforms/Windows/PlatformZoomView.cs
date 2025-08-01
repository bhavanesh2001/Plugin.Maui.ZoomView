using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using ContentPresenter = Microsoft.UI.Xaml.Controls.ContentPresenter;
using ScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility;
using ScrollMode = Microsoft.UI.Xaml.Controls.ScrollMode;

namespace Plugin.Maui.ZoomView.Platforms.Windows
{
    public class PlatformZoomView : Microsoft.UI.Xaml.Controls.Grid
    {
        private readonly ScrollViewer _scrollViewer;
        private readonly Microsoft.UI.Xaml.Controls.ContentPresenter _contentPresenter;
        private bool _zoomInOnDoubleTap = true;
        private bool _zoomOutOnDoubleTap = true;

        public PlatformZoomView()
        {
            _scrollViewer = new ScrollViewer
            {
                ZoomMode = ZoomMode.Enabled,
                HorizontalScrollMode = ScrollMode.Enabled,
                VerticalScrollMode = ScrollMode.Enabled,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                IsTabStop = false,
                MinZoomFactor = 1.0f,
                MaxZoomFactor = 10.0f,
                IsZoomChainingEnabled = false,
                IsScrollInertiaEnabled = true,
                IsZoomInertiaEnabled = true
            };

            _contentPresenter = new ContentPresenter();
            _scrollViewer.Content = _contentPresenter;

            Children.Add(_scrollViewer);

            SetupDoubleTapGesture();
        }

        public void SetContent(UIElement content)
        {
            _contentPresenter.Content = content;
            UpdateZoomBounds();
        }

        public void ResetZoom() =>
            _scrollViewer.ChangeView(0.0, 0.0, 1.0f, false);

        public void SetZoomOnDoubleTap(bool zoomInOnDoubleTap, bool zoomOutOnDoubleTap)
        {
            _zoomInOnDoubleTap = zoomInOnDoubleTap;
            _zoomOutOnDoubleTap = zoomOutOnDoubleTap;
        }

        private void SetupDoubleTapGesture()
        {
            DoubleTapped += OnDoubleTapped;
        }

        private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var currentZoom = _scrollViewer.ZoomFactor;

            if (currentZoom > 1.0f + 0.01f && _zoomOutOnDoubleTap)
            {
                ResetZoom();
            }
            else if (currentZoom <= 1.0f + 0.01f && _zoomInOnDoubleTap)
            {
                var position = e.GetPosition(this);
                var targetZoom = Math.Min(2.0f, _scrollViewer.MaxZoomFactor);

                var centerX = Math.Max(0, position.X - (ActualWidth / 2 / targetZoom));
                var centerY = Math.Max(0, position.Y - (ActualHeight / 2 / targetZoom));

                _scrollViewer.ChangeView(centerX, centerY, targetZoom, false);
            }

            e.Handled = true;
        }

        public void Disconnect()
        {
            DoubleTapped -= OnDoubleTapped;
        }

        private void UpdateZoomBounds()
        {
            if (_contentPresenter.Content is FrameworkElement content && content.ActualWidth > 0 && content.ActualHeight > 0)
            {
                var scaleX = ActualWidth / content.ActualWidth;
                var scaleY = ActualHeight / content.ActualHeight;
                var minScale = Math.Min(scaleX, scaleY);

                _scrollViewer.MinZoomFactor = (float)Math.Max(0.1, Math.Min(1.0, minScale));
            }
        }
    }
}
