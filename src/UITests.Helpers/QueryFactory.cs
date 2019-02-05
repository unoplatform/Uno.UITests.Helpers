using UITests.Queries;
using Xamarin.UITest;

namespace UITests.Helpers
{
	internal static class QueryExFactory
	{
		public static QueryEx BlankQuery()
		{
			return new QueryEx(q => q);
		}
	}
}
