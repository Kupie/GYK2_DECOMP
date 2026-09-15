using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public static class FloatExtensions
{
	// Token: 0x06000095 RID: 149 RVA: 0x0000455D File Offset: 0x0000275D
	public static int Round05(this float value)
	{
		if (value % 0.5f == 0f)
		{
			return Mathf.CeilToInt(value);
		}
		return Mathf.RoundToInt(value);
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0000457A File Offset: 0x0000277A
	public static bool IsInteger(this float value)
	{
		return Math.Abs(value - Mathf.Round(value)) < 0.0001f;
	}
}
