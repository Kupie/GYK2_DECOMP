using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020005E9 RID: 1513
[Serializable]
public class ZombieCraftActivity : IWorkActivity
{
	// Token: 0x1400008D RID: 141
	// (add) Token: 0x060027F2 RID: 10226 RVA: 0x000BA48C File Offset: 0x000B868C
	// (remove) Token: 0x060027F3 RID: 10227 RVA: 0x000BA4C4 File Offset: 0x000B86C4
	public event Action OnActiveStateChanged;

	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000BA4F9 File Offset: 0x000B86F9
	public SGuid WgoUniqueId
	{
		get
		{
			return this.wgoUniqueId;
		}
	}

	// Token: 0x1700066E RID: 1646
	// (get) Token: 0x060027F5 RID: 10229 RVA: 0x000BA501 File Offset: 0x000B8701
	public SGuid ZombieUniqueId
	{
		get
		{
			return this.zombieUniqueId;
		}
	}

	// Token: 0x1700066F RID: 1647
	// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000BA509 File Offset: 0x000B8709
	private WgoData WgoData
	{
		get
		{
			if (this.wgoData == null)
			{
				this.wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(this.wgoUniqueId);
			}
			return this.wgoData;
		}
	}

	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000BA539 File Offset: 0x000B8739
	public ZombieWgoData Zombie
	{
		get
		{
			if (this.zombie == null)
			{
				this.zombie = MainGame.ZombieSystemData.GetZombie(this.zombieUniqueId);
			}
			return this.zombie;
		}
	}

	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000BA55F File Offset: 0x000B875F
	private CraftComponent CraftComponent
	{
		get
		{
			WgoData wgoData = this.WgoData;
			if (wgoData == null)
			{
				return null;
			}
			return wgoData.CraftComponent;
		}
	}

	// Token: 0x17000672 RID: 1650
	// (get) Token: 0x060027F9 RID: 10233 RVA: 0x000BA572 File Offset: 0x000B8772
	// (set) Token: 0x060027FA RID: 10234 RVA: 0x000BA57A File Offset: 0x000B877A
	public bool IsActive
	{
		get
		{
			return this.isActive;
		}
		private set
		{
			bool flag = this.isActive;
			this.isActive = value;
			if (flag != this.isActive)
			{
				Action onActiveStateChanged = this.OnActiveStateChanged;
				if (onActiveStateChanged == null)
				{
					return;
				}
				onActiveStateChanged();
			}
		}
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x000BA5A4 File Offset: 0x000B87A4
	public ZombieCraftActivity(WgoData wgoData, ZombieWgoData zombie)
	{
		this.wgoUniqueId.SetGuid(wgoData.UniqueId);
		this.wgoData = wgoData;
		this.zombie = zombie;
		this.zombieUniqueId.SetGuid(zombie.UniqueId);
		this.isActive = true;
	}

	// Token: 0x060027FC RID: 10236 RVA: 0x000BA604 File Offset: 0x000B8804
	public void Update(float deltaTime)
	{
		if (this.Zombie == null || this.CraftComponent == null)
		{
			this.IsActive = false;
			return;
		}
		if (this.Zombie.CrafterOrders.Count > 0)
		{
			this.IsActive = false;
			return;
		}
		if (this.CraftComponent.Status == CraftComponentStatus.WaitingForOutputDrop)
		{
			this.IsActive = false;
			return;
		}
		Item hand = this.Zombie.Hand;
		if (!this.CanUseTool(hand))
		{
			this.IsActive = false;
			return;
		}
		if (this.CraftComponent.CurrentCraftElement != null && (this.CraftComponent.CurrentCraftElement.ParamsData.customRes.GetInt("wait_for_zombie_at_sawmill") == 1 || this.CraftComponent.CurrentCraftElement.ParamsData.customRes.GetInt("wait_for_zombie_at_mine") == 1 || this.CraftComponent.CurrentCraftElement.ParamsData.customRes.GetInt("wait_for_zombie_at_sand") == 1 || this.CraftComponent.CurrentCraftElement.ParamsData.customRes.GetInt("wait_for_zombie_at_clay") == 1))
		{
			return;
		}
		this.oneTickAnimationProgress += deltaTime;
		float num = 0f;
		if (hand != null && hand.Definition.isTool && this.CraftComponent.CurrentCraftElement != null)
		{
			if (!this.CraftComponent.CurrentCraftElement.Def.isAuto)
			{
				CraftDef craftDef = this.CraftComponent.CurrentCraftElement.Def as CraftDef;
				if (craftDef != null)
				{
					for (int i = 0; i < craftDef.zombieSpeedItemModificators.List.Count; i++)
					{
						GameResAtom gameResAtom = craftDef.zombieSpeedItemModificators.List[i];
						if (gameResAtom.type == hand.id)
						{
							num = gameResAtom.value;
							break;
						}
					}
				}
			}
			this.IsActive = true;
		}
		this.oneTickAnimationProgress += num * this.oneTickAnimationProgress;
		if (this.oneTickAnimationProgress < 1f)
		{
			return;
		}
		int num2 = Mathf.FloorToInt(this.oneTickAnimationProgress);
		this.oneTickAnimationProgress -= (float)num2;
		if (this.CraftComponent.CurrentCraftElement != null)
		{
			if (!this.CraftComponent.CurrentCraftElement.Def.isAuto)
			{
				this.UseTool(null, num2);
			}
			this.IsActive = true;
			return;
		}
		this.IsActive = false;
	}

	// Token: 0x060027FD RID: 10237 RVA: 0x000BA84C File Offset: 0x000B8A4C
	public bool IsEnoughDurability(Item tool)
	{
		DurabilitySerializedItemProperty durabilitySerializedItemProperty;
		return !tool.TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty) || durabilitySerializedItemProperty.Durability > tool.Definition.durDecreaseOnUse;
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x000BA878 File Offset: 0x000B8A78
	public int GetActionDamage(Item item)
	{
		if (this.CraftComponent.CurrentCraftElement == null)
		{
			return 0;
		}
		int num = this.WgoData.Worker.GetMasteryLevelForTalentBranch(this.WgoData.Definition.talent, this.CraftComponent.CurrentCraftElement.Def);
		int num2 = this.CraftComponent.CurrentCraftElement.Def.talentLock;
		if (num < num2)
		{
			num = num2;
		}
		if (!this.CraftComponent.CurrentCraftElement.Def.isStarCraft)
		{
			if (num2 == 0)
			{
				num2 = 1;
				if (num == 0)
				{
					num = 1;
				}
			}
			return Math.Clamp(num / num2, 0, ConstDef.Get("max_cells_per_one_hit").IntValue);
		}
		float num3 = 100f / (float)num2;
		if (num >= num2)
		{
			return Math.Clamp(num / num2, 0, ConstDef.Get("max_cells_per_one_hit").IntValue);
		}
		float num4 = num3 * (float)num;
		if ((float)global::UnityEngine.Random.Range(1, 100) > num4)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000BA955 File Offset: 0x000B8B55
	public bool CanStartActivity()
	{
		return this.CraftComponent != null && !this.CraftComponent.IsAutoCraftable && (this.CraftComponent.CurrentCraftElement != null || this.CraftComponent.IsQueueDelayed);
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x00002318 File Offset: 0x00000518
	public void OnStartActivity()
	{
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x000BA988 File Offset: 0x000B8B88
	public bool IsEnoughMastery()
	{
		return this.Zombie.CrafterIsEnoughMastery(this.WgoData);
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000BA99B File Offset: 0x000B8B9B
	public bool CanUseTool(Item tool)
	{
		return this.CraftComponent != null && this.CraftComponent.CurrentCraftElement != null && this.Zombie.CrafterCanUseTool(this.WgoData, this.CraftComponent.CurrentCraftElement.Def);
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000BA9D8 File Offset: 0x000B8BD8
	public void UseTool(Item tool, int deltaTick)
	{
		int intValue = ConstDef.Get("zombie_craft_sub_ticks_count").IntValue;
		if (this.CraftComponent.ZombieSubTicks >= intValue)
		{
			this.CraftComponent.ZombieSubTicks -= intValue;
		}
		int num = this.CraftComponent.ZombieSubTicks + 1;
		int num2 = 0;
		if (num >= intValue)
		{
			num -= intValue;
			num2 = this.GetActionDamage(tool);
		}
		this.CraftComponent.ZombieSubTicks = num;
		this.CraftComponent.UpdateManual(num2);
		this.WgoData.NotifyApplyTool(false);
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public bool IsEnoughEnergy(Item tool, float energyPerTick)
	{
		return true;
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x00002318 File Offset: 0x00000518
	public void ConsumeEnergy(Item tool, float energyPerTick)
	{
	}

	// Token: 0x06002806 RID: 10246 RVA: 0x00059250 File Offset: 0x00057450
	public float GetEnergyCostPerTick(Item tool)
	{
		return 0f;
	}

	// Token: 0x06002807 RID: 10247 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public bool CanChangeInsanity(Item tool, float insanityPerTick)
	{
		return true;
	}

	// Token: 0x06002808 RID: 10248 RVA: 0x00002318 File Offset: 0x00000518
	public void ChangeInsanity(Item tool, float insanityPerTick)
	{
	}

	// Token: 0x06002809 RID: 10249 RVA: 0x00059250 File Offset: 0x00057450
	public float GetInsanityCostPerTick(Item tool)
	{
		return 0f;
	}

	// Token: 0x040021BC RID: 8636
	[SerializeField]
	private float oneTickAnimationProgress;

	// Token: 0x040021BD RID: 8637
	[SerializeField]
	private SGuid wgoUniqueId = SGuid.Empty;

	// Token: 0x040021BE RID: 8638
	[SerializeField]
	private SGuid zombieUniqueId = SGuid.Empty;

	// Token: 0x040021C0 RID: 8640
	private WgoData wgoData;

	// Token: 0x040021C1 RID: 8641
	private ZombieWgoData zombie;

	// Token: 0x040021C2 RID: 8642
	private bool isActive;
}
