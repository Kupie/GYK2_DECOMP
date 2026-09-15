using System;
using UnityEngine;

// Token: 0x02000B0C RID: 2828
public class TriggerColliderComponentLinker : MonoBehaviour
{
	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x06004B4F RID: 19279 RVA: 0x00163DFB File Offset: 0x00161FFB
	// (set) Token: 0x06004B50 RID: 19280 RVA: 0x00163E03 File Offset: 0x00162003
	public MonoBehaviour Component
	{
		get
		{
			return this.component;
		}
		set
		{
			this.component = value;
		}
	}

	// Token: 0x04003CB0 RID: 15536
	[SerializeField]
	private MonoBehaviour component;
}
