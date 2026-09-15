using System;
using System.Text;
using UnityEngine;

// Token: 0x0200001F RID: 31
public static class ColorExtensions
{
	// Token: 0x0600008E RID: 142 RVA: 0x00004464 File Offset: 0x00002664
	public static string ToHex(this Color32 color, bool useAlpha = false)
	{
		StringBuilder stringBuilder = new StringBuilder().Append(color.r.ToString("X2")).Append(color.g.ToString("X2")).Append(color.b.ToString("X2"));
		if (useAlpha)
		{
			stringBuilder.Append(color.a.ToString("X2"));
		}
		return stringBuilder.ToString();
	}
}
