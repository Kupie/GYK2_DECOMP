using System;
using Rewired;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000126 RID: 294
	public class Stick
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001D74E File Offset: 0x0001B94E
		public bool HasDirection
		{
			get
			{
				return this.direction.magnitude > 0f;
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001D762 File Offset: 0x0001B962
		public Stick(int horizontalAxisId, int verticalAxisId)
		{
			this.horizontalAxisId = horizontalAxisId;
			this.verticalAxisId = verticalAxisId;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001D778 File Offset: 0x0001B978
		public void Update(Player player)
		{
			this.currentH = player.GetAxis(this.horizontalAxisId);
			this.currentV = player.GetAxis(this.verticalAxisId);
			this.direction = new Vector2(this.currentH, this.currentV);
			if (this.direction.magnitude < 0.1f)
			{
				this.direction = Vector2.zero;
				this.currentH = (this.currentV = 0f);
			}
			this.stickDelay -= Time.deltaTime;
			if (this.direction.magnitude > 0f)
			{
				if (((this.prevH < 0f && this.currentH > 0f) || (this.prevH > 0f && this.currentH < 0f)) && ((this.prevV < 0f && this.currentV > 0f) || (this.prevV > 0f && this.currentV < 0f)))
				{
					this.stickDelay = 0.05f;
				}
			}
			else
			{
				this.stickDelay = 0f;
			}
			if (this.stickDelay > 0f && this.direction.magnitude < 0.35f)
			{
				this.direction = Vector2.zero;
				this.currentH = (this.currentV = 0f);
			}
			this.prevH = this.direction.x;
			this.prevV = this.direction.y;
		}

		// Token: 0x040002D8 RID: 728
		private const float NEW_DIR_DELAY = 0.05f;

		// Token: 0x040002D9 RID: 729
		private const float MIN_MAGNITUDE = 0.1f;

		// Token: 0x040002DA RID: 730
		private const float OPPOSITE_DIR_MIN_MAGNITUDE = 0.35f;

		// Token: 0x040002DB RID: 731
		public Vector2 direction;

		// Token: 0x040002DC RID: 732
		private int horizontalAxisId;

		// Token: 0x040002DD RID: 733
		private int verticalAxisId;

		// Token: 0x040002DE RID: 734
		private float prevH;

		// Token: 0x040002DF RID: 735
		private float prevV;

		// Token: 0x040002E0 RID: 736
		private float currentH;

		// Token: 0x040002E1 RID: 737
		private float currentV;

		// Token: 0x040002E2 RID: 738
		private float stickDelay;
	}
}
