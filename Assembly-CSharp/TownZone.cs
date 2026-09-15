using System;
using UnityEngine;

// Token: 0x020001BD RID: 445
[RequireComponent(typeof(Collider))]
public class TownZone : MonoBehaviour
{
	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06000B3F RID: 2879 RVA: 0x000383FA File Offset: 0x000365FA
	public Collider Collider
	{
		get
		{
			if (this.coll == null)
			{
				this.coll = base.GetComponent<Collider>();
			}
			return this.coll;
		}
	}

	// Token: 0x04000C7D RID: 3197
	private Collider coll;
}
