using System;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class DecoyComponent : MonoBehaviour
{
	// Token: 0x06001296 RID: 4758 RVA: 0x0005BEE4 File Offset: 0x0005A0E4
	public void Initialize(Wgo wgo)
	{
		this.attachedWgo = wgo;
		this.attachedWgo.AttackPriority = 20;
		wgo.Data.HpComponent.IsImmuneToDamage = true;
	}

	// Token: 0x0400142D RID: 5165
	private Wgo attachedWgo;

	// Token: 0x0400142E RID: 5166
	[Range(0f, 10f)]
	public float enemyRetargetRange = 5f;
}
