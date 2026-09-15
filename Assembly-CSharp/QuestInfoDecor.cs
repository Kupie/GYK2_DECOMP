using System;
using UnityEngine;

// Token: 0x02000A1F RID: 2591
public class QuestInfoDecor : MonoBehaviour
{
	// Token: 0x060045A3 RID: 17827 RVA: 0x001493A0 File Offset: 0x001475A0
	public void DisableAll()
	{
		this.noCenter.gameObject.SetActive(false);
		this.centerDown.gameObject.SetActive(false);
		this.centerUp.gameObject.SetActive(false);
		this.centerUpAndDown.gameObject.SetActive(false);
	}

	// Token: 0x04003676 RID: 13942
	public GameObject noCenter;

	// Token: 0x04003677 RID: 13943
	public GameObject centerDown;

	// Token: 0x04003678 RID: 13944
	public GameObject centerUp;

	// Token: 0x04003679 RID: 13945
	public GameObject centerUpAndDown;
}
