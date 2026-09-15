using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000125 RID: 293
	public class NavigationStick
	{
		// Token: 0x060005D4 RID: 1492 RVA: 0x0001D5BF File Offset: 0x0001B7BF
		public NavigationStick(bool vertical, GamepadController controller)
		{
			this.vertical = vertical;
			this.controller = controller;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001D5D8 File Offset: 0x0001B7D8
		public void Update(Vector2 dir)
		{
			float num = (this.vertical ? dir.y : dir.x);
			float num2 = Mathf.Abs(num);
			if (num2 <= 0.4f)
			{
				this.minValue = (this.maxValue = 0f);
				this.waitForPress = false;
				return;
			}
			GameKey key = this.GetKey(num);
			if (this.maxValue.EqualsTo(0f, 1E-05f))
			{
				this.maxValue = num2;
				this.minValue = this.maxValue - 0.2f;
				this.waitForPress = false;
				this.controller.HandlePressing(key);
				this.controller.HandleHolding(key);
				return;
			}
			if (num2 < this.minValue)
			{
				this.waitForPress = true;
				this.minValue = num2;
				this.maxValue = this.minValue + 0.2f;
				return;
			}
			if (num2 < this.minValue + 0.1f)
			{
				return;
			}
			if (this.waitForPress)
			{
				this.controller.HandlePressing(key);
				this.controller.HandleHolding(key);
			}
			if (num2 > this.minValue + 0.2f)
			{
				this.maxValue = num2;
				this.minValue = this.maxValue - 0.2f;
			}
			if (this.waitForPress)
			{
				this.waitForPress = false;
				return;
			}
			this.controller.HandleHolding(key);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0001D71D File Offset: 0x0001B91D
		private GameKey GetKey(float value)
		{
			if (!this.vertical)
			{
				if (value <= 0f)
				{
					return GameKey.Left;
				}
				return GameKey.Right;
			}
			else
			{
				if (value <= 0f)
				{
					return GameKey.Down;
				}
				return GameKey.Up;
			}
		}

		// Token: 0x040002D1 RID: 721
		private const float MIN_DEAD_ZONE = 0.4f;

		// Token: 0x040002D2 RID: 722
		private const float DELTA = 0.2f;

		// Token: 0x040002D3 RID: 723
		private bool vertical;

		// Token: 0x040002D4 RID: 724
		private GamepadController controller;

		// Token: 0x040002D5 RID: 725
		private float maxValue;

		// Token: 0x040002D6 RID: 726
		private float minValue;

		// Token: 0x040002D7 RID: 727
		private bool waitForPress;
	}
}
