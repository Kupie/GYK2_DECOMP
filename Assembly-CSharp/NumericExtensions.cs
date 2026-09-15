using System;
using System.Globalization;

// Token: 0x02000ACC RID: 2764
public static class NumericExtensions
{
	// Token: 0x06004AA3 RID: 19107 RVA: 0x00160520 File Offset: 0x0015E720
	public static int ToInt32Invariant(this string value, int defaultValue = 0)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return defaultValue;
		}
		int num;
		if (!int.TryParse(value.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
		{
			return defaultValue;
		}
		return num;
	}

	// Token: 0x06004AA4 RID: 19108 RVA: 0x0016054F File Offset: 0x0015E74F
	public static string ToInvariantCultureString(this int value)
	{
		return value.ToString(CultureInfo.InvariantCulture);
	}

	// Token: 0x06004AA5 RID: 19109 RVA: 0x0016055D File Offset: 0x0015E75D
	public static string ToInvariantCultureString(this float value)
	{
		return value.ToString(CultureInfo.InvariantCulture);
	}
}
