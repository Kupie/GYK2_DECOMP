using System;
using UnityEngine;

// Token: 0x02000A49 RID: 2633
[DisallowMultipleComponent]
public class UITutorialWindowPageHeaderOverride : MonoBehaviour
{
	// Token: 0x17000ACA RID: 2762
	// (get) Token: 0x06004701 RID: 18177 RVA: 0x0014FD63 File Offset: 0x0014DF63
	public string HeaderLocaleId
	{
		get
		{
			return this.headerLocaleId;
		}
	}

	// Token: 0x04003760 RID: 14176
	[SerializeField]
	private string headerLocaleId;
}
