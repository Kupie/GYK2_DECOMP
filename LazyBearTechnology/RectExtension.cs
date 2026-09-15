using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000175 RID: 373
	public static class RectExtension
	{
		// Token: 0x0600082B RID: 2091 RVA: 0x00028F40 File Offset: 0x00027140
		public static bool LCS_Overlaps(this Rect rect, Rect other)
		{
			return other.xMax > rect.xMin && other.xMin < rect.xMax && other.yMin - other.height < rect.yMin && other.yMin > rect.yMin - rect.height;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00028FA0 File Offset: 0x000271A0
		public static bool LCS_IsOverlapsWithAnyOthers(this Rect rect, Rect[] others)
		{
			foreach (Rect rect2 in others)
			{
				if (rect.LCS_Overlaps(rect2))
				{
					return true;
				}
			}
			return false;
		}
	}
}
