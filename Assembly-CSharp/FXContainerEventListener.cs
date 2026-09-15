using System;
using UnityEngine;

// Token: 0x02000502 RID: 1282
public class FXContainerEventListener : MonoBehaviour
{
	// Token: 0x0600214B RID: 8523 RVA: 0x0009CFFA File Offset: 0x0009B1FA
	public void PlayFx(string fxName)
	{
		this.fxContainer.PlayFx(fxName);
	}

	// Token: 0x04001DDF RID: 7647
	[SerializeField]
	private FXContainer fxContainer;
}
