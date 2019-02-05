# Uno.UITests.Helpers

A set of Xamarin.UITest helpers for the UI Testsing of [Uno Platform](https://github.com/nventive/Uno) applications for iOS and Android.

WebAssembly testing is currently not supported.

## Usage

```csharp
[Test]
public void When_SmokeTest()
{
	var mainButton = _app.Find("mainButton");

	var position = mainButton.FirstResult().Rect;
	_app.TapCoordinates(position.X + 5, position.Y + 5);

	var mainButtonResults = _app.Find("mainButtonResults");

	_app.WaitForDependencyPropertyValue(mainButtonResults, "Text", "Pressed 1");
}
```

See this UI Tests [sample for more details](src/SamplesApp.UITests/Given_BasicSample.cs).

For Android and iOS, the `x:Name` marked controls in the XAML are automatically mapped to `contentDescription`
when [`<IsUiAutomationMappingEnabled>true</IsUiAutomationMappingEnabled>`](https://github.com/nventive/Uno.UITests.Helpers/blob/02428169342c12e9d9c09a011d2395ad6248387d/src/SamplesApp/SamplesApp.Droid/SamplesApp.Droid.csproj#L23) is 
set in both iOS and Android projects.