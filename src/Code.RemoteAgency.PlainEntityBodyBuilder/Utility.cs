using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
	internal static class Utility
	{
		public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			var index = 0;
			foreach (var item in source)
			{
				if (predicate(item))
				{
					return index;
				}

				index++;
			}

			return -1;
		}

	}
}
