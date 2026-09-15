using System;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000176 RID: 374
	public static class RectTransformExtension
	{
		// Token: 0x0600082D RID: 2093 RVA: 0x00028FD4 File Offset: 0x000271D4
		public static bool IsRectOutOfScreen(this RectTransform rectTransform, Bounds screenBounds)
		{
			Rect worldRect = rectTransform.GetWorldRect();
			return worldRect.xMax < screenBounds.min.x || worldRect.xMin > screenBounds.max.x || worldRect.yMin > screenBounds.max.y || worldRect.yMax < screenBounds.min.y;
		}
	}
}
