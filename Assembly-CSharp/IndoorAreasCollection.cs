using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class IndoorAreasCollection : MonoBehaviour
{
	// Token: 0x06000ABE RID: 2750 RVA: 0x00027874 File Offset: 0x00025A74
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000C35 RID: 3125
	[SerializeField]
	private List<IndoorArea> indoorAreas = new List<IndoorArea>();
}
