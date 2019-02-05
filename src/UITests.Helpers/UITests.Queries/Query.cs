using System;
using System.Runtime.CompilerServices;
using Xamarin.UITest.Queries;

namespace UITests.Queries
{
	public class QueryEx
	{
		public static readonly QueryEx Any = new QueryEx((AppQuery x) => x.All(null));

		public static readonly QueryEx Visible = new QueryEx((AppQuery x) => x);

		public static readonly QueryEx Button = QueryEx.Visible.Compose((AppQuery x) => x.Button(null));

		public static readonly QueryEx Switch = QueryEx.Visible.Compose((AppQuery x) => x.Switch(null));

		public static readonly QueryEx WebView = QueryEx.Visible.Compose((AppQuery x) => x.WebView());

		public readonly Func<AppQuery, AppQuery> Unwrap;

		public static QueryEx Label
		{
			[CompilerGenerated]
			get
			{
				return Helpers.On<QueryEx>(QueryEx.Visible[".UILabel"], QueryEx.Visible[".android.widget.TextView"]);
			}
		}

		[Obsolete("Use Entry instead.")]
		public static QueryEx TextField
		{
			[CompilerGenerated]
			get
			{
				return QueryEx.Entry;
			}
		}

		public static QueryEx Entry
		{
			[CompilerGenerated]
			get
			{
				return QueryEx.Visible.Compose((AppQuery x) => x.TextField(null));
			}
		}

		public static QueryEx Table
		{
			[CompilerGenerated]
			get
			{
				return Helpers.On<QueryEx>(QueryEx.Visible[".UITableView"], QueryEx.Visible[".android.widget.ListView"]);
			}
		}

		public static QueryEx Cell
		{
			[CompilerGenerated]
			get
			{
				return Helpers.On<QueryEx>(QueryEx.Table.Descendant("UITableViewCell"), QueryEx.Table.Child);
			}
		}

		public QueryEx Sibling
		{
			[CompilerGenerated]
			get
			{
				return this.Compose((AppQuery x) => x.Sibling(null));
			}
		}

		public QueryEx Child
		{
			[CompilerGenerated]
			get
			{
				return this.Compose((AppQuery x) => x.Child(null));
			}
		}

		public QueryEx Parent
		{
			[CompilerGenerated]
			get
			{
				return this.Compose((AppQuery x) => x.Parent(null));
			}
		}

		public QueryEx this[string marked]
		{
			[CompilerGenerated]
			get
			{
				return this.MarkedOrPrefixedOverloaded(marked);
			}
		}

		public QueryEx this[int index]
		{
			[CompilerGenerated]
			get
			{
				return this.AtIndex(index);
			}
		}

		public QueryEx(Func<AppQuery, AppQuery> query)
		{
			this.Unwrap = query;
		}

		public static implicit operator QueryEx(string mark)
		{
			return QueryEx.Visible[mark];
		}

		public static implicit operator Func<AppQuery, AppQuery>(QueryEx query)
		{
			return query.Unwrap;
		}

		private QueryEx Compose(Func<AppQuery, AppQuery> q)
		{
			return new QueryEx((AppQuery x) => q(this.Unwrap(x)));
		}

		public QueryEx WithClass(string @class)
		{
			return this.Compose((AppQuery x) => x.Class(@class));
		}

		public QueryEx Marked(string mark)
		{
			return this.Compose((AppQuery x) => x.Marked(mark));
		}

		public QueryEx WithId(string id)
		{
			return this.Compose((AppQuery x) => x.Id(id));
		}

		public QueryEx Descendant(string s)
		{
			return this.Compose((AppQuery x) => x.Descendant(s));
		}

		public QueryEx Descendant(int i)
		{
			return this.Compose((AppQuery x) => x.Descendant(i));
		}

		public QueryEx Descendant()
		{
			return this.Compose((AppQuery x) => x.Descendant(null));
		}

		public QueryEx Raw(string raw)
		{
			return this.Compose((AppQuery x) => x.Raw(raw));
		}

		public QueryEx WithPlaceholder(string placeholder)
		{
			return Helpers.On<QueryEx>(this.Raw(string.Format("* {{placeholder LIKE '{0}'}}", placeholder)), this.Raw(string.Format("* {{hint LIKE '{0}'}}", placeholder)));
		}

		public QueryEx WithExactText(string text)
		{
			return this.Compose((AppQuery x) => x.Text(text));
		}

		public QueryEx WithText(string text)
		{
			string arg = text.Replace("'", "\\'");
			return this.Descendant().Raw(string.Format("* {{text contains '{0}'}}", arg));
		}

		public QueryEx AtIndex(int i)
		{
			return this.Compose((AppQuery x) => x.Index(i));
		}

		private QueryEx MarkedOrPrefixedOverloaded(string marked)
		{
			if (string.IsNullOrEmpty(marked))
			{
				throw new ArgumentException("Empty or null string", "marked");
			}
			if (marked.Length == 1)
			{
				return this.Marked(marked);
			}
			string text = marked.Substring(0, 1);
			string text2 = marked.Substring(1).Trim();
			if (text != null)
			{
				if (text == "#")
				{
					return this.WithId(text2);
				}
				if (text == ".")
				{
					return this.WithClass(text2);
				}
				if (text == "~")
				{
					return this.WithText(text2);
				}
			}
			return this.Marked(marked);
		}
	}
}
