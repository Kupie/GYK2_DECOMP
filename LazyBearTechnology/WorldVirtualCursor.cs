using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000164 RID: 356
	public class WorldVirtualCursor : BaseVirtualCursor
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00027119 File Offset: 0x00025319
		protected Vector2 screenVector
		{
			get
			{
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002712C File Offset: 0x0002532C
		public override void CalculateBounds()
		{
			this.minCoords = this.ScreenToWorld(Vector2.zero);
			this.maxCoords = this.ScreenToWorld(this.screenVector);
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00027151 File Offset: 0x00025351
		protected virtual Vector2 ScreenToWorld(Vector2 point)
		{
			return Camera.main.ScreenToWorldPoint(point);
		}
	}
}
