using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000124 RID: 292
	public abstract class BaseInputController
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001D53F File Offset: 0x0001B73F
		public Vector2 Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0001D547 File Offset: 0x0001B747
		public Vector2 Direction2
		{
			get
			{
				return this.direction2;
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001D54F File Offset: 0x0001B74F
		public virtual void Update()
		{
			this.holdedKeys.Clear();
			this.pressedKeys.Clear();
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0001D567 File Offset: 0x0001B767
		public virtual bool IsActive()
		{
			return this.holdedKeys.Count > 0 || this.direction.magnitude > 0f;
		}

		// Token: 0x040002CD RID: 717
		public List<GameKey> holdedKeys = new List<GameKey>();

		// Token: 0x040002CE RID: 718
		public List<GameKey> pressedKeys = new List<GameKey>();

		// Token: 0x040002CF RID: 719
		protected Vector2 direction = Vector2.zero;

		// Token: 0x040002D0 RID: 720
		protected Vector2 direction2 = Vector2.zero;
	}
}
