namespace ZoomViewSample;

public partial class MainPage : ContentPage
{


	public MainPage()
	{
		InitializeComponent();
	}

    private void OnResetZoomClicked(object sender, EventArgs e)
    {
        MyZoomView.Reset();
    }
	
}
