using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000FD RID: 253
	public class RandomCoordinate : MonoBehaviour
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x00017E90 File Offset: 0x00016090
		private void Update()
		{
			float num = Mathf.Min(Time.deltaTime, 0.05f);
			this.time += num;
			if (this.time > this.rollPeriodSec)
			{
				while (this.time > this.rollPeriodSec)
				{
					this.time -= this.rollPeriodSec;
				}
				this.curTarget = new Vector3(global::UnityEngine.Random.Range(-this.random.x, this.random.x), global::UnityEngine.Random.Range(-this.random.y, this.random.y), global::UnityEngine.Random.Range(-this.random.z, this.random.z));
			}
			if (this.tr == null)
			{
				this.tr = base.transform;
			}
			float num2 = Mathf.Min(1f, this.speed * num);
			if (this.animateZ)
			{
				Vector3 vector = this.curTarget - this.tr.localPosition;
				this.tr.localPosition += vector * num2;
				return;
			}
			Vector2 vector2 = this.curTarget - this.tr.localPosition;
			if (vector2.x > 10000f)
			{
				vector2.x = 0f;
			}
			if (vector2.y > 10000f)
			{
				vector2.y = 0f;
			}
			this.tr.localPosition += vector2 * num2;
		}

		// Token: 0x04000233 RID: 563
		public Vector3 random = Vector3.zero;

		// Token: 0x04000234 RID: 564
		public float speed = 1f;

		// Token: 0x04000235 RID: 565
		public float rollPeriodSec = 0.05f;

		// Token: 0x04000236 RID: 566
		public bool animateZ;

		// Token: 0x04000237 RID: 567
		private Vector3 curTarget = Vector3.zero;

		// Token: 0x04000238 RID: 568
		private float time;

		// Token: 0x04000239 RID: 569
		private Transform tr;
	}
}
