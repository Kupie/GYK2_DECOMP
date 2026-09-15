using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200026A RID: 618
[Serializable]
public class CraftElementBase
{
	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06000FE3 RID: 4067 RVA: 0x00051130 File Offset: 0x0004F330
	// (remove) Token: 0x06000FE4 RID: 4068 RVA: 0x00051168 File Offset: 0x0004F368
	public event Action<CraftStatus> OnStatusChanged;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06000FE5 RID: 4069 RVA: 0x000511A0 File Offset: 0x0004F3A0
	// (remove) Token: 0x06000FE6 RID: 4070 RVA: 0x000511D8 File Offset: 0x0004F3D8
	public event Action OnCountChanged;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06000FE7 RID: 4071 RVA: 0x00051210 File Offset: 0x0004F410
	// (remove) Token: 0x06000FE8 RID: 4072 RVA: 0x00051248 File Offset: 0x0004F448
	public event Action OnProgressChanged;

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x0005127D File Offset: 0x0004F47D
	public string CraftId
	{
		get
		{
			return this.craftId;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00051285 File Offset: 0x0004F485
	public virtual bool IsStarted
	{
		get
		{
			return this.isStarted;
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0005128D File Offset: 0x0004F48D
	public bool IsFinished
	{
		get
		{
			return this.isFinished;
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual bool CaBeFinished
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000FED RID: 4077 RVA: 0x00051295 File Offset: 0x0004F495
	// (set) Token: 0x06000FEE RID: 4078 RVA: 0x0005129D File Offset: 0x0004F49D
	public bool IsAllRequirementsTaken
	{
		get
		{
			return this.isAllRequirementsTaken;
		}
		set
		{
			this.isAllRequirementsTaken = value;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000FEF RID: 4079 RVA: 0x000512A6 File Offset: 0x0004F4A6
	public bool IsFailed
	{
		get
		{
			return this.Def.isStarCraft && this.paramsData.total <= 0f && this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.Common;
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x000512D7 File Offset: 0x0004F4D7
	// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x000512DF File Offset: 0x0004F4DF
	public CraftComponentStatus PrevCraftComponentStatus
	{
		get
		{
			return this.prevCraftComponentStatus;
		}
		set
		{
			this.prevCraftComponentStatus = value;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x000512E8 File Offset: 0x0004F4E8
	// (set) Token: 0x06000FF3 RID: 4083 RVA: 0x000512F0 File Offset: 0x0004F4F0
	public bool IsPreFinishUpdated
	{
		get
		{
			return this.isPreFinishUpdated;
		}
		set
		{
			this.isPreFinishUpdated = value;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x000512F9 File Offset: 0x0004F4F9
	public List<Item> CustomItems
	{
		get
		{
			return this.customItems;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x00051301 File Offset: 0x0004F501
	public CraftDefBase Def
	{
		get
		{
			if (this.def != null)
			{
				return this.def;
			}
			this.def = this.GetCraftDef();
			return this.def;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00051324 File Offset: 0x0004F524
	public List<Item> CraftInput
	{
		get
		{
			return this.craftInput;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x0005132C File Offset: 0x0004F52C
	public List<NeedItemData> Requirements
	{
		get
		{
			return this.requirements;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x00051334 File Offset: 0x0004F534
	public CraftParamsData ParamsData
	{
		get
		{
			return this.paramsData;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x0005133C File Offset: 0x0004F53C
	public float ProgressTimeNormalized
	{
		get
		{
			if (this.totalProgressTicks == 0)
			{
				return 1f;
			}
			return (float)this.currentProgressTicks / (float)this.totalProgressTicks;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000FFA RID: 4090 RVA: 0x0005135B File Offset: 0x0004F55B
	public float ProgressTimeNormalizedFailed
	{
		get
		{
			if (this.totalProgressTicks == 0)
			{
				return 1f;
			}
			return (float)this.failedProgressTicks / (float)this.totalProgressTicks;
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0005137A File Offset: 0x0004F57A
	public int ProgressTicks
	{
		get
		{
			return this.currentProgressTicks;
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00051382 File Offset: 0x0004F582
	public virtual int TotalProgressTicks
	{
		get
		{
			return this.totalProgressTicks;
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0005138A File Offset: 0x0004F58A
	public int FailedProgressTicks
	{
		get
		{
			return this.failedProgressTicks;
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00051392 File Offset: 0x0004F592
	public int SucceededProgressTicks
	{
		get
		{
			return this.succeededProgressTicks;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0005139A File Offset: 0x0004F59A
	public List<ItemCount> PreToWgoOnStartItems
	{
		get
		{
			return this.preToWgoOnStartItems;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06001000 RID: 4096 RVA: 0x000513A2 File Offset: 0x0004F5A2
	public List<ItemCount> PreToWgoOnFinishItems
	{
		get
		{
			return this.preToWgoOnFinishItems;
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06001001 RID: 4097 RVA: 0x000513AA File Offset: 0x0004F5AA
	public List<ItemCount> PreOutputItems
	{
		get
		{
			return this.preOutputItems;
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06001002 RID: 4098 RVA: 0x000513B2 File Offset: 0x0004F5B2
	// (set) Token: 0x06001003 RID: 4099 RVA: 0x000513BA File Offset: 0x0004F5BA
	public int Count
	{
		get
		{
			return this.count;
		}
		set
		{
			this.count = value;
			Action onCountChanged = this.OnCountChanged;
			if (onCountChanged == null)
			{
				return;
			}
			onCountChanged();
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06001004 RID: 4100 RVA: 0x000513D3 File Offset: 0x0004F5D3
	// (set) Token: 0x06001005 RID: 4101 RVA: 0x000513DB File Offset: 0x0004F5DB
	public bool IsInfinite
	{
		get
		{
			return this.isInfinite;
		}
		set
		{
			if (!value)
			{
				this.count = 1;
			}
			this.isInfinite = value;
			Action onCountChanged = this.OnCountChanged;
			if (onCountChanged == null)
			{
				return;
			}
			onCountChanged();
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06001006 RID: 4102 RVA: 0x000513FE File Offset: 0x0004F5FE
	// (set) Token: 0x06001007 RID: 4103 RVA: 0x00051406 File Offset: 0x0004F606
	public CraftStatus CraftStatus
	{
		get
		{
			return this.craftStatus;
		}
		set
		{
			if (this.craftStatus == value)
			{
				return;
			}
			this.craftStatus = value;
			Action<CraftStatus> onStatusChanged = this.OnStatusChanged;
			if (onStatusChanged == null)
			{
				return;
			}
			onStatusChanged(this.craftStatus);
		}
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x00051430 File Offset: 0x0004F630
	public bool TryMergeWith(CraftElementBase other)
	{
		if (this.Def.IsOneTimeCraft())
		{
			return false;
		}
		if (!(this.craftId == other.craftId))
		{
			return false;
		}
		if (this.isStarted != other.isStarted)
		{
			return false;
		}
		if (this.isPaused != other.isPaused)
		{
			return false;
		}
		if (!this.paramsData.Equals(other.paramsData))
		{
			return false;
		}
		if (this.requirements.Count != other.requirements.Count)
		{
			return false;
		}
		for (int i = 0; i < this.requirements.Count; i++)
		{
			if (!this.requirements[i].Equals(other.requirements[i]))
			{
				return false;
			}
		}
		this.Count += other.count;
		return true;
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x00051500 File Offset: 0x0004F700
	public CraftElementBase()
	{
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x00051555 File Offset: 0x0004F755
	public CraftElementBase(string craftId, int count, CraftParamsData craftParamsData)
		: this(craftId, count, new List<NeedItemData>(), craftParamsData)
	{
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x00051568 File Offset: 0x0004F768
	public CraftElementBase(string craftId, int count, List<NeedItemData> requirements, CraftParamsData paramsData)
	{
		this.craftId = craftId;
		this.count = count;
		this.requirements = requirements;
		this.paramsData = paramsData;
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x000515DC File Offset: 0x0004F7DC
	protected CraftElementBase(CraftElementBase other, int count = 1)
	{
		this.CopyFrom(other, count);
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0005163C File Offset: 0x0004F83C
	protected virtual void CopyFrom(CraftElementBase other, int count = 1)
	{
		this.craftId = other.craftId;
		this.isStarted = other.isStarted;
		this.isPaused = other.isPaused;
		this.isFinished = other.isFinished;
		this.isAllRequirementsTaken = other.isAllRequirementsTaken;
		this.currentProgressTicks = other.currentProgressTicks;
		this.totalProgressTicks = other.totalProgressTicks;
		this.failedProgressTicks = other.failedProgressTicks;
		this.succeededProgressTicks = other.succeededProgressTicks;
		this.requirements = new List<NeedItemData>(other.requirements);
		this.count = count;
		this.isInfinite = false;
		this.craftInput = new List<Item>(other.craftInput);
		this.craftStatus = other.craftStatus;
		this.paramsData = new CraftParamsData(other.paramsData);
		this.customCraftOutput = new List<Item>(other.customCraftOutput);
		this.customItems = new List<Item>(other.customItems);
		this.preToWgoOnStartItems = new List<ItemCount>(other.preToWgoOnStartItems);
		this.preToWgoOnFinishItems = new List<ItemCount>(other.preToWgoOnFinishItems);
		this.preOutputItems = new List<ItemCount>(other.preOutputItems);
		this.isFinishOutputUpdated = other.isFinishOutputUpdated;
		this.craftable = other.craftable;
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0005176F File Offset: 0x0004F96F
	public virtual CraftElementBase Clone(int count = 1)
	{
		return new CraftElementBase(this, count);
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x00051778 File Offset: 0x0004F978
	public NeedItemData GetRequirement(string itemId)
	{
		foreach (NeedItemData needItemData in this.requirements)
		{
			if (itemId == needItemData.id)
			{
				return needItemData;
			}
		}
		return null;
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x000517DC File Offset: 0x0004F9DC
	public void AddRequirement(NeedItemData needItemData)
	{
		this.requirements.Add(needItemData);
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x000517EC File Offset: 0x0004F9EC
	public void RemoveRequirement(string itemId)
	{
		for (int i = 0; i < this.requirements.Count; i++)
		{
			if (this.requirements[i].id == itemId)
			{
				this.requirements.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x00051838 File Offset: 0x0004FA38
	public virtual void DoBeforeStartCalculations(ICraftable craftable)
	{
		this.preToWgoOnStartItems = (this.Def.isStarCraft ? this.Def.addItemsToWgoOnStart.MakePreOutput(craftable, (this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.Common) ? this.paramsData.total : 0f) : this.Def.addItemsToWgoOnStart.MakePreOutput(craftable, 0f));
		this.preToWgoOnFinishItems = (this.Def.isStarCraft ? this.Def.addItemsToWgoOnFinish.MakePreOutput(craftable, (this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.Common) ? this.paramsData.total : 0f) : this.Def.addItemsToWgoOnFinish.MakePreOutput(craftable, 0f));
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x000518FC File Offset: 0x0004FAFC
	public virtual CraftStatus CanStartCraft(ICraftable craftableObject, MultiInventory multiInventory = null)
	{
		MainGame instance = MainGame.Instance;
		KnowledgeSystem knowledgeSystem;
		if (instance == null)
		{
			knowledgeSystem = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			knowledgeSystem = ((gameSave != null) ? gameSave.knowledgeSystem : null);
		}
		KnowledgeSystem knowledgeSystem2 = knowledgeSystem;
		if (knowledgeSystem2 != null && knowledgeSystem2.IsOneTimeCraftCompleted(this.Def))
		{
			return CraftStatus.Other;
		}
		if (multiInventory == null)
		{
			multiInventory = craftableObject.GetCraftableMultiInventory(false);
		}
		if (this.Def.isStarCraft || this.Def.isAutopsyCraft)
		{
			WgoData wgoData = craftableObject as WgoData;
			if (wgoData != null && craftableObject.CraftableAttachedWorker != null && craftableObject.CraftableAttachedWorker.GetMasteryLevelForTalentBranch(wgoData.Definition.talent, this.Def) <= 0)
			{
				return CraftStatus.NotEnoughMastery;
			}
		}
		if (this.requirements.Count > 0 && !multiInventory.HasItemsById(this.requirements, craftableObject as WgoData))
		{
			return CraftStatus.NotEnoughResources;
		}
		if (this.Def.needItemsFromWgo.Count > 0 && !craftableObject.CraftableObjectInventory.Data.HasItemsWithIds(this.Def.needItemsFromWgo, 1, null))
		{
			return CraftStatus.NotEnoughResources;
		}
		if (this.Def.addItemsToWgoOnStart.HasOutputItems && !craftableObject.CraftableObjectInventory.CanAddItemsToInventory(this.preToWgoOnStartItems))
		{
			return CraftStatus.NotEnoughSpaceInWgo;
		}
		return CraftStatus.OK;
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x00051A18 File Offset: 0x0004FC18
	public virtual CraftStatus CanFinishCraft(ICraftable craftableObject)
	{
		if (this.Def.isAuto)
		{
			if (this.customCraftOutput.Count == 0)
			{
				if (!craftableObject.CraftableObjectCraftInventory.CanAddItemsToInventory(this.preOutputItems))
				{
					return CraftStatus.NotEnoughSpaceInWgo;
				}
			}
			else
			{
				List<ItemCount> list = new List<ItemCount>();
				foreach (ItemCount itemCount in list)
				{
					list.Add(itemCount);
				}
				foreach (Item item in this.customCraftOutput)
				{
					list.Add(new ItemCount(item));
					if (!craftableObject.CraftableObjectCraftInventory.CanAddItemsToInventory(list))
					{
						return CraftStatus.NotEnoughSpaceInWgo;
					}
				}
			}
		}
		if (this.Def.addItemsToWgoOnFinish.HasOutputItems && !craftableObject.CraftableObjectInventory.CanAddItemsToInventoryConsideringDestination(this, this.preToWgoOnFinishItems))
		{
			return CraftStatus.NotEnoughSpaceInWgo;
		}
		if (!this.Def.isAuto && craftableObject.CraftableAttachedWorker != null && this.paramsData.customRes.GetInt("do_not_check_multiinventory_space") == 0 && craftableObject.CraftableAttachedWorker is ZombieWgoData && !craftableObject.GetCraftableMultiInventory(true).CanAddItems(this.preOutputItems))
		{
			return CraftStatus.NotEnoughSpaceInMultiInventory;
		}
		return CraftStatus.OK;
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x00051B7C File Offset: 0x0004FD7C
	public void BindCraftable(ICraftable craftable)
	{
		this.craftable = craftable;
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x00051B88 File Offset: 0x0004FD88
	public virtual void Start(ICraftable craftable)
	{
		this.currentProgressTicks = ((this.Def.isStarCraft || this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing || this.Def.isAutopsyCraft) ? this.paramsData.CraftStartTicks : 0);
		this.failedProgressTicks = ((this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing) ? this.paramsData.FailedStartTicks : 0);
		this.succeededProgressTicks = this.currentProgressTicks;
		this.currentProgressTicks += this.failedProgressTicks;
		this.craftable = craftable;
		this.totalProgressTicks = this.Def.duration.EvaluateInt(craftable) + this.paramsData.PerksCraftAddTotalProgressTicksValue;
		this.isStarted = true;
		this.preOutputItems = (this.Def.isStarCraft ? this.Def.outputItems.MakePreOutput(craftable, (float)((this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.Common) ? 1 : 0)) : this.Def.outputItems.MakePreOutput(craftable, 0f));
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.CraftStart, this.craftId);
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x00051CA0 File Offset: 0x0004FEA0
	public virtual void Finish()
	{
		bool flag = this.isStarted;
		this.isStarted = false;
		this.isFinished = true;
		if (flag)
		{
			MainGame instance = MainGame.Instance;
			if (instance != null)
			{
				GameSave gameSave = instance.GameSave;
				if (gameSave != null)
				{
					KnowledgeSystem knowledgeSystem = gameSave.knowledgeSystem;
					if (knowledgeSystem != null)
					{
						knowledgeSystem.CompleteOneTimeCraft(this.Def);
					}
				}
			}
		}
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.CraftFinish, this.craftId);
	}

	// Token: 0x06001018 RID: 4120 RVA: 0x00051CFC File Offset: 0x0004FEFC
	public virtual void Cancel()
	{
		this.isStarted = false;
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x00051D08 File Offset: 0x0004FF08
	public virtual void Update(int deltaTicks)
	{
		if (deltaTicks == 0 && (this.Def.isStarCraft || this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.GardenGrowing || this.Def.isAutopsyCraft) && (this.craftable.CraftableAttachedWorker == null || (this.craftable.CraftableAttachedWorker != null && !(this.craftable.CraftableAttachedWorker is ZombieWgoData))))
		{
			this.currentProgressTicks++;
			this.failedProgressTicks++;
			this.NotifyProgressChanged();
			return;
		}
		this.currentProgressTicks += deltaTicks;
		this.succeededProgressTicks += deltaTicks;
		this.craftable.OnSuccessfulTicksChange(this.succeededProgressTicks - deltaTicks, this.succeededProgressTicks, this);
		this.NotifyProgressChanged();
	}

	// Token: 0x0600101A RID: 4122 RVA: 0x00051DCB File Offset: 0x0004FFCB
	public virtual List<Item> MakeOutput()
	{
		List<Item> list = OutputItems.MakeOutput(this.preOutputItems);
		list.AddRange(this.customCraftOutput);
		return list;
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x00051DE4 File Offset: 0x0004FFE4
	public virtual void UpdateActualOutputBeforeFinish()
	{
		if (this.isFinishOutputUpdated)
		{
			return;
		}
		this.UpdateTotalParamValue();
		this.preOutputItems = (this.Def.isStarCraft ? this.Def.outputItems.MakePreOutput(this.craftable, (this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.Common) ? this.paramsData.total : 0f) : this.Def.outputItems.MakePreOutput(this.craftable, 0f));
		this.preToWgoOnFinishItems = (this.Def.isStarCraft ? this.Def.addItemsToWgoOnFinish.MakePreOutput(this.craftable, (this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.Common) ? this.paramsData.total : 0f) : this.Def.addItemsToWgoOnFinish.MakePreOutput(this.craftable, 0f));
		this.isFinishOutputUpdated = true;
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x00051ED4 File Offset: 0x000500D4
	public virtual void Clear()
	{
		this.craftId = null;
		this.def = null;
		this.isStarted = false;
		this.isPaused = false;
		this.currentProgressTicks = 0;
		this.failedProgressTicks = 0;
		this.succeededProgressTicks = 0;
		this.paramsData = null;
		this.craftInput.Clear();
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x00051F24 File Offset: 0x00050124
	public virtual void RemoveCraftRequirements(ICraftable craftable)
	{
		this.craftInput = craftable.GetCraftableMultiInventory(false).RemoveItems(this.requirements, craftable as WgoData);
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x00028294 File Offset: 0x00026494
	public virtual CraftStatus CheckWorkerDependentValues(IWorker worker, float deltaTime = 1f, bool skipEnergyCheck = false, bool skipInsanityCheck = false)
	{
		return CraftStatus.OK;
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x00051F44 File Offset: 0x00050144
	public virtual int GetCurrentQuality()
	{
		return -1;
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x00051F48 File Offset: 0x00050148
	public virtual void UpdateCountOnFinish()
	{
		int num = this.Count;
		this.Count = num - 1;
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x00051F65 File Offset: 0x00050165
	public void SetCustomOutputItems(List<Item> outputItems)
	{
		this.customCraftOutput = outputItems;
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x00051F6E File Offset: 0x0005016E
	public void SetCustomItems(List<Item> items)
	{
		this.customItems = items;
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x00051F78 File Offset: 0x00050178
	public void UpdateTotalParamValue()
	{
		if (this.Def.isStarCraft && this.paramsData.craftParamsType == CraftParamsData.CraftParamsType.Common)
		{
			int num = 0;
			if (this.succeededProgressTicks >= ((CraftDef)this.Def).goldLevel)
			{
				num = 3;
			}
			else if (this.succeededProgressTicks >= ((CraftDef)this.Def).silverLevel)
			{
				num = 2;
			}
			else if (this.succeededProgressTicks >= ((CraftDef)this.Def).bronzeLevel)
			{
				num = 1;
			}
			this.paramsData.total = (float)num;
		}
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x00052000 File Offset: 0x00050200
	protected virtual CraftDefBase GetCraftDef()
	{
		this.def = GameBalance.GetCraftDefBase(this.craftId);
		return this.def;
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x00052019 File Offset: 0x00050219
	protected void NotifyProgressChanged()
	{
		Action onProgressChanged = this.OnProgressChanged;
		if (onProgressChanged == null)
		{
			return;
		}
		onProgressChanged();
	}

	// Token: 0x0400127A RID: 4730
	[SerializeField]
	protected string craftId;

	// Token: 0x0400127B RID: 4731
	[SerializeField]
	protected bool isStarted;

	// Token: 0x0400127C RID: 4732
	[SerializeField]
	protected bool isPaused;

	// Token: 0x0400127D RID: 4733
	[SerializeField]
	protected bool isFinished;

	// Token: 0x0400127E RID: 4734
	[SerializeField]
	protected bool isAllRequirementsTaken;

	// Token: 0x0400127F RID: 4735
	[SerializeField]
	protected int currentProgressTicks;

	// Token: 0x04001280 RID: 4736
	[SerializeField]
	protected int totalProgressTicks;

	// Token: 0x04001281 RID: 4737
	[SerializeField]
	protected int failedProgressTicks;

	// Token: 0x04001282 RID: 4738
	[SerializeField]
	protected int succeededProgressTicks;

	// Token: 0x04001283 RID: 4739
	[SerializeField]
	protected List<NeedItemData> requirements;

	// Token: 0x04001284 RID: 4740
	[SerializeField]
	protected int count;

	// Token: 0x04001285 RID: 4741
	[SerializeField]
	protected bool isInfinite;

	// Token: 0x04001286 RID: 4742
	[SerializeField]
	protected List<Item> craftInput = new List<Item>();

	// Token: 0x04001287 RID: 4743
	[SerializeField]
	protected CraftStatus craftStatus;

	// Token: 0x04001288 RID: 4744
	[SerializeField]
	protected CraftParamsData paramsData;

	// Token: 0x04001289 RID: 4745
	[SerializeField]
	protected List<Item> customCraftOutput = new List<Item>();

	// Token: 0x0400128A RID: 4746
	[SerializeField]
	protected List<Item> customItems = new List<Item>();

	// Token: 0x0400128B RID: 4747
	[SerializeField]
	protected List<ItemCount> preToWgoOnStartItems = new List<ItemCount>();

	// Token: 0x0400128C RID: 4748
	[SerializeField]
	protected List<ItemCount> preToWgoOnFinishItems = new List<ItemCount>();

	// Token: 0x0400128D RID: 4749
	[SerializeField]
	protected List<ItemCount> preOutputItems = new List<ItemCount>();

	// Token: 0x0400128E RID: 4750
	[SerializeField]
	protected bool isFinishOutputUpdated;

	// Token: 0x0400128F RID: 4751
	[SerializeField]
	protected CraftComponentStatus prevCraftComponentStatus;

	// Token: 0x04001290 RID: 4752
	[SerializeField]
	protected bool isPreFinishUpdated;

	// Token: 0x04001291 RID: 4753
	[NonSerialized]
	protected ICraftable craftable;

	// Token: 0x04001292 RID: 4754
	private CraftDefBase def;
}
