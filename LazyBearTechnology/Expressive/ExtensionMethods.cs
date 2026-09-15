using System;
using System.Collections.Generic;
using System.Globalization;

namespace Expressive
{
	// Token: 0x02000030 RID: 48
	internal static class ExtensionMethods
	{
		// Token: 0x060000FB RID: 251 RVA: 0x00006E40 File Offset: 0x00005040
		internal static bool IsArithmeticOperator(this string source)
		{
			return string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "-", StringComparison.Ordinal) || string.Equals(source, "−", StringComparison.Ordinal) || string.Equals(source, "/", StringComparison.Ordinal) || string.Equals(source, "÷", StringComparison.Ordinal) || string.Equals(source, "*", StringComparison.Ordinal) || string.Equals(source, "×", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal) || string.Equals(source, "+", StringComparison.Ordinal);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006EDC File Offset: 0x000050DC
		internal static bool IsNumeric(this string source, CultureInfo cultureInfo)
		{
			double num;
			return double.TryParse(source, NumberStyles.Any, cultureInfo, out num);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006EF8 File Offset: 0x000050F8
		internal static T PeekOrDefault<T>(this Queue<T> queue)
		{
			if (queue.Count <= 0)
			{
				return default(T);
			}
			return queue.Peek();
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006F20 File Offset: 0x00005120
		internal static string SubstringUpTo(this string source, int startIndex, char character)
		{
			if (startIndex != 0)
			{
				string text = source.Substring(startIndex);
				return text.Substring(0, text.IndexOf(character) + 1);
			}
			return source.Substring(startIndex, source.IndexOf(character) + 1);
		}
	}
}
