using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using static UITests.Queries.Helpers;
using Platform = Xamarin.UITest.Platform;

namespace UITests.Helpers
{
	public static class PlatformHelpers
	{
		public static void On(Action iOS, Action Android)
		{
			switch (UITests.Queries.Helpers.Platform)
			{
				case Platform.Android:
					Android();
					break;
				case Platform.iOS:
					iOS();
					break;
				default: throw new ArgumentOutOfRangeException();
			}
		}

		public static T On<T>(Func<T> iOS, Func<T> Android)
		{
			switch (UITests.Queries.Helpers.Platform)
			{
				case Platform.Android:
					return Android();
				case Platform.iOS:
					return iOS();
				default: throw new ArgumentOutOfRangeException();
			}
		}
	}
}
