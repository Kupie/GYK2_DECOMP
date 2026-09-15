using System;
using TMPro;
using UnityEngine;

// Token: 0x020009FE RID: 2558
public class UIMapWorldZone : MonoBehaviour
{
	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x060044F7 RID: 17655 RVA: 0x0014673B File Offset: 0x0014493B
	public string WorldZoneId
	{
		get
		{
			return this.worldZoneId;
		}
	}

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x060044F8 RID: 17656 RVA: 0x00146743 File Offset: 0x00144943
	public TextMeshProUGUI Label
	{
		get
		{
			return this.label;
		}
	}

	// Token: 0x040035CF RID: 13775
	[SerializeField]
	private string worldZoneId;

	// Token: 0x040035D0 RID: 13776
	[SerializeField]
	private TextMeshProUGUI label;
}
