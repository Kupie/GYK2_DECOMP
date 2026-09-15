using System;
using System.Collections.Generic;

namespace Expressive.Helpers
{
	// Token: 0x02000054 RID: 84
	public static class Comparison
	{
		// Token: 0x0600019D RID: 413 RVA: 0x00007C30 File Offset: 0x00005E30
		public static int CompareUsingMostPreciseType(object lhs, object rhs, Context context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			Type mostPreciseType = Comparison.GetMostPreciseType((lhs != null) ? lhs.GetType() : null, (rhs != null) ? rhs.GetType() : null);
			if (mostPreciseType == typeof(string))
			{
				return string.Compare((string)Convert.ChangeType(lhs, mostPreciseType, context.CurrentCulture), (string)Convert.ChangeType(rhs, mostPreciseType, context.CurrentCulture), context.EqualityStringComparison);
			}
			return Comparison.Compare(lhs, rhs, mostPreciseType, context);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00007CB4 File Offset: 0x00005EB4
		private static Type GetMostPreciseType(Type a, Type b)
		{
			if (a == b)
			{
				return a;
			}
			foreach (Type type in Comparison.CommonTypes)
			{
				if (a == type || b == type)
				{
					return type;
				}
			}
			return a;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00007CFC File Offset: 0x00005EFC
		private static int Compare(object lhs, object rhs, Type mostPreciseType, Context context)
		{
			if (lhs == null && rhs == null)
			{
				return 0;
			}
			if (lhs == null)
			{
				return -1;
			}
			if (rhs == null)
			{
				return 1;
			}
			Type type = lhs.GetType();
			Type type2 = rhs.GetType();
			if (type == type2)
			{
				return Comparer<object>.Default.Compare(lhs, rhs);
			}
			try
			{
				if (type == mostPreciseType)
				{
					rhs = Convert.ChangeType(rhs, mostPreciseType, context.CurrentCulture);
				}
				else
				{
					lhs = Convert.ChangeType(lhs, mostPreciseType, context.CurrentCulture);
				}
				return Comparer<object>.Default.Compare(lhs, rhs);
			}
			catch (Exception)
			{
			}
			try
			{
				return Comparer<object>.Default.Compare(lhs, Convert.ChangeType(rhs, type, context.CurrentCulture));
			}
			catch (Exception)
			{
			}
			try
			{
				return Comparer<object>.Default.Compare(lhs, Convert.ChangeType(rhs, type, context.CurrentCulture));
			}
			catch (Exception)
			{
			}
			try
			{
				return string.Compare((string)Convert.ChangeType(lhs, typeof(string), context.CurrentCulture), (string)Convert.ChangeType(rhs, typeof(string), context.CurrentCulture), context.EqualityStringComparison);
			}
			catch (Exception)
			{
			}
			return 0;
		}

		// Token: 0x040000B2 RID: 178
		private static readonly Type[] CommonTypes = new Type[]
		{
			typeof(DateTime),
			typeof(decimal),
			typeof(double),
			typeof(long),
			typeof(int),
			typeof(bool),
			typeof(string)
		};
	}
}
