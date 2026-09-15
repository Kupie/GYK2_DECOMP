using System;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
[RequireComponent(typeof(SpriteRenderer))]
public class CachedSpriteRenderer : MonoBehaviour
{
	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x0600210F RID: 8463 RVA: 0x0009C3A7 File Offset: 0x0009A5A7
	public SpriteRenderer SpriteRenderer
	{
		get
		{
			if (!this.isSprrSet)
			{
				this.isSprrSet = true;
				this.sprr = base.GetComponent<SpriteRenderer>();
			}
			return this.sprr;
		}
	}

	// Token: 0x04001DAC RID: 7596
	private SpriteRenderer sprr;

	// Token: 0x04001DAD RID: 7597
	private bool isSprrSet;
}
