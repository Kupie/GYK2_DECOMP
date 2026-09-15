using System;
using UnityEngine;

// Token: 0x02000157 RID: 343
[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class PlacementBlockingArea : MonoBehaviour
{
	// Token: 0x1700014E RID: 334
	// (get) Token: 0x0600082B RID: 2091 RVA: 0x0002819A File Offset: 0x0002639A
	public Collider Collider
	{
		get
		{
			return this.collider;
		}
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x000281A2 File Offset: 0x000263A2
	public static bool TryGet(Collider col, out PlacementBlockingArea blockingArea)
	{
		blockingArea = null;
		return col != null && col.TryGetComponent<PlacementBlockingArea>(out blockingArea);
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x000281BC File Offset: 0x000263BC
	public static bool IsBlockingFor(Collider col, BuildingDef placingDef, Wgo selfTarget = null)
	{
		PlacementBlockingArea placementBlockingArea;
		if (!PlacementBlockingArea.TryGet(col, out placementBlockingArea) || placingDef == null)
		{
			return false;
		}
		Wgo componentInParent = col.GetComponentInParent<Wgo>();
		if (!(componentInParent == null))
		{
			WgoData data = componentInParent.Data;
			if (((data != null) ? data.Definition : null) != null)
			{
				return !(componentInParent == selfTarget) && !componentInParent.Data.isTempObject && placingDef.IsAlwaysBlockedByWgoGroup(componentInParent.Data.Definition.wgoGroup);
			}
		}
		return false;
	}

	// Token: 0x04000A1C RID: 2588
	[SerializeField]
	private Collider collider;
}
