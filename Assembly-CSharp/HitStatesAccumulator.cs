using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class HitStatesAccumulator : MonoBehaviour
{
	// Token: 0x0600164D RID: 5709 RVA: 0x0006B8A5 File Offset: 0x00069AA5
	public void Clear()
	{
		this.hitStates.Clear();
		this.hitColliders.Clear();
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x0006B8BD File Offset: 0x00069ABD
	private void OnEnable()
	{
		this.Clear();
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x0006B8BD File Offset: 0x00069ABD
	private void OnDisable()
	{
		this.Clear();
	}

	// Token: 0x040016A1 RID: 5793
	public Dictionary<ICombatEntity, WeaponHitState> hitStates = new Dictionary<ICombatEntity, WeaponHitState>();

	// Token: 0x040016A2 RID: 5794
	public HashSet<Collider> hitColliders = new HashSet<Collider>();
}
