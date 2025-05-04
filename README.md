# Plugin.Maui.ZoomView

## Overview
Plugin.Maui.ZoomView is a .NET MAUI plugin that provides a customizable zoomable view for cross-platform applications. It allows developers to easily integrate zooming functionality into their applications, supporting **Android** and **iOS** platforms.

---

## Features
✨ Smooth zooming and panning functionality  
✨ Easy-to-use API for developers  

---

## Supported Platforms
- **Android**
- **iOS**

---

## Getting Started

### Prerequisites
- 🛠️ **.NET 8.0 or later**
- 🛠️ **.NET MAUI installed and configured**

### Installation
1. Add the `Plugin.Maui.ZoomView` package to your .NET MAUI project. You can do this via NuGet Package Manager or by running the following command:
   ```bash
   dotnet add package Plugin.Maui.ZoomView
   ```

2. Ensure your project is set up for .NET MAUI and includes the necessary platform-specific configurations.

3. After installing the package, call the `UseZoomView()` method in the `MauiProgram.cs` file of your project to initialize the plugin:
   ```csharp
   public static MauiApp CreateMauiApp()
   {
       var builder = MauiApp.CreateBuilder();
       builder
           .UseMauiApp<App>()
           .UseZoomView(); // Add this line

       return builder.Build();
   }
   ```

---

## Usage

1. Add the `ZoomView` control to your XAML file:
   ```xml
   <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                xmlns:zoomview="clr-namespace:Plugin.Maui.ZoomView;assembly=Plugin.Maui.ZoomView"
                x:Class="YourNamespace.MainPage">
       <zoomview:ZoomView>
           <!-- Add your content here -->
       </zoomview:ZoomView>
   </ContentPage>
   ```

2. Customize the `ZoomView` settings in your code-behind or XAML as needed.

---

## Sample Application
📂 A sample application demonstrating the usage of `Plugin.Maui.ZoomView` is available in the `samples/ZoomViewSample` directory. You can run the sample to see the plugin in action.

---

## Contributing
🤝 Contributions are welcome! If you encounter any issues or have suggestions for improvements, feel free to open an issue or submit a pull request.

---

## License
📜 This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

