using System;

namespace Plugin.Maui.ZoomView;

[ContentProperty(nameof(Content))]
public class ZoomView : View
{

	public static BindableProperty ContentProperty =
		  BindableProperty.Create(nameof(Content), typeof(View), typeof(ZoomView), default(View));

	public View Content
	{
		get => (View)GetValue(ContentProperty);
		set => SetValue(ContentProperty, value);
	}
}

public static class AppBuilderExtension
{
	public static MauiAppBuilder UseZoomView(this MauiAppBuilder builder)
	{
		builder.ConfigureMauiHandlers(handlers => 
		{
			handlers.AddHandler<ZoomView,ZoomViewHandler>();
		});
		return builder;
	}
}