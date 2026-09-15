using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000107 RID: 263
	[AddComponentMenu("")]
	public class Bullet : MonoBehaviour
	{
		// Token: 0x06000CCC RID: 3276 RVA: 0x000251D0 File Offset: 0x000233D0
		private void Start()
		{
			if (this.lifeTime > 0f)
			{
				this.deathTime = Time.time + this.lifeTime;
				this.die = true;
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x000251F8 File Offset: 0x000233F8
		private void Update()
		{
			if (this.die && Time.time >= this.deathTime)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04000694 RID: 1684
		public float lifeTime = 3f;

		// Token: 0x04000695 RID: 1685
		private bool die;

		// Token: 0x04000696 RID: 1686
		private float deathTime;
	}
}
