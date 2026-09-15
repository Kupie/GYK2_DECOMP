using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class FightingStage : MonoBehaviour, IBakingContext
{
	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x0600158E RID: 5518 RVA: 0x000692C1 File Offset: 0x000674C1
	public IReadOnlyList<FightingStageData> StagesData
	{
		get
		{
			return this.stagesData;
		}
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x000692CC File Offset: 0x000674CC
	public bool IsEnabledForStage(int stageId)
	{
		foreach (FightingStageData fightingStageData in this.stagesData)
		{
			if (fightingStageData.id == stageId && fightingStageData.enabled)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x00069330 File Offset: 0x00067530
	public void ApplyStage(int stageId)
	{
		bool flag = this.IsEnabledForStage(stageId);
		if (this.chunkableObjects.Count == 0)
		{
			this.CollectChunkableObjects();
		}
		this.ApplyCustomVisibilityDisabled(!flag);
		base.gameObject.SetActive(flag);
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x00069370 File Offset: 0x00067570
	public void ApplyStageFromRuntimeInstance(int stageId)
	{
		bool flag = this.IsEnabledForStage(stageId);
		this.CollectChunkableObjects();
		this.ApplyCustomVisibilityDisabled(!flag);
		base.gameObject.SetActive(flag);
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x000693A4 File Offset: 0x000675A4
	private void ApplyCustomVisibilityDisabled(bool disabled)
	{
		for (int i = 0; i < this.chunkableObjects.Count; i++)
		{
			ChunkableObjectComponent chunkableObjectComponent = this.chunkableObjects[i];
			if (!(chunkableObjectComponent == null))
			{
				chunkableObjectComponent.CustomVisibilityDisabled = disabled;
			}
		}
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x000693E4 File Offset: 0x000675E4
	public int GetStageMask()
	{
		int num = 0;
		foreach (FightingStageData fightingStageData in this.stagesData)
		{
			if (fightingStageData.enabled)
			{
				num |= 1 << fightingStageData.id;
			}
		}
		return num;
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x0006944C File Offset: 0x0006764C
	public void CollectChunkableObjects()
	{
		this.chunkableObjects.Clear();
		base.GetComponentsInChildren<ChunkableObjectComponent>(true, this.chunkableObjects);
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06001595 RID: 5525 RVA: 0x00069466 File Offset: 0x00067666
	public List<BakedChunkableObjectComponentData> GetBakedData
	{
		get
		{
			return this.bakedData;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06001596 RID: 5526 RVA: 0x0006946E File Offset: 0x0006766E
	public int Editor_BakingContextPriority
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x00069471 File Offset: 0x00067671
	public void Editor_SetContextEnableState(bool isActive)
	{
		this.disabledBakingContext = !isActive;
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x0006947D File Offset: 0x0006767D
	private void OnEnable()
	{
		if (this.disabledBakingContext)
		{
			return;
		}
		BakingContextRuntimeRegistration.Register(this, this.registeredBakedChunkCaches);
		this.RegisterLiveChunkableObjects();
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x0006949A File Offset: 0x0006769A
	private void OnDisable()
	{
		this.ForceUnregisterBakingContext();
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x000694A2 File Offset: 0x000676A2
	public void ForceUnregisterBakingContext()
	{
		if (this.disabledBakingContext)
		{
			return;
		}
		BakingContextRuntimeRegistration.Unregister(this.registeredBakedChunkCaches);
		this.UnregisterLiveChunkableObjects();
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x000694C0 File Offset: 0x000676C0
	private void RegisterLiveChunkableObjects()
	{
		this.UnregisterLiveChunkableObjects();
		if (this.chunkableObjects.Count == 0)
		{
			this.CollectChunkableObjects();
		}
		for (int i = 0; i < this.chunkableObjects.Count; i++)
		{
			ChunkableObjectComponent chunkableObjectComponent = this.chunkableObjects[i];
			if (!(chunkableObjectComponent == null))
			{
				chunkableObjectComponent.UpdateChunkVisibility(false);
				this.registeredLiveChunkables.Add(chunkableObjectComponent);
			}
		}
		if (this.registeredLiveChunkables.Count == 0)
		{
			return;
		}
		ChunkManager instance = LazySingleton<ChunkManager>.Instance;
		if (instance == null)
		{
			return;
		}
		instance.RegisterChunks<ChunkableObjectComponent>(this.registeredLiveChunkables, ChunkManagerLayerType.FightingLevelStaticObjects);
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x00069550 File Offset: 0x00067750
	private void UnregisterLiveChunkableObjects()
	{
		if (this.registeredLiveChunkables.Count == 0)
		{
			return;
		}
		ChunkManager instance = LazySingleton<ChunkManager>.Instance;
		if (instance != null)
		{
			instance.UnregisterChunks<ChunkableObjectComponent>(this.registeredLiveChunkables, ChunkManagerLayerType.FightingLevelStaticObjects);
		}
		for (int i = 0; i < this.registeredLiveChunkables.Count; i++)
		{
			ChunkableObjectComponent chunkableObjectComponent = this.registeredLiveChunkables[i];
			if (!(chunkableObjectComponent == null))
			{
				chunkableObjectComponent.UpdateChunkVisibility(false);
			}
		}
		this.registeredLiveChunkables.Clear();
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x000695C8 File Offset: 0x000677C8
	private void Reset()
	{
		for (int i = 0; i < 10; i++)
		{
			this.stagesData.Add(new FightingStageData(i + 1));
		}
	}

	// Token: 0x04001612 RID: 5650
	private const int DEFAULT_STAGES_COUNT = 10;

	// Token: 0x04001613 RID: 5651
	[SerializeField]
	private List<FightingStageData> stagesData = new List<FightingStageData>();

	// Token: 0x04001614 RID: 5652
	[SerializeField]
	private List<ChunkableObjectComponent> chunkableObjects = new List<ChunkableObjectComponent>();

	// Token: 0x04001615 RID: 5653
	[SerializeField]
	private List<BakedChunkableObjectComponentData> bakedData = new List<BakedChunkableObjectComponentData>();

	// Token: 0x04001616 RID: 5654
	[SerializeField]
	private bool disabledBakingContext;

	// Token: 0x04001617 RID: 5655
	[NonSerialized]
	private readonly List<BakedChunkableObjectComponentData> registeredBakedChunkCaches = new List<BakedChunkableObjectComponentData>();

	// Token: 0x04001618 RID: 5656
	[NonSerialized]
	private readonly List<ChunkableObjectComponent> registeredLiveChunkables = new List<ChunkableObjectComponent>();
}
