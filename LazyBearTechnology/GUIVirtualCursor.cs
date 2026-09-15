using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000163 RID: 355
	public class GUIVirtualCursor : BaseVirtualCursor
	{
		// Token: 0x060007A4 RID: 1956 RVA: 0x000270CE File Offset: 0x000252CE
		public override void CalculateBounds()
		{
			this.minCoords = Vector2.zero;
			this.maxCoords.x = (float)Screen.width;
			this.maxCoords.y = (float)Screen.height;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x000270FD File Offset: 0x000252FD
		protected override float GetSpeed()
		{
			return this.currentSpeed * LazyUI.ScaleFactor * Time.deltaTime;
		}
	}
}
