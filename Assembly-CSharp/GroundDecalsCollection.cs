using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class GroundDecalsCollection : MonoBehaviour
{
	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06001218 RID: 4632 RVA: 0x0005A037 File Offset: 0x00058237
	internal Pool DecalPool
	{
		get
		{
			return this.decalPool;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06001219 RID: 4633 RVA: 0x0005A03F File Offset: 0x0005823F
	public DecalLayerBand LayerBand
	{
		get
		{
			return this.layerBand;
		}
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x0005A047 File Offset: 0x00058247
	public void SetLayerBand(DecalLayerBand band)
	{
		this.layerBand = band;
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x0005A050 File Offset: 0x00058250
	public void SpawnDecal(Vector3 position, Direction orientation, string customDeathEffectId = "")
	{
		GroundDecal groundDecal = this.sortedDecals.AddDecal<GroundDecal>(position, orientation, this.decalPool, this.layerBand, this, customDeathEffectId);
		this.activeDecals.Add(groundDecal);
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x0005A085 File Offset: 0x00058285
	internal void RemoveFromActive(GroundDecal decal)
	{
		this.activeDecals.Remove(decal);
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x0005A094 File Offset: 0x00058294
	public void UpdateLifeTime(float deltaTime)
	{
		for (int i = this.activeDecals.Count - 1; i >= 0; i--)
		{
			GroundDecal groundDecal = this.activeDecals[i];
			groundDecal.Lifetime += deltaTime;
			if (groundDecal.Lifetime > groundDecal.TimeToLive)
			{
				this.sortedDecals.RemoveDecal(groundDecal, this.decalPool);
			}
		}
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x0005A0F4 File Offset: 0x000582F4
	private void Awake()
	{
		this.decalPool = new Pool(this.decalPrefab, this.decalsParent, 5, Pool.PoolType.ImmediateActivation, false, null);
		this.decalPrefab.gameObject.SetActive(false);
	}

	// Token: 0x040013E7 RID: 5095
	public GroundDecal decalPrefab;

	// Token: 0x040013E8 RID: 5096
	public Transform decalsParent;

	// Token: 0x040013E9 RID: 5097
	[SerializeField]
	private DecalLayerBand layerBand;

	// Token: 0x040013EA RID: 5098
	private Pool decalPool;

	// Token: 0x040013EB RID: 5099
	private List<GroundDecal> activeDecals = new List<GroundDecal>();

	// Token: 0x040013EC RID: 5100
	[HideInInspector]
	public SortedDecals sortedDecals = new SortedDecals();
}
