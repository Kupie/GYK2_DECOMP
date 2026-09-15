using System;
using UnityEngine;

// Token: 0x02000AB5 RID: 2741
public static class D
{
	// Token: 0x06004A09 RID: 18953 RVA: 0x0015DACB File Offset: 0x0015BCCB
	public static void Log(string log)
	{
		Debug.Log("#inner# " + log);
	}

	// Token: 0x06004A0A RID: 18954 RVA: 0x0015DADD File Offset: 0x0015BCDD
	public static void LogWarning(string log)
	{
		Debug.LogWarning(log);
	}

	// Token: 0x06004A0B RID: 18955 RVA: 0x0015DAE5 File Offset: 0x0015BCE5
	public static void LogError(string log)
	{
		Debug.LogError(log);
	}

	// Token: 0x06004A0C RID: 18956 RVA: 0x0015DAED File Offset: 0x0015BCED
	public static void LogColor(string log, string color)
	{
		Debug.Log(log);
	}

	// Token: 0x06004A0D RID: 18957 RVA: 0x0015DAF5 File Offset: 0x0015BCF5
	public static void LogColor(string log, string color, global::UnityEngine.Object context)
	{
		Debug.Log(log, context);
	}
}
