using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000264 RID: 612
[Serializable]
public class CraftComponent : IComponent
{
	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06000F77 RID: 3959 RVA: 0x0004F154 File Offset: 0x0004D354
	// (remove) Token: 0x06000F78 RID: 3960 RVA: 0x0004F18C File Offset: 0x0004D38C
	public event Action OnCraftStart;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06000F79 RID: 3961 RVA: 0x0004F1C4 File Offset: 0x0004D3C4
	// (remove) Token: 0x06000F7A RID: 3962 RVA: 0x0004F1FC File Offset: 0x0004D3FC
	public event Action OnCraftFinish;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06000F7B RID: 3963 RVA: 0x0004F234 File Offset: 0x0004D434
	// (remove) Token: 0x06000F7C RID: 3964 RVA: 0x0004F26C File Offset: 0x0004D46C
	public event Action OnPreFinishHoldReleased;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000F7D RID: 3965 RVA: 0x0004F2A4 File Offset: 0x0004D4A4
	// (remove) Token: 0x06000F7E RID: 3966 RVA: 0x0004F2DC File Offset: 0x0004D4DC
	public event Action<CraftComponentStatus> OnStatusChanged;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000F7F RID: 3967 RVA: 0x0004F314 File Offset: 0x0004D514
	// (remove) Token: 0x06000F80 RID: 3968 RVA: 0x0004F34C File Offset: 0x0004D54C
	public event Action<float> OnCraftCurProgressNormalizedChanged;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06000F81 RID: 3969 RVA: 0x0004F384 File Offset: 0x0004D584
	// (remove) Token: 0x06000F82 RID: 3970 RVA: 0x0004F3BC File Offset: 0x0004D5BC
	public event CraftComponent.DelCraftAddedToQueue OnCraftAddedToQueue;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06000F83 RID: 3971 RVA: 0x0004F3F4 File Offset: 0x0004D5F4
	// (remove) Token: 0x06000F84 RID: 3972 RVA: 0x0004F42C File Offset: 0x0004D62C
	public event CraftComponent.DelCraftRemovedFromQueue OnCraftRemovedFromQueue;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06000F85 RID: 3973 RVA: 0x0004F464 File Offset: 0x0004D664
	// (remove) Token: 0x06000F86 RID: 3974 RVA: 0x0004F49C File Offset: 0x0004D69C
	public event Action OnCurCraftIndexUpdate;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06000F87 RID: 3975 RVA: 0x0004F4D4 File Offset: 0x0004D6D4
	// (remove) Token: 0x06000F88 RID: 3976 RVA: 0x0004F50C File Offset: 0x0004D70C
	public event Action<int> OnZombieSubTicksChanged;

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0004F541 File Offset: 0x0004D741
	// (set) Token: 0x06000F8A RID: 3978 RVA: 0x0004F549 File Offset: 0x0004D749
	public int ZombieSubTicks
	{
		get
		{
			return this.zombieSubTicks;
		}
		set
		{
			this.zombieSubTicks = value;
			Action<int> onZombieSubTicksChanged = this.OnZombieSubTicksChanged;
			if (onZombieSubTicksChanged == null)
			{
				return;
			}
			onZombieSubTicksChanged(value);
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0004F564 File Offset: 0x0004D764
	public float AutoCraftTickProgressNormalized
	{
		get
		{
			float num = ((this.autoCraftTickDuration > 0f) ? this.autoCraftTickDuration : 5f);
			return Mathf.Clamp01(this.currentAutoCraftTickTime / num);
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0004F59C File Offset: 0x0004D79C
	public List<CraftDefBase> AvailableCrafts
	{
		get
		{
			List<CraftDefBase> list = new List<CraftDefBase>();
			List<CraftDefBase> list2;
			if (this.craftsFromBalance == null && GameBalance.Me.craftsInCache.TryGetValue(this.craftableObject.CraftableObjectId, out list2))
			{
				this.craftsFromBalance = list2;
			}
			if (this.craftsFromBalance == null)
			{
				return new List<CraftDefBase>();
			}
			foreach (CraftDefBase craftDefBase in this.craftsFromBalance)
			{
				if (!CraftComponent.IsCraftHiddenByKnowledge(craftDefBase))
				{
					CraftDef craftDef = craftDefBase as CraftDef;
					if (craftDef == null || !craftDef.isNeedsUnlock || MainGame.Instance.GameSave.knowledgeSystem.unlockedCrafts.Contains(craftDef.id))
					{
						list.Add(craftDefBase);
					}
				}
			}
			return list;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0004F670 File Offset: 0x0004D870
	public List<CraftDefBase> CraftsIn
	{
		get
		{
			List<CraftDefBase> list = new List<CraftDefBase>();
			List<CraftDefBase> list2;
			if (this.craftsFromBalance == null && GameBalance.Me.craftsInCache.TryGetValue(this.craftableObject.CraftableObjectId, out list2))
			{
				this.craftsFromBalance = list2;
			}
			if (this.craftsFromBalance == null)
			{
				return new List<CraftDefBase>();
			}
			foreach (CraftDefBase craftDefBase in this.craftsFromBalance)
			{
				if (!CraftComponent.IsCraftHiddenByKnowledge(craftDefBase))
				{
					list.Add(craftDefBase);
				}
			}
			return list;
		}
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x0004F710 File Offset: 0x0004D910
	private static bool IsCraftHiddenByKnowledge(CraftDefBase craftDef)
	{
		KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
		return knowledgeSystem.blackListCrafts.Contains(craftDef.id) || knowledgeSystem.IsOneTimeCraftCompleted(craftDef);
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0004F749 File Offset: 0x0004D949
	public bool IsStarted
	{
		get
		{
			return this.status == CraftComponentStatus.Started || this.status == CraftComponentStatus.FinishDelayed;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000F90 RID: 3984 RVA: 0x0004F75F File Offset: 0x0004D95F
	public bool IsQueueDelayed
	{
		get
		{
			return this.status == CraftComponentStatus.QueueDelayed;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0004F76A File Offset: 0x0004D96A
	public bool IsFinishDelayed
	{
		get
		{
			return this.status == CraftComponentStatus.FinishDelayed;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0004F775 File Offset: 0x0004D975
	public bool HasPreFinishUpdate
	{
		get
		{
			return this.hasPreFinishUpdate;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0004F77D File Offset: 0x0004D97D
	public bool IsPreFinishHeld
	{
		get
		{
			return this.preFinishHoldCount > 0;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000F94 RID: 3988 RVA: 0x0004F788 File Offset: 0x0004D988
	// (set) Token: 0x06000F95 RID: 3989 RVA: 0x0004F790 File Offset: 0x0004D990
	public CraftComponentStatus Status
	{
		get
		{
			return this.status;
		}
		set
		{
			CraftComponentStatus craftComponentStatus = this.status;
			this.status = value;
			if (craftComponentStatus != this.status)
			{
				Action<CraftComponentStatus> onStatusChanged = this.OnStatusChanged;
				if (onStatusChanged == null)
				{
					return;
				}
				onStatusChanged(this.status);
			}
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000F96 RID: 3990 RVA: 0x0004F7BD File Offset: 0x0004D9BD
	public ICraftable CraftableObject
	{
		get
		{
			return this.craftableObject;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000F97 RID: 3991 RVA: 0x0004F7C5 File Offset: 0x0004D9C5
	public bool HasCraftsByBalance
	{
		get
		{
			return this.AvailableCrafts != null && this.AvailableCrafts.Count > 0;
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000F98 RID: 3992 RVA: 0x0004F7DF File Offset: 0x0004D9DF
	public CraftElementBase CurrentCraftElement
	{
		get
		{
			if (this.curCraftQueueIdx < 0 || this.curCraftQueueIdx >= this.craftElementsQueue.Count)
			{
				return null;
			}
			return this.craftElementsQueue[this.curCraftQueueIdx];
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0004F810 File Offset: 0x0004DA10
	public bool HasCraftsInQueue
	{
		get
		{
			return this.craftElementsQueue.Count > 0;
		}
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x0004F820 File Offset: 0x0004DA20
	public bool ShouldRegisterInCraftSystem()
	{
		return this.HasCraftsInQueue && (this.craftableObject == null || this.craftableObject.CraftableType != CraftableType.ConveyorWorkbench || this.IsDestroyingCraftActive);
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000F9B RID: 3995 RVA: 0x0004F84A File Offset: 0x0004DA4A
	public bool IsAutoCraftable
	{
		get
		{
			CraftElementBase currentCraftElement = this.CurrentCraftElement;
			return currentCraftElement != null && currentCraftElement.Def.isAuto;
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000F9C RID: 3996 RVA: 0x0004F862 File Offset: 0x0004DA62
	public bool IsManualActualCraftable
	{
		get
		{
			return !this.IsAutoCraftable;
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0004F86D File Offset: 0x0004DA6D
	public List<CraftElementBase> CraftElementsQueue
	{
		get
		{
			return this.craftElementsQueue;
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0004F875 File Offset: 0x0004DA75
	// (set) Token: 0x06000F9F RID: 3999 RVA: 0x0004F87D File Offset: 0x0004DA7D
	public CraftElementBase LastStartedCraftWithRequirements
	{
		get
		{
			return this.lastStartedCraftWithRequirements;
		}
		set
		{
			this.lastStartedCraftWithRequirements = value;
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0004F886 File Offset: 0x0004DA86
	public bool IsDestroyingCraftActive
	{
		get
		{
			return this.CurrentCraftElement != null && this.CurrentCraftElement is CraftElement && ((CraftElement)this.CurrentCraftElement).Definition.isObjDestroyCraft;
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0004F8B4 File Offset: 0x0004DAB4
	public bool IsRemovingDestroyCraft
	{
		get
		{
			return this.isRemovingDestroyCraft;
		}
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x0004F8BC File Offset: 0x0004DABC
	public void Init(ICraftable craftableObject)
	{
		this.craftsFromBalance = null;
		this.craftableObject = craftableObject;
		this.autoCraftTickDuration = craftableObject.AutoCraftTickDuration;
		for (int i = 0; i < this.craftElementsQueue.Count; i++)
		{
			CraftElementBase craftElementBase = this.craftElementsQueue[i];
			if (craftElementBase != null)
			{
				craftElementBase.BindCraftable(craftableObject);
			}
		}
		CraftElementBase craftElementBase2 = this.lastStartedCraftWithRequirements;
		if (craftElementBase2 != null)
		{
			craftElementBase2.BindCraftable(craftableObject);
		}
		this.Init_Runtime(craftableObject);
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0004F92C File Offset: 0x0004DB2C
	private void Init_Runtime(ICraftable craftableObject)
	{
		List<CraftDefBase> list;
		if (GameBalance.Me.craftsInCache.TryGetValue(this.craftableObject.CraftableObjectId, out list))
		{
			this.craftsFromBalance = list;
		}
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x0004F95E File Offset: 0x0004DB5E
	public void ResetCraftsFromBalanceCache()
	{
		this.craftsFromBalance = null;
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x0004F967 File Offset: 0x0004DB67
	public bool TryStartCraft(CraftElementBase craftElement)
	{
		if (!this.IsStarted && this.GetStartCraftStatus(craftElement, null) == CraftStatus.OK)
		{
			this.AddToQueue(craftElement, false, -1);
			this.TryContinueFromQueue();
			return true;
		}
		return false;
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x0004F98F File Offset: 0x0004DB8F
	public void AddCraftNoStart(CraftElementBase craftElement)
	{
		if (this.CurrentCraftElement != null && this.CurrentCraftElement.IsStarted)
		{
			return;
		}
		this.AddToQueue(craftElement, false, -1);
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0004F9B1 File Offset: 0x0004DBB1
	public void RemoveCurNotStartedCraft()
	{
		this.RemoveFromQueue(this.CurrentCraftElement, false);
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x0004F9C0 File Offset: 0x0004DBC0
	private void TryDropConveyorWorkbenchCraftInventory()
	{
		if (this.CraftableObject.CraftableType != CraftableType.ConveyorWorkbench)
		{
			return;
		}
		Inventory craftableObjectCraftInventory = this.CraftableObject.CraftableObjectCraftInventory;
		if (((craftableObjectCraftInventory != null) ? craftableObjectCraftInventory.Data : null) == null || craftableObjectCraftInventory.Data.Inventory.Count == 0)
		{
			return;
		}
		List<Item> list = craftableObjectCraftInventory.Data.RemoveAllItems();
		if (list.Count != 0)
		{
			WgoData wgoData = this.CraftableObject as WgoData;
			if (wgoData != null)
			{
				MainGame instance = MainGame.Instance;
				if (((instance != null) ? instance.dropSystem : null) != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						Item item = list[i];
						MainGame.Instance.dropSystem.DropItem(item, wgoData.WorldId, wgoData.GetDropPos(item), null);
					}
					return;
				}
			}
		}
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x0004FA7B File Offset: 0x0004DC7B
	public void TryStartCurCraft()
	{
		if (this.CurrentCraftElement != null && this.CurrentCraftElement.IsStarted)
		{
			return;
		}
		this.TryContinueFromQueue();
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x0004FA9A File Offset: 0x0004DC9A
	public void TryFinishCurCraft()
	{
		if (this.CurrentCraftElement == null)
		{
			return;
		}
		this.Finish();
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x0004FAAB File Offset: 0x0004DCAB
	public void ContinueAutoCraft()
	{
		this.Finish();
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x0004FAB4 File Offset: 0x0004DCB4
	public void AddDestroyCraft(CraftElement craftElement)
	{
		ZombieWgoData zombieWgoData = this.CraftableObject.CraftableAttachedWorker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			MainGame.Instance.dropSystem.DropItem(zombieWgoData.ZombieItem, zombieWgoData.AttachedWgoData.WorldId, zombieWgoData.AttachedWgoData.GetDropPos(zombieWgoData.ZombieItem), null);
			zombieWgoData.UnAttachFromWgoData(false);
			MainGame.ZombieSystemData.PutZombieFromGameSceneToStore(zombieWgoData);
		}
		this.AddToQueue(craftElement, true, -1);
		this.TryContinueFromQueue();
		if (this.CraftableObject.CraftableType == CraftableType.ConveyorWorkbench)
		{
			MainGame.Instance.craftSystem.AddCraftObject(this);
		}
		Action<CraftComponentStatus> onStatusChanged = this.OnStatusChanged;
		if (onStatusChanged == null)
		{
			return;
		}
		onStatusChanged(this.status);
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0004FB60 File Offset: 0x0004DD60
	public void RemoveDestroyCraft()
	{
		if (!((CraftElement)this.CurrentCraftElement).Definition.isObjDestroyCraft)
		{
			return;
		}
		this.isRemovingDestroyCraft = true;
		if (this.CraftableObject.CraftableType == CraftableType.ConveyorWorkbench)
		{
			MainGame.Instance.craftSystem.RemoveCraftObject(this);
		}
		this.RemoveFromQueue(this.CurrentCraftElement, true);
		this.TryContinueFromQueue();
		if (this.CurrentCraftElement != null && this.CurrentCraftElement.IsPreFinishUpdated)
		{
			this.finishHeldTimer = 0.6f;
			this.Status = this.CurrentCraftElement.PrevCraftComponentStatus;
		}
		Action<CraftComponentStatus> onStatusChanged = this.OnStatusChanged;
		if (onStatusChanged != null)
		{
			onStatusChanged(this.status);
		}
		this.isRemovingDestroyCraft = false;
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0004FC10 File Offset: 0x0004DE10
	public CraftElementBase AddToQueue(CraftElementBase craftElement, bool addToQueueTop = false, int queueIdx = -1)
	{
		if (craftElement.Def.IsOneTimeCraft())
		{
			if (MainGame.Instance.GameSave.knowledgeSystem.IsOneTimeCraftCompleted(craftElement.Def))
			{
				return craftElement;
			}
			CraftElementBase craftElementBase = this.craftElementsQueue.Find((CraftElementBase x) => x.CraftId == craftElement.CraftId);
			if (craftElementBase != null && CraftDefExtensions.ShouldSkipDuplicateOneTimeCraft(craftElement.IsStarted, craftElementBase.IsStarted))
			{
				return craftElementBase;
			}
		}
		craftElement.BindCraftable(this.craftableObject);
		CraftElementBase craftElementBase2 = null;
		if (this.craftElementsQueue.Count > 0)
		{
			craftElementBase2 = (this.CurrentCraftElement.IsStarted ? this.CurrentCraftElement : null);
			List<CraftElementBase> list = this.craftElementsQueue;
			CraftElementBase craftElementBase3 = list[list.Count - 1];
			if (!addToQueueTop && craftElementBase3.TryMergeWith(craftElement))
			{
				return craftElementBase3;
			}
		}
		craftElement.CraftStatus = this.GetStartCraftStatus(craftElement, null);
		if (!addToQueueTop)
		{
			if (queueIdx == -1)
			{
				this.craftElementsQueue.Add(craftElement);
			}
			else
			{
				this.craftElementsQueue.Insert(queueIdx, craftElement);
			}
		}
		else
		{
			int num = ((craftElementBase2 != null && craftElementBase2.CraftId == craftElement.CraftId) ? 1 : 0);
			this.craftElementsQueue.Insert(num, craftElement);
		}
		this.UpdateQueue();
		this.craftableObject.OnAddToQueue(craftElement);
		CraftComponent.DelCraftAddedToQueue onCraftAddedToQueue = this.OnCraftAddedToQueue;
		if (onCraftAddedToQueue != null)
		{
			onCraftAddedToQueue(craftElement);
		}
		return craftElement;
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0004FDA8 File Offset: 0x0004DFA8
	public void RemoveFromQueue(CraftElementBase craftElement, bool removeEvenIfStarted = false)
	{
		if (this.CurrentCraftElement != null && craftElement == this.CurrentCraftElement && (this.CurrentCraftElement.IsStarted && !removeEvenIfStarted))
		{
			return;
		}
		bool flag = this.CraftableObject.CraftableType == CraftableType.ConveyorWorkbench && craftElement != null && !craftElement.IsStarted && this.craftElementsQueue.Contains(craftElement) && this.craftElementsQueue.Count == 1;
		if (this.craftElementsQueue.Remove(craftElement))
		{
			this.UpdateQueue();
			CraftComponent.DelCraftRemovedFromQueue onCraftRemovedFromQueue = this.OnCraftRemovedFromQueue;
			if (onCraftRemovedFromQueue != null)
			{
				onCraftRemovedFromQueue(craftElement);
			}
			if (flag)
			{
				this.TryDropConveyorWorkbenchCraftInventory();
			}
		}
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x0004FE44 File Offset: 0x0004E044
	public void Update(float deltaTime)
	{
		CraftComponentStatus craftComponentStatus = this.status;
		if (craftComponentStatus == CraftComponentStatus.FinishDelayed || craftComponentStatus == CraftComponentStatus.ReadyToFinishAutoCraft || craftComponentStatus == CraftComponentStatus.WaitingForWorkerPickUp)
		{
			return;
		}
		if (this.IsAutoCraftable)
		{
			ZombieWgoData zombieWgoData = this.CraftableObject.CraftableAttachedWorker as ZombieWgoData;
			if (zombieWgoData != null && zombieWgoData.CrafterCurrentOrder != null)
			{
				return;
			}
		}
		if (this.IsQueueDelayed || this.status == CraftComponentStatus.ReadyToStartCraft)
		{
			this.restartQueueTimer += deltaTime;
			if (this.restartQueueTimer >= 1f)
			{
				this.restartQueueTimer = 0f;
				this.TryContinueFromQueue();
			}
			return;
		}
		CraftElementBase currentCraftElement = this.CurrentCraftElement;
		if (currentCraftElement == null || !currentCraftElement.IsStarted)
		{
			return;
		}
		if (this.CurrentCraftElement.ProgressTicks < this.CurrentCraftElement.TotalProgressTicks)
		{
			this.currentAutoCraftTickTime += deltaTime;
			float num = this.autoCraftTickDuration;
			if (num <= 0f)
			{
				num = 5f;
			}
			if (this.currentAutoCraftTickTime.EqualsOrMore(num, 1E-05f))
			{
				int num2 = Mathf.FloorToInt(this.currentAutoCraftTickTime / num);
				if (num2 + currentCraftElement.ProgressTicks > this.CurrentCraftElement.TotalProgressTicks)
				{
					num2 = this.CurrentCraftElement.TotalProgressTicks - currentCraftElement.ProgressTicks;
				}
				this.currentAutoCraftTickTime %= num;
				if (currentCraftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing)
				{
					this.UpdateGardenGrowingCraft(num2, currentCraftElement);
				}
				else
				{
					currentCraftElement.Update(num2);
				}
				Action<float> onCraftCurProgressNormalizedChanged = this.OnCraftCurProgressNormalizedChanged;
				if (onCraftCurProgressNormalizedChanged != null)
				{
					onCraftCurProgressNormalizedChanged(currentCraftElement.ProgressTimeNormalized);
				}
			}
		}
		this.TrySetPreFinishState();
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x0004FFB4 File Offset: 0x0004E1B4
	public void UpdateManual(int deltaTicks)
	{
		if (this.IsFinishDelayed)
		{
			return;
		}
		if (this.status == CraftComponentStatus.WaitingForWorkerPickUp || this.status == CraftComponentStatus.WaitingForOutputDrop)
		{
			return;
		}
		if (this.IsQueueDelayed || this.status == CraftComponentStatus.ReadyToStartCraft)
		{
			this.TryContinueFromQueue();
			return;
		}
		CraftElementBase currentCraftElement = this.CurrentCraftElement;
		if (currentCraftElement == null || !currentCraftElement.IsStarted)
		{
			return;
		}
		if (deltaTicks + currentCraftElement.ProgressTicks > this.CurrentCraftElement.TotalProgressTicks)
		{
			deltaTicks = this.CurrentCraftElement.TotalProgressTicks - currentCraftElement.ProgressTicks;
		}
		if (this.CurrentCraftElement.ProgressTicks < this.CurrentCraftElement.TotalProgressTicks)
		{
			this.CurrentCraftElement.Update(deltaTicks);
		}
		this.TrySetPreFinishState();
		Action<float> onCraftCurProgressNormalizedChanged = this.OnCraftCurProgressNormalizedChanged;
		if (onCraftCurProgressNormalizedChanged == null)
		{
			return;
		}
		onCraftCurProgressNormalizedChanged(this.CurrentCraftElement.ProgressTimeNormalized);
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x00050078 File Offset: 0x0004E278
	public void Cancel()
	{
		CraftDef craftDef = this.CurrentCraftElement.Def as CraftDef;
		if (craftDef != null && craftDef.replaceWgoId == "0")
		{
			return;
		}
		this.DropItems(this.CurrentCraftElement.CraftInput);
		CraftElementBase currentCraftElement = this.CurrentCraftElement;
		this.CurrentCraftElement.Cancel();
		this.craftableObject.OnCraftCancel(currentCraftElement);
		this.Status = CraftComponentStatus.Canceled;
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x000500E4 File Offset: 0x0004E2E4
	public void PreFinishUpdate(float deltaTime)
	{
		if (this.IsPreFinishHeld)
		{
			return;
		}
		this.finishHeldTimer += deltaTime;
		if (this.finishHeldTimer.EqualsOrMore(0.6f, 1E-05f))
		{
			CraftElementBase currentCraftElement = this.CurrentCraftElement;
			currentCraftElement.BindCraftable(this.craftableObject);
			currentCraftElement.IsPreFinishUpdated = true;
			this.hasPreFinishUpdate = false;
			currentCraftElement.UpdateActualOutputBeforeFinish();
			if (currentCraftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing)
			{
				this.Finish();
				return;
			}
			ZombieWgoData zombieWgoData = this.CraftableObject.CraftableAttachedWorker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				ZombieType zombieType = zombieWgoData.ZombieType;
				if (zombieType != ZombieType.Crafter)
				{
					if (zombieType == ZombieType.ConveyorCrafter)
					{
						this.HandleOutputConveyor(currentCraftElement);
						if (currentCraftElement.CaBeFinished)
						{
							this.craftableObject.OnCraftEnd(currentCraftElement);
						}
						this.Status = CraftComponentStatus.WaitingForOutputDrop;
						currentCraftElement.PrevCraftComponentStatus = this.Status;
						return;
					}
					if (zombieType != ZombieType.Gardener)
					{
						return;
					}
					this.Finish();
				}
				else
				{
					bool flag;
					this.HandleOutput(currentCraftElement, out flag);
					this.craftableObject.OnCraftEnd(currentCraftElement);
					this.Status = CraftComponentStatus.WaitingForWorkerPickUp;
					currentCraftElement.PrevCraftComponentStatus = this.Status;
					if (flag && zombieWgoData.WorldZoneData.FindOrdersByTarget(zombieWgoData.UniqueId, typeof(PickupOrder)).Count == 0)
					{
						zombieWgoData.CrafterFinishAndContinueAfterBigItemDropped();
						return;
					}
				}
				return;
			}
			if (currentCraftElement.Def.isAuto && !currentCraftElement.Def.isHidden && !currentCraftElement.Def.autoFinishAutoCraft)
			{
				this.Status = CraftComponentStatus.ReadyToFinishAutoCraft;
				currentCraftElement.PrevCraftComponentStatus = this.Status;
				return;
			}
			if (currentCraftElement.CanFinishCraft(this.craftableObject) == CraftStatus.OK)
			{
				this.Finish();
				return;
			}
			this.Status = CraftComponentStatus.FinishDelayed;
			currentCraftElement.PrevCraftComponentStatus = this.Status;
		}
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x0005027A File Offset: 0x0004E47A
	public void AddPreFinishHold()
	{
		this.preFinishHoldCount++;
		this.finishHeldTimer = 0f;
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x00050298 File Offset: 0x0004E498
	public void ReleasePreFinishHold()
	{
		if (this.preFinishHoldCount <= 0)
		{
			return;
		}
		this.preFinishHoldCount--;
		if (this.preFinishHoldCount > 0)
		{
			return;
		}
		if (this.hasPreFinishUpdate)
		{
			this.finishHeldTimer = 0.6f;
		}
		Action onPreFinishHoldReleased = this.OnPreFinishHoldReleased;
		if (onPreFinishHoldReleased == null)
		{
			return;
		}
		onPreFinishHoldReleased();
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x000502EA File Offset: 0x0004E4EA
	public void Clear()
	{
		CraftElementBase currentCraftElement = this.CurrentCraftElement;
		if (currentCraftElement != null)
		{
			currentCraftElement.Cancel();
		}
		this.craftElementsQueue.Clear();
		this.UpdateQueue();
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x00050310 File Offset: 0x0004E510
	public void UpdateQueueElementsCraftStatus()
	{
		MultiInventory multiInventory = null;
		bool flag = false;
		foreach (CraftElementBase craftElementBase in this.craftElementsQueue)
		{
			if (!craftElementBase.IsStarted)
			{
				if (multiInventory == null)
				{
					multiInventory = this.craftableObject.GetCraftableMultiInventory(false);
				}
				craftElementBase.CraftStatus = this.GetStartCraftStatus(craftElementBase, multiInventory);
				flag = true;
			}
			else
			{
				craftElementBase.CraftStatus = craftElementBase.CanFinishCraft(this.craftableObject);
			}
		}
		if (flag)
		{
			this.TrySetIdxCurrentCraftFromQueue(true);
		}
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x000503A8 File Offset: 0x0004E5A8
	public void UpdateCanContinueManualCraftState(float deltaTime = 1f)
	{
		if (this.IsStarted && !this.CurrentCraftElement.Def.isAuto)
		{
			this.CurrentCraftElement.CraftStatus = this.CurrentCraftElement.CheckWorkerDependentValues(this.craftableObject.CraftableAttachedWorker, deltaTime, false, false);
		}
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x000503E8 File Offset: 0x0004E5E8
	public CraftStatus GetStartCraftStatus(CraftElementBase craftElement, MultiInventory multiInventory = null)
	{
		if (!this.IsCraftAllowedByAttachedExtensions(craftElement))
		{
			return CraftStatus.NoExtension;
		}
		CraftStatus craftStatus = craftElement.CheckWorkerDependentValues(this.craftableObject.CraftableAttachedWorker, 1f, true, true);
		if (craftStatus != CraftStatus.OK)
		{
			return craftStatus;
		}
		if (!craftElement.IsStarted)
		{
			return craftElement.CanStartCraft(this.craftableObject, multiInventory);
		}
		return craftStatus;
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x0005043A File Offset: 0x0004E63A
	public bool IsCraftAllowedByAttachedExtensions(CraftDefBase craftDef)
	{
		return craftDef != null && this.IsCraftAllowedByAttachedExtensionsImpl(craftDef);
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x00050448 File Offset: 0x0004E648
	public void ProcessInstantCraft(ICraftable craftable, CraftElement craftElement)
	{
		while (craftElement.Count > 0)
		{
			craftElement.DoBeforeStartCalculations(craftable);
			this.RemoveRequirements(craftElement);
			craftElement.Start(craftable);
			this.craftableObject.OnCraftStart(craftElement);
			Action onCraftStart = this.OnCraftStart;
			if (onCraftStart != null)
			{
				onCraftStart();
			}
			craftElement.Finish();
			bool flag;
			this.HandleOutput(craftElement, out flag);
			this.craftableObject.OnCraftEnd(craftElement);
			Action onCraftFinish = this.OnCraftFinish;
			if (onCraftFinish != null)
			{
				onCraftFinish();
			}
			int count = craftElement.Count;
			craftElement.Count = count - 1;
			if (this.GetStartCraftStatus(craftElement, null) != CraftStatus.OK)
			{
				return;
			}
		}
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x000504DC File Offset: 0x0004E6DC
	private void TrySetPreFinishState()
	{
		if (this.CurrentCraftElement.ProgressTicks < this.CurrentCraftElement.TotalProgressTicks || this.hasPreFinishUpdate)
		{
			return;
		}
		this.hasPreFinishUpdate = true;
		this.finishHeldTimer = 0f;
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x00050514 File Offset: 0x0004E714
	private void DropItems(List<Item> items)
	{
		foreach (Item item in items)
		{
			this.craftableObject.MakeDrop(item);
		}
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x00050568 File Offset: 0x0004E768
	[CanBeNull]
	private CraftElementBase Start(CraftElementBase craftElement)
	{
		if (craftElement.Count == 0)
		{
			return null;
		}
		craftElement.DoBeforeStartCalculations(this.craftableObject);
		if (this.CurrentCraftElement != null && craftElement != this.CurrentCraftElement && !this.CurrentCraftElement.IsStarted)
		{
			this.CurrentCraftElement.Finish();
		}
		CraftElementBase craftElementBase = null;
		if (!craftElement.IsStarted)
		{
			craftElementBase = craftElement.Clone(1);
			if (!craftElement.IsInfinite)
			{
				craftElement.UpdateCountOnFinish();
			}
			this.RemoveRequirements(craftElementBase);
			this.autoCraftTickDuration = this.craftableObject.AutoCraftTickDuration;
			craftElementBase.Start(this.craftableObject);
			this.craftableObject.OnCraftStart(craftElementBase);
			if (craftElementBase.Requirements.Count != 0)
			{
				this.lastStartedCraftWithRequirements = craftElementBase;
			}
		}
		Action onCraftStart = this.OnCraftStart;
		if (onCraftStart != null)
		{
			onCraftStart();
		}
		this.Status = CraftComponentStatus.Started;
		return craftElementBase;
	}

	// Token: 0x06000FBF RID: 4031 RVA: 0x00050634 File Offset: 0x0004E834
	private void Finish()
	{
		CraftElementBase currentCraftElement = this.CurrentCraftElement;
		currentCraftElement.UpdateCountOnFinish();
		this.currentAutoCraftTickTime = 0f;
		this.zombieSubTicks = 0;
		this.CurrentCraftElement.Finish();
		if (this.status != CraftComponentStatus.WaitingForWorkerPickUp && this.status != CraftComponentStatus.WaitingForOutputDrop)
		{
			bool flag;
			this.HandleOutput(currentCraftElement, out flag);
			CraftDef craftDef = currentCraftElement.Def as CraftDef;
			if (craftDef != null && craftDef.isObjDestroyCraft)
			{
				for (int i = 1; i < this.craftElementsQueue.Count; i++)
				{
					CraftElementBase craftElementBase = this.craftElementsQueue[i];
					if (!craftElementBase.IsStarted && craftElementBase.CraftInput.Count > 0)
					{
						foreach (NeedItemData needItemData in craftElementBase.Requirements)
						{
							if (!needItemData.ItemDef.isFuel)
							{
								this.craftableObject.MakeDrop(new Item(needItemData.id, needItemData.GetCount(this.craftableObject as WgoData)));
							}
						}
					}
				}
			}
			this.craftableObject.OnCraftEnd(currentCraftElement);
		}
		Action onCraftFinish = this.OnCraftFinish;
		if (onCraftFinish != null)
		{
			onCraftFinish();
		}
		this.UpdateQueue();
		if (this.craftElementsQueue.Count > 0)
		{
			this.TryContinueFromQueue();
			return;
		}
		this.Status = CraftComponentStatus.Finished;
	}

	// Token: 0x06000FC0 RID: 4032 RVA: 0x000507A8 File Offset: 0x0004E9A8
	private void HandleOutput(CraftElementBase craftElementBase, out bool isZombieCrafterMadeDrop)
	{
		isZombieCrafterMadeDrop = false;
		if (craftElementBase.ParamsData.customRes.GetInt("ignore_handle_output") == 1)
		{
			return;
		}
		List<Item> list = craftElementBase.MakeOutput();
		if (this.CraftableObject.CraftableAttachedWorker != null)
		{
			ZombieWgoData zombieWgoData = this.CraftableObject.CraftableAttachedWorker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				if (zombieWgoData.ZombieType == ZombieType.Crafter)
				{
					using (List<Item>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Item item = enumerator.Current;
							if (item.Definition.itemGroupIds.Contains("town_box"))
							{
								isZombieCrafterMadeDrop = true;
								this.craftableObject.MakeDrop(item);
							}
							else
							{
								zombieWgoData.CrafterAddCraftDrop(item);
							}
						}
						return;
					}
				}
				if (zombieWgoData.ZombieType == ZombieType.ConveyorCrafter)
				{
					this.CraftableObject.CraftableObjectCraftInventory.AddItemsToInventory(list);
					return;
				}
				return;
			}
		}
		if (craftElementBase.Def.isAuto && this.CraftableObject.CraftableObjectCraftInventory.Data != null && !this.CraftableObject.CraftableObjectCraftInventory.Data.IsEmpty)
		{
			this.CraftableObject.CraftableObjectCraftInventory.AddItemsToInventory(list);
			return;
		}
		this.DropItems(list);
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x000508DC File Offset: 0x0004EADC
	private void HandleOutputConveyor(CraftElementBase craftElementBase)
	{
		List<Item> list = craftElementBase.MakeOutput();
		this.CraftableObject.CraftableObjectCraftInventory.AddItemsToInventory(list);
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x00050904 File Offset: 0x0004EB04
	private void RemoveRequirements(CraftElementBase craftElement)
	{
		craftElement.RemoveCraftRequirements(this.craftableObject);
		if (craftElement.Def.needItemsFromWgo.Count != 0)
		{
			this.craftableObject.CraftableObjectInventory.RemoveItems(this.CurrentCraftElement.Def.needItemsFromWgo, 1, null);
		}
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x00050954 File Offset: 0x0004EB54
	public bool TryContinueFromQueue()
	{
		this.RemoveUnavailableExtensionCraftsFromQueue();
		this.UpdateQueueElementsCraftStatus();
		ZombieWgoData zombieWgoData = this.CraftableObject.CraftableAttachedWorker as ZombieWgoData;
		bool flag = zombieWgoData != null && zombieWgoData.CrafterCurrentOrder != null;
		if (this.CurrentCraftElement != null && this.CurrentCraftElement.CraftStatus == CraftStatus.OK && !flag)
		{
			CraftElementBase currentCraftElement = this.CurrentCraftElement;
			CraftElementBase craftElementBase = this.Start(currentCraftElement);
			this.curCraftQueueIdx = 0;
			if (craftElementBase != null)
			{
				this.AddToQueue(craftElementBase, true, -1);
			}
			else
			{
				this.ReplaceElementToTop(currentCraftElement);
			}
			Action<float> onCraftCurProgressNormalizedChanged = this.OnCraftCurProgressNormalizedChanged;
			if (onCraftCurProgressNormalizedChanged != null)
			{
				onCraftCurProgressNormalizedChanged(this.CurrentCraftElement.ProgressTimeNormalized);
			}
			return true;
		}
		if (this.craftElementsQueue.Count > 0)
		{
			this.curCraftQueueIdx = 0;
			this.Status = CraftComponentStatus.QueueDelayed;
		}
		return false;
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x00050A10 File Offset: 0x0004EC10
	private bool RemoveUnavailableExtensionCraftsFromQueue()
	{
		bool flag = false;
		for (int i = this.craftElementsQueue.Count - 1; i >= 0; i--)
		{
			CraftElementBase craftElementBase = this.craftElementsQueue[i];
			if (!craftElementBase.IsStarted && !this.IsCraftAllowedByAttachedExtensions(craftElementBase))
			{
				this.craftElementsQueue.RemoveAt(i);
				CraftComponent.DelCraftRemovedFromQueue onCraftRemovedFromQueue = this.OnCraftRemovedFromQueue;
				if (onCraftRemovedFromQueue != null)
				{
					onCraftRemovedFromQueue(craftElementBase);
				}
				flag = true;
			}
		}
		if (flag)
		{
			this.UpdateQueue();
		}
		return flag;
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x00050A7F File Offset: 0x0004EC7F
	private bool IsCraftAllowedByAttachedExtensions(CraftElementBase craftElement)
	{
		return ((craftElement != null) ? craftElement.Def : null) != null && this.IsCraftAllowedByAttachedExtensionsImpl(craftElement.Def);
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x00050AA0 File Offset: 0x0004ECA0
	private bool IsCraftAllowedByAttachedExtensionsImpl(CraftDefBase craftDef)
	{
		WgoData wgoData = this.craftableObject as WgoData;
		if (wgoData == null)
		{
			return true;
		}
		if (string.IsNullOrEmpty(craftDef.extensionNeedId))
		{
			return true;
		}
		List<string> attachedWorkbenchExtensionIds = wgoData.Definition.attachedWorkbenchExtensionIds;
		if (wgoData.AttachedWorkbenchExtensions.Count > 0 && !attachedWorkbenchExtensionIds.Contains(craftDef.extensionNeedId))
		{
			return false;
		}
		foreach (SGuid sguid in wgoData.AttachedWorkbenchExtensions)
		{
			WgoData wgoData2 = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
			if (wgoData2 != null && attachedWorkbenchExtensionIds.Contains(wgoData2.id) && wgoData2.id == craftDef.extensionNeedId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x00050B78 File Offset: 0x0004ED78
	private void UpdateQueue()
	{
		CraftComponentStatus craftComponentStatus = this.status;
		if (this.prevQueueCount <= 0 && this.craftElementsQueue.Count > 0)
		{
			this.prevQueueCount = this.craftElementsQueue.Count;
			if (this.CraftableObject.CraftableType == CraftableType.Regular)
			{
				MainGame.Instance.craftSystem.AddCraftObject(this);
			}
			craftComponentStatus = CraftComponentStatus.ReadyToStartCraft;
		}
		this.curCraftQueueIdx = ((this.craftElementsQueue.Count == 0) ? (-1) : Mathf.Clamp(this.curCraftQueueIdx, 0, this.craftElementsQueue.Count - 1));
		if (this.curCraftQueueIdx == -1 && this.craftElementsQueue.Count > 0)
		{
			this.curCraftQueueIdx = 0;
		}
		for (int i = 0; i < this.craftElementsQueue.Count; i++)
		{
			CraftElementBase craftElementBase = this.craftElementsQueue[i];
			if (craftElementBase.Count <= 0)
			{
				this.craftElementsQueue.RemoveAt(i);
				CraftComponent.DelCraftRemovedFromQueue onCraftRemovedFromQueue = this.OnCraftRemovedFromQueue;
				if (onCraftRemovedFromQueue != null)
				{
					onCraftRemovedFromQueue(craftElementBase);
				}
				i--;
			}
		}
		this.curCraftQueueIdx = ((this.craftElementsQueue.Count == 0) ? (-1) : Mathf.Clamp(this.curCraftQueueIdx, 0, this.craftElementsQueue.Count - 1));
		if (this.craftElementsQueue.Count == 0)
		{
			this.curCraftQueueIdx = -1;
			this.prevQueueCount = 0;
			MainGame.Instance.craftSystem.RemoveCraftObject(this);
			craftComponentStatus = CraftComponentStatus.Finished;
		}
		this.TrySetIdxCurrentCraftFromQueue(false);
		this.Status = craftComponentStatus;
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x00050CD8 File Offset: 0x0004EED8
	private void TrySetIdxCurrentCraftFromQueue(bool useCachedCraftStatuses = false)
	{
		if (this.craftElementsQueue.Count == 0 || (this.CurrentCraftElement != null && this.CurrentCraftElement.IsStarted))
		{
			return;
		}
		this.curCraftQueueIdx = 0;
		for (int i = 0; i < this.craftElementsQueue.Count; i++)
		{
			CraftElementBase craftElementBase = this.craftElementsQueue[i];
			if ((useCachedCraftStatuses ? craftElementBase.CraftStatus : this.GetStartCraftStatus(craftElementBase, null)) == CraftStatus.OK)
			{
				this.curCraftQueueIdx = i;
				break;
			}
		}
		Action onCurCraftIndexUpdate = this.OnCurCraftIndexUpdate;
		if (onCurCraftIndexUpdate == null)
		{
			return;
		}
		onCurCraftIndexUpdate();
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x00050D60 File Offset: 0x0004EF60
	private void ReplaceElementToTop(CraftElementBase craftElement)
	{
		if (this.CraftElementsQueue.IndexOf(craftElement) == -1)
		{
			return;
		}
		this.CraftElementsQueue.Remove(craftElement);
		this.CraftElementsQueue.Insert(0, craftElement);
		this.UpdateQueue();
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x00050D94 File Offset: 0x0004EF94
	public bool TryElementUpToQueue(CraftElementBase craftElement)
	{
		int num = this.CraftElementsQueue.IndexOf(craftElement);
		if (num == 0)
		{
			return false;
		}
		this.CraftElementsQueue.Move(craftElement, num - 1);
		this.TrySetIdxCurrentCraftFromQueue(false);
		return true;
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x00050DCC File Offset: 0x0004EFCC
	public bool TryElementDownToQueue(CraftElementBase craftElement)
	{
		int num = this.CraftElementsQueue.IndexOf(craftElement);
		if (num == this.CraftElementsQueue.Count - 1)
		{
			return false;
		}
		this.CraftElementsQueue.Move(craftElement, num + 1);
		this.TrySetIdxCurrentCraftFromQueue(false);
		return true;
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x00050E10 File Offset: 0x0004F010
	public void UpdateGardenGrowingCraft(int ticks, CraftElementBase craftEl)
	{
		int num = 0;
		while (num < ticks && craftEl.ProgressTicks < craftEl.TotalProgressTicks)
		{
			float num2 = 100f / (float)craftEl.ParamsData.MasteryLock;
			int num4;
			if (craftEl.ParamsData.MasteryValue < craftEl.ParamsData.MasteryLock)
			{
				float num3 = num2 * (float)craftEl.ParamsData.MasteryValue;
				num4 = (((float)global::UnityEngine.Random.Range(1, 100) <= num3) ? 1 : 0);
			}
			else
			{
				num4 = Math.Clamp(craftEl.ParamsData.MasteryValue / craftEl.ParamsData.MasteryLock, 0, ConstDef.Get("max_cells_per_one_hit").IntValue);
			}
			num4 = Mathf.Clamp(num4, 0, 3);
			if (num4 + craftEl.ProgressTicks > craftEl.TotalProgressTicks)
			{
				num4 = craftEl.TotalProgressTicks - craftEl.ProgressTicks;
			}
			craftEl.Update(num4);
			num++;
		}
	}

	// Token: 0x0400124B RID: 4683
	public const int MAX_QUEUE_CRAFTS = 999;

	// Token: 0x0400124C RID: 4684
	public const int MIN_QUEUE_CRAFTS = 1;

	// Token: 0x0400124D RID: 4685
	public const int MIN_QUEUE_CRAFTS_NON_STARTED = 0;

	// Token: 0x0400124E RID: 4686
	private const float RESTART_CRAFT_FROM_QUEUE_DELAY_TIME = 1f;

	// Token: 0x0400124F RID: 4687
	private const float CRAFT_PROGRESS_EPSILON = 0.001f;

	// Token: 0x04001250 RID: 4688
	private const float FINISH_HELD_TIME = 0.6f;

	// Token: 0x04001251 RID: 4689
	private const int GARDEN_MAX_PROGRESS_TICKS = 3;

	// Token: 0x0400125B RID: 4699
	[SerializeField]
	private List<CraftElementBase> craftElementsQueue = new List<CraftElementBase>();

	// Token: 0x0400125C RID: 4700
	[SerializeField]
	private int curCraftQueueIdx = -1;

	// Token: 0x0400125D RID: 4701
	[SerializeField]
	private int prevQueueCount;

	// Token: 0x0400125E RID: 4702
	[SerializeField]
	private float restartQueueTimer;

	// Token: 0x0400125F RID: 4703
	[SerializeField]
	private CraftComponentStatus status;

	// Token: 0x04001260 RID: 4704
	[SerializeField]
	private float autoCraftTickDuration;

	// Token: 0x04001261 RID: 4705
	[SerializeField]
	private float currentAutoCraftTickTime;

	// Token: 0x04001262 RID: 4706
	[SerializeField]
	private int zombieSubTicks;

	// Token: 0x04001263 RID: 4707
	[SerializeField]
	private bool hasPreFinishUpdate;

	// Token: 0x04001264 RID: 4708
	[SerializeField]
	private float finishHeldTimer;

	// Token: 0x04001265 RID: 4709
	private int preFinishHoldCount;

	// Token: 0x04001266 RID: 4710
	[SerializeField]
	private CraftElementBase lastStartedCraftWithRequirements;

	// Token: 0x04001267 RID: 4711
	[NonSerialized]
	private ICraftable craftableObject;

	// Token: 0x04001268 RID: 4712
	[NonSerialized]
	private List<CraftDefBase> craftsFromBalance;

	// Token: 0x04001269 RID: 4713
	private bool isRemovingDestroyCraft;

	// Token: 0x02000265 RID: 613
	// (Invoke) Token: 0x06000FCF RID: 4047
	public delegate void DelCraftAddedToQueue(CraftElementBase craftQueueElement);

	// Token: 0x02000266 RID: 614
	// (Invoke) Token: 0x06000FD3 RID: 4051
	public delegate void DelCraftRemovedFromQueue(CraftElementBase craftQueueElement);
}
