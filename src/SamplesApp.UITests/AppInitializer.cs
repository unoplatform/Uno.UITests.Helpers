using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xamarin.UITest;

namespace SamplesApp.UITests
{
	public class AppInitializer
	{
		public const string UITestPlatform = "UITEST_PLATFORM";
        public const string AppBundleId = "com.nventive.samplesapp.uitestshelpers";

		public static IApp StartApp()
		{
#if DEBUG
            // This is required to run the UI Test for android from Visual Studio using NUnit 3

            AppDomain.CurrentDomain.AssemblyResolve += (s, e) => {

                if (e.Name.Contains("nunit.framework, Version=2.6.4.14350, Culture=neutral, PublicKeyToken=96d09a1eb7f44a77"))
                {
                    var basePath = Path.GetDirectoryName(new Uri(typeof(AppInitializer).Assembly.CodeBase).LocalPath);
                    return Assembly.LoadFrom(Path.Combine(basePath, "nunit2.framework.dll"));
                }

                return null;
            };
#endif

            switch (TestEnvironment.Platform)
			{
				case TestPlatform.TestCloudiOS:
					return ConfigureApp
						.iOS
						.StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);

				case TestPlatform.TestCloudAndroid:
					return ConfigureApp
						.Android
						.StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);

				default:
					var uitestPlatform = Environment.GetEnvironmentVariable(UITestPlatform);
					if (!Enum.TryParse(uitestPlatform, out Platform retVal))
					{
						if (Environment.OSVersion.Platform != PlatformID.Unix)
						{
							retVal = Platform.Android;
						}
						else if (Environment.OSVersion.Platform == PlatformID.Unix)
						{
							retVal = Platform.iOS;
						}
						else
						{
							throw new Exception($"{UITestPlatform} environment variable {uitestPlatform} is unkown");
						}
					}

					switch(retVal)
					{
						case Platform.Android:
							return ConfigureApp
								.Android
								.Debug()
								.EnableLocalScreenshots()
								.InstalledApp(AppBundleId)
								.StartApp();

						case Platform.iOS:
							return ConfigureApp
								.iOS
								.Debug()
								.EnableLocalScreenshots()
								.InstalledApp(AppBundleId)
								.StartApp();

						default:
							throw new Exception($"Platform {retVal} is not enabled.");
					}
			}
		}
	}
}
