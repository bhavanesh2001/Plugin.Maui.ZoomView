using System;

namespace Plugin.Maui.ZoomView;

[ContentProperty(nameof(Content))]
public class ZoomView : View, IZoomView
{

	public static BindableProperty ContentProperty =
		BindableProperty.Create(nameof(Content), typeof(View), typeof(ZoomView), default(View));

	public View Content
	{
		get => (View)GetValue(ContentProperty);
		set => SetValue(ContentProperty, value);
	}

	public static readonly BindableProperty ZoomInOnDoubleTapProperty =
		BindableProperty.Create(nameof(ZoomInOnDoubleTap),typeof(bool),typeof(ZoomView),true);

	/// <summary>
	/// Gets or sets a value indicating whether a double-tap gesture should zoom into the content
	/// when the zoom level is at the default scale.
	/// </summary>
	public bool ZoomInOnDoubleTap
	{
		get => (bool)GetValue(ZoomInOnDoubleTapProperty);
		set => SetValue(ZoomInOnDoubleTapProperty, value);
	}

	public static readonly BindableProperty ZoomOutOnDoubleTapProperty =
		BindableProperty.Create(nameof(ZoomOutOnDoubleTap),typeof(bool),typeof(ZoomView),true);

	/// <summary>
	/// Gets or sets a value indicating whether a double-tap gesture should reset the zoom level 
	/// back to default when the content is already zoomed in.
	/// </summary>
	public bool ZoomOutOnDoubleTap
	{
		get => (bool)GetValue(ZoomOutOnDoubleTapProperty);
		set => SetValue(ZoomOutOnDoubleTapProperty, value);
	}

	public static BindableProperty ZoomProperty =
		BindableProperty.Create(nameof(Zoom), typeof(float), typeof(ZoomView), 1.0f);

	/// <summary>
	/// Gets or sets the current zoom level applied to the content.
	/// A value of <c>1.0f</c> represents the default (no zoom) scale.
	/// Values greater than 1.0 zoom in.
	/// </summary>
	public float Zoom
	{
		get => (float)GetValue(ZoomProperty);
		set => SetValue(ZoomProperty, value);
	}
	/// <summary>
	/// Resets the zoom and position to the initial state.
	/// </summary>
	public void Reset()
	{
		Handler?.Invoke(nameof(IZoomView.Reset), EventArgs.Empty);
	}

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (Content is VisualElement ve)
        {
            ve.BindingContext = BindingContext;
        }
    }
}

public static class AppBuilderExtension
{
	public static MauiAppBuilder UseZoomView(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddHandler<IZoomView, ZoomViewHandler>();
		});
		return builder;
	}
}