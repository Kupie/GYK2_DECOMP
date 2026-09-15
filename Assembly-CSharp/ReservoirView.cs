using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000414 RID: 1044
public class ReservoirView : MonoBehaviour
{
	// Token: 0x170004CE RID: 1230
	// (get) Token: 0x06001B3C RID: 6972 RVA: 0x0007E7E9 File Offset: 0x0007C9E9
	public ReservoirConfig Config
	{
		get
		{
			return this.config;
		}
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x0007E7F4 File Offset: 0x0007C9F4
	public void Init(Wgo wgo)
	{
		if (this.isInitialized)
		{
			return;
		}
		if (!wgo)
		{
			return;
		}
		if (!wgo.MainWgoPart)
		{
			return;
		}
		this.wgoData = wgo.Data;
		this.wgoData.OnGameResChanged += this.UpdateFishesInPond;
		this.fishingDefs = FishingDef.GetAllForReservoir(this.wgoData.id);
		this.particles = this.vfxParent.GetComponentInChildren<ParticleSystem>();
		this.dockPoint = wgo.DockPoints[0];
		this.direction = this.dockPoint.Direction.ConvertToVector2XZ();
		this.direction = new Vector3(this.direction.x, 0f, this.direction.y);
		this.spawnPos = this.dockPoint.transform.position + this.direction.normalized * this.config.fishSpawnHorOffset + Vector3.down * this.config.fishSpawnVertOffset;
		this.UpdateFishesInPond("");
		this.isInitialized = true;
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x0007E920 File Offset: 0x0007CB20
	private void OnDestroy()
	{
		if (this.wgoData != null)
		{
			this.wgoData.OnGameResChanged -= this.UpdateFishesInPond;
		}
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x0007E944 File Offset: 0x0007CB44
	private void UpdateFishesInPond(string fishGameRes = "")
	{
		ParticleSystem.EmissionModule emission = this.particles.emission;
		int fishCount = 0;
		this.vfxParent.position = this.spawnPos + Vector3.down * this.vfxDepthOffsetY;
		this.fishingDefs.ForEach(delegate(FishingDef x)
		{
			fishCount += this.wgoData.GetGameResInt(x.fishId);
		});
		if (fishCount <= 5)
		{
			emission.rateOverTime = (float)fishCount / 10f;
			return;
		}
		emission.rateOverTime = 0.5f;
	}

	// Token: 0x04001A60 RID: 6752
	[SerializeField]
	private ReservoirConfig config;

	// Token: 0x04001A61 RID: 6753
	[SerializeField]
	private Transform vfxParent;

	// Token: 0x04001A62 RID: 6754
	[SerializeField]
	private float vfxDepthOffsetY = 0.8f;

	// Token: 0x04001A63 RID: 6755
	private List<FishingDef> fishingDefs = new List<FishingDef>();

	// Token: 0x04001A64 RID: 6756
	private WgoData wgoData;

	// Token: 0x04001A65 RID: 6757
	private ParticleSystem particles;

	// Token: 0x04001A66 RID: 6758
	private DockPoint dockPoint;

	// Token: 0x04001A67 RID: 6759
	private Vector3 direction;

	// Token: 0x04001A68 RID: 6760
	private Vector3 spawnPos;

	// Token: 0x04001A69 RID: 6761
	private bool isInitialized;
}
