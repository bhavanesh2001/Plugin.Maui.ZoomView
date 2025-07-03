using System.Windows.Input;

namespace ZoomViewSample;

public partial class MainPage : ContentPage
{
    public ICommand LongPressedCommand { get; }

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        LongPressedCommand = new Command(OnLongPressed);
        MyZoomView.LongPressedCommand = LongPressedCommand;
    }

    private async void OnLongPressed()
    {
        await DisplayAlert("Alert", "Long pressed!", "OK");
    }

    private void OnResetZoomClicked(object sender, EventArgs e)
    {
        MyZoomView.Reset();
    }

}
