using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public static class ComparisonExtensions
{
	// Token: 0x0600008F RID: 143 RVA: 0x000044DA File Offset: 0x000026DA
	public static bool EqualsTo(this float a, float b, float epsilon = 1E-05f)
	{
		return Mathf.Abs(a - b) < epsilon;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000044E7 File Offset: 0x000026E7
	public static bool EqualsOrMore(this float a, float b, float epsilon = 1E-05f)
	{
		return Mathf.Abs(a - b) < epsilon || a > b;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x000044FA File Offset: 0x000026FA
	public static bool EqualsOrLess(this float a, float b, float epsilon = 1E-05f)
	{
		return Mathf.Abs(a - b) < epsilon || a < b;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0000450D File Offset: 0x0000270D
	public static bool More(this float a, float b, float epsilon = 1E-05f)
	{
		return Mathf.Abs(a - b) >= epsilon && a > b;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00004520 File Offset: 0x00002720
	public static bool Less(this float a, float b, float epsilon = 1E-05f)
	{
		return Mathf.Abs(a - b) >= epsilon && a < b;
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00004533 File Offset: 0x00002733
	public static bool EqualsTo(this Vector2 a, Vector2 b, float epsilon = 1E-05f)
	{
		return a.x.EqualsTo(b.x, epsilon) && a.y.EqualsTo(b.y, epsilon);
	}
}
