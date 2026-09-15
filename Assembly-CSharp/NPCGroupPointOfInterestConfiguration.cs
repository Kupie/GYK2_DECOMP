using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AF RID: 431
[Serializable]
public class NPCGroupPointOfInterestConfiguration
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00036C00 File Offset: 0x00034E00
	public List<string> AllPonts
	{
		get
		{
			return this.pointsOfInterest;
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00036C08 File Offset: 0x00034E08
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x04000C52 RID: 3154
	[SerializeField]
	private string id;

	// Token: 0x04000C53 RID: 3155
	[SerializeField]
	private List<string> pointsOfInterest;
}
