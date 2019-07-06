using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateApp.DAL.Extensions
{
	public static class Extensions
	{
		public static DateTime? ToUtc(this DateTime? date)
		{
			if (date.HasValue)
			{
				return date.Value.ToUniversalTime();
			}

			return null;
		}

		public static DateTime? ToLocalTime(this DateTime? date)
		{
			if (date.HasValue)
			{
				return date.Value.ToLocalTime();
			}

			return null;
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

	}
}
