using Android.Content;
using Android.Graphics;
using Paint = Android.Graphics.Paint;
using Color = Android.Graphics.Color;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Plugin.Maui.ZoomView.Platforms.Android;

public class PlatformZoomView : FrameLayout
{
	public event EventHandler? LongPressed;
	#region  Properties
	bool _zoomInOnDoubleTap;
	bool _zoomInOutDoubleTap;
	float zoom = 1.0f;
	float maxZoom = 2.0f;
	float smoothZoom = 1.0f;
	float zoomX, zoomY;
	float smoothZoomX, smoothZoomY;
	bool scrolling;

	long lastTapTime;
	float touchStartX, touchStartY;
	float touchLastX, touchLastY;
	float startd;
	bool pinching;
	float lastd;
	float lastdx1, lastdy1;
	float lastdx2, lastdy2;

	Matrix m = new Matrix();
	Paint p = new Paint();
	Bitmap? ch;
	#endregion

	#region  Constructor
	public PlatformZoomView(Context context) : base(context)
	{
		ClipToOutline = true;
	}

	private const int LongPressTimeout = 2000; // ms
	private bool _longPressTriggered = false;
	private Handler? _handler;
	#endregion

	#region  Override
	public override bool DispatchTouchEvent(MotionEvent? e)
	{
		if (e is null)
			return base.DispatchTouchEvent(e);

		if (e.PointerCount == 1)
			ProcessSingleTouchEvent(e);
		else if (e.PointerCount == 2)
			ProcessDoubleTocuhEvent(e);

		PostInvalidateOnAnimation();
		return true;
	}

	protected override void DispatchDraw(Canvas canvas)
	{
		zoom = Lerp(Bias(zoom, smoothZoom, 0.05f), smoothZoom, 0.2f);
		smoothZoomX = Clamp(0.5f * Width / smoothZoom, smoothZoomX, Width - 0.5f * Width / smoothZoom);
		smoothZoomY = Clamp(0.5f * Height / smoothZoom, smoothZoomY, Height - 0.5f * Height / smoothZoom);
		zoomX = Lerp(Bias(zoomX, smoothZoomX, 0.1f), smoothZoomX, 0.35f);
		zoomY = Lerp(Bias(zoomY, smoothZoomY, 0.1f), smoothZoomY, 0.35f);

		bool animating = Math.Abs(zoom - smoothZoom) > 0.0000001f ||
						 Math.Abs(zoomX - smoothZoomX) > 0.0000001f ||
						 Math.Abs(zoomY - smoothZoomY) > 0.0000001f;

		if (ChildCount == 0)
			return;

		m.Reset();
		m.SetTranslate(0.5f * Width, 0.5f * Height);
		m.PreScale(zoom, zoom);
		m.PreTranslate(-Clamp(0.5f * Width / zoom, zoomX, Width - 0.5f * Width / zoom),
					   -Clamp(0.5f * Height / zoom, zoomY, Height - 0.5f * Height / zoom));

		var content = GetChildAt(0);
		if (content == null)
			return;

		m.PreTranslate(content.Left, content.Top);

		if (animating && ch == null && content.Width > 0 && content.Height > 0)
		{
			if (content.Width * content.Height < 4096 * 4096) // Conservative guard
			{
				ch = Bitmap.CreateBitmap(content.Width, content.Height, Bitmap.Config.Argb8888!);
				var tempCanvas = new Canvas(ch);
				content.Draw(tempCanvas);
			}
		}

		if (animating && ch != null)
		{
			p.Color = Color.White;
			canvas.DrawBitmap(ch, m, p);
		}
		else
		{
			if (ch != null)
			{
				ch.Dispose();
				ch = null;
			}

			canvas.Save();
			canvas.Concat(m);
			content.Draw(canvas);
			canvas.Restore();
		}

		if(animating)
		PostInvalidateOnAnimation();
	}

	protected override void OnLayout(bool changed, int l, int t, int r, int b)
	{
		if (ChildCount > 0)
		{
			var content = GetChildAt(0);
			if (content is not null)
			{
				content.Layout(0, 0, Width, Height);
			}
		}
	}
	protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
	{
		base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

		if (ChildCount > 0)
		{
			var content = GetChildAt(0);
			if (content is not null)
			{
				int width = MeasuredWidth;
				int height = MeasuredHeight;
				content.Measure(
					MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly),
					MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.Exactly));
			}
		}
	}
	#endregion

	#region  Private
	private void ProcessSingleTouchEvent(global::Android.Views.MotionEvent e)
	{
		float x = e.GetX();
		float y = e.GetY();
		float lx = x - touchStartX;
		float ly = y - touchStartY;
		float l = (float)Math.Sqrt(lx * lx + ly * ly);
		float dx = x - touchLastX;
		float dy = y - touchLastY;
		touchLastX = x;
		touchLastY = y;

		switch (e.Action)
		{
			case MotionEventActions.Down:
				touchStartX = x;
				touchStartY = y;
				touchLastX = x;
				touchLastY = y;
				scrolling = false;
				_longPressTriggered = false;
				_handler = new Handler(Looper.MainLooper);
				_handler.PostDelayed(() =>
				{
					if (!_longPressTriggered && smoothZoom == 1.0f)
					{
						_longPressTriggered = true;
						LongPressed?.Invoke(this, EventArgs.Empty);
					}
				}, LongPressTimeout);
				break;
			case MotionEventActions.Move:
				if (scrolling || (smoothZoom > 1.0f && l > 30.0f))
				{
					if (!scrolling)
					{
						scrolling = true;
						e.Action = MotionEventActions.Cancel;
						base.DispatchTouchEvent(e);
						_handler?.RemoveCallbacksAndMessages(null);
						return;
					}
					smoothZoomX -= dx / zoom;
					smoothZoomY -= dy / zoom;
					_handler?.RemoveCallbacksAndMessages(null);
					return;
				}
				break;
			case MotionEventActions.Up:
				_handler?.RemoveCallbacksAndMessages(null);
				if (l < 30.0f)
				{
					if (Java.Lang.JavaSystem.CurrentTimeMillis() - lastTapTime < 500)
					{
						if (smoothZoom == 1.0f && _zoomInOnDoubleTap)
							SmoothZoomTo(maxZoom, x, y);
						else if (_zoomInOutDoubleTap)
							SmoothZoomTo(1.0f, Width / 2.0f, Height / 2.0f);
						lastTapTime = 0;
						e.Action = MotionEventActions.Cancel;
						base.DispatchTouchEvent(e);
						return;
					}
					lastTapTime = Java.Lang.JavaSystem.CurrentTimeMillis();
					PerformClick();
				}
				break;
			case MotionEventActions.Cancel:
			_handler?.RemoveCallbacksAndMessages(null);
				break;
		}
	
		e.SetLocation(zoomX + (x - 0.5f * Width) / zoom, zoomY + (y - 0.5f * Height) / zoom);
		base.DispatchTouchEvent(e);
	}
	public void ResetZoom()
	{
		smoothZoom = 1.0f;
		zoom = 1.0f;
		smoothZoomX = Width / 2.0f;
		smoothZoomY = Height / 2.0f;
		zoomX = Width / 2.0f;
		zoomY = Height / 2.0f;
		Invalidate();
	}
	private void ProcessDoubleTocuhEvent(MotionEvent e)
	{
		float x1 = e.GetX(0);
		float dx1 = x1 - lastdx1;
		lastdx1 = x1;
		float y1 = e.GetY(0);
		float dy1 = y1 - lastdy1;
		lastdy1 = y1;
		float x2 = e.GetX(1);
		float dx2 = x2 - lastdx2;
		lastdx2 = x2;
		float y2 = e.GetY(1);
		float dy2 = y2 - lastdy2;
		lastdy2 = y2;

		float d = (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
		float dd = d - lastd;
		lastd = d;
		float ld = Math.Abs(d - startd);

		switch (e.Action)
		{
			case MotionEventActions.Down:
				startd = d;
				pinching = false;
				break;
			case MotionEventActions.Move:
				if (pinching || ld > 30.0f)
				{
					pinching = true;
					float dxk = 0.5f * (dx1 + dx2);
					float dyk = 0.5f * (dy1 + dy2);
					SmoothZoomTo(Math.Max(1.0f, zoom * d / (d - dd)), zoomX - dxk / zoom, zoomY - dyk / zoom);
				}
				break;
			default:
				pinching = false;
				break;
		}
		e.Action = MotionEventActions.Cancel;
		base.DispatchTouchEvent(e);
	}

	public void SmoothZoomTo(float zoom, float x, float y)
	{
		smoothZoom = Clamp(1.0f, zoom, maxZoom);
		smoothZoomX = x;
		smoothZoomY = y;
	}

	private float Clamp(float min, float value, float max) => Math.Max(min, Math.Min(value, max));
	private float Lerp(float a, float b, float k) => a + (b - a) * k;
	private float Bias(float a, float b, float k) => Math.Abs(b - a) >= k ? a + k * Math.Sign(b - a) : b;

	#endregion

	#region  Public
	public void SetZoomOnDoubleTap(bool zoomInOnDoubleTap, bool zoomOutonDoubleTap)
	{
		_zoomInOnDoubleTap = zoomInOnDoubleTap;
		_zoomInOutDoubleTap = zoomOutonDoubleTap;
	}
	#endregion
}
