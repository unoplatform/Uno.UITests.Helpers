using NUnit.Framework;
using System;
using UITests.Helpers;
using UITests.Queries;
using Xamarin.UITest;

namespace SamplesApp.UITests
{
    [TestFixture]
    public class Given_BasicSample
    {
		protected IApp _app;

        public Given_BasicSample()
        {
            _app = AppInitializer.StartApp();
            _app.SetOrientationLandscape();

            global::UITests.Queries.Helpers.App = _app;
        }

        [Test]
        public void When_SmokeTest()
        {
            // If your app fails to find the button on Android, make sure to add 
            // <IsUiAutomationMappingEnabled>true</IsUiAutomationMappingEnabled>
            // in the Android project.
            var mainButton = _app.Find("mainButton");

            var position = mainButton.FirstResult().Rect;
            _app.TapCoordinates(position.X + 5, position.Y + 5);

            var mainButtonResults = _app.Find("mainButtonResults");

            _app.WaitForDependencyPropertyValue(mainButtonResults, "Text", "Pressed 1");
        }
    }
}
