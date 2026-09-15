using System;
using UnityEngine;

// Token: 0x020001BB RID: 443
[RequireComponent(typeof(Collider))]
public class MapZone : MonoBehaviour
{
	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000B3A RID: 2874 RVA: 0x000383AE File Offset: 0x000365AE
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000B3B RID: 2875 RVA: 0x000383B6 File Offset: 0x000365B6
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

	// Token: 0x04000C79 RID: 3193
	[SerializeField]
	private string id;

	// Token: 0x04000C7A RID: 3194
	private Collider coll;
}
