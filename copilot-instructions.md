# Copilot Instructions for Plugin.Maui.ZoomView

## Project Overview

This is a **.NET MAUI plugin** that provides a customizable **ZoomView** control for cross-platform mobile applications. The plugin enables developers to easily add zoom and pan functionality to their MAUI apps, supporting **Android** and **iOS** platforms.

### Key Purpose
- Provide a reusable zoomable container for any MAUI View content
- Support pinch-to-zoom, pan gestures, and double-tap zoom behaviors
- Follow MAUI plugin patterns for cross-platform compatibility

## Architecture & Components

### Core Architecture
The plugin follows the **MAUI Handler pattern** for cross-platform implementation:

```
ZoomView (Public API)
    ↓
IZoomView (Interface)
    ↓
ZoomViewHandler (Cross-platform handler)
    ↓
Platform-specific implementations (Android/iOS)
```

### Key Files Structure
```
src/Plugin.Maui.ZoomView/
├── ZoomView.cs                     # Main control with bindable properties
├── IZoomView.cs                    # Interface defining the contract
├── ZoomViewHandler.cs              # Cross-platform handler registration
├── ZoomViewHandler.net.cs          # Fallback handler for unsupported platforms
├── PlatformZoomView.net.cs         # Fallback platform view
└── Platforms/
    ├── Android/
    │   ├── PlatformZoomView.cs     # Android-specific implementation
    │   └── ZoomViewHandler.cs      # Android handler
    └── iOS/
        ├── PlatformZoomView.cs     # iOS-specific implementation
        └── ZoomViewHandler.cs      # iOS handler
```

## Core Components

### 1. ZoomView Control (`ZoomView.cs`)
The main public API that developers use. Key features:
- **Content property**: Accepts any `View` as zoomable content
- **Bindable properties** for behavior configuration
- **Reset() method** to restore initial zoom/pan state
- **ContentProperty attribute** for XAML content support

### 2. Bindable Properties
| Property | Type | Default | Purpose |
|----------|------|---------|---------|
| `Content` | `View` | `null` | The content to be displayed inside the zoomable area |
| `ZoomInOnDoubleTap` | `bool` | `true` | Enable double-tap to zoom in when at default scale |
| `ZoomOutOnDoubleTap` | `bool` | `true` | Enable double-tap to reset zoom when zoomed in |
| `Zoom` | `float` | `1.0f` | Internal property tracking current zoom level |

### 3. Platform Handlers
- **Android**: Uses native scroll/zoom gestures with matrix transformations
- **iOS**: Leverages UIScrollView with zoom capabilities
- **Fallback**: No-op implementations for unsupported platforms

## Development Patterns

### Adding New Properties
When adding bindable properties, follow this pattern:
```csharp
public static readonly BindableProperty NewPropertyProperty =
    BindableProperty.Create(nameof(NewProperty), typeof(PropertyType), typeof(ZoomView), defaultValue);

public PropertyType NewProperty
{
    get => (PropertyType)GetValue(NewPropertyProperty);
    set => SetValue(NewPropertyProperty, value);
}
```

### Platform-Specific Implementation
1. **Update IZoomView interface** if adding new functionality
2. **Implement in both Android and iOS handlers**
3. **Add fallback in .net.cs files** for unsupported platforms
4. **Update the main ZoomView class** to expose the API

### Handler Invocation Pattern
For methods that need platform implementation:
```csharp
public void NewMethod()
{
    Handler?.Invoke(nameof(IZoomView.NewMethod), EventArgs.Empty);
}
```

## Usage Patterns

### Basic XAML Usage
```xml
<ContentPage xmlns:zoomview="clr-namespace:Plugin.Maui.ZoomView;assembly=Plugin.Maui.ZoomView">
    <zoomview:ZoomView>
        <Image Source="myimage.jpg" />
    </zoomview:ZoomView>
</ContentPage>
```

### Registration in MauiProgram.cs
```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseZoomView();  // Extension method registration
    
    return builder.Build();
}
```

## Platform Considerations

### Android
- Uses Matrix transformations for zoom/pan
- Handles touch events through gesture detection
- May need to consider different screen densities

### iOS
- Leverages UIScrollView's built-in zoom capabilities
- More native iOS zoom behavior
- Handles delegate methods for zoom events

### Content Compatibility
- **Best for**: Static content like Images, Labels, custom views
- **Caution with**: Interactive controls (Entry, Editor, Button) may not behave reliably during zoom/pan operations

## Extension Points

### Adding New Platforms
1. Create new platform folder: `Platforms/{Platform}/`
2. Implement `PlatformZoomView.cs` with native view
3. Implement `ZoomViewHandler.cs` with platform-specific logic
4. Update project file with conditional compilation

### Custom Behaviors
- Override handlers for custom zoom behavior
- Extend ZoomView class for additional properties
- Create platform-specific custom renderers if needed

## Testing Approach

### Sample App
The `samples/ZoomViewSample/` contains a working example:
- Demonstrates basic usage
- Tests different content types
- Validates platform-specific behavior

### Key Test Scenarios
- Zoom in/out with pinch gestures
- Pan around zoomed content
- Double-tap zoom behaviors
- Reset functionality
- Content binding and updates

## Common Code Patterns

### Handler Registration
```csharp
builder.ConfigureMauiHandlers(handlers =>
{
    handlers.AddHandler<IZoomView, ZoomViewHandler>();
});
```

### Conditional Platform Code
```csharp
#if ANDROID
    // Android-specific code
#elif IOS
    // iOS-specific code
#else
    // Fallback or shared code
#endif
```

### Event Handling
```csharp
Handler?.Invoke(nameof(IZoomView.MethodName), new EventArgs());
```

## Dependencies & Framework

- **Target Framework**: .NET 9.0
- **Platforms**: net9.0-android, net9.0-ios, net9.0 (fallback)
- **Main Dependency**: Microsoft.Maui.Controls
- **Package Type**: NuGet plugin package

## Best Practices

1. **Minimal API Surface**: Keep public API simple and focused
2. **Platform Parity**: Ensure consistent behavior across platforms
3. **Performance**: Optimize for smooth zoom/pan operations
4. **Documentation**: Maintain XML documentation for public APIs
5. **Backward Compatibility**: Consider impact of changes on existing users

## Common Issues & Solutions

- **Build Errors**: May occur if MAUI workloads not installed
- **Platform Support**: Graceful fallback for unsupported platforms
- **Content Sizing**: Ensure content properly measures within zoom container
- **Memory Management**: Dispose of platform resources appropriately