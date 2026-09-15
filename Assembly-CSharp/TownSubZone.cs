using System;
using UnityEngine;

// Token: 0x020001BC RID: 444
[RequireComponent(typeof(Collider))]
public class TownSubZone : MonoBehaviour
{
	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06000B3D RID: 2877 RVA: 0x000383D8 File Offset: 0x000365D8
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

	// Token: 0x04000C7B RID: 3195
	public string id;

	// Token: 0x04000C7C RID: 3196
	private Collider coll;
}
