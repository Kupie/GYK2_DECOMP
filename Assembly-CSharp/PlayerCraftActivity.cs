using System;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class PlayerCraftActivity : PlayerActivity
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x06001831 RID: 6193 RVA: 0x000721A8 File Offset: 0x000703A8
	// (remove) Token: 0x06001832 RID: 6194 RVA: 0x000721DC File Offset: 0x000703DC
	public static event Action<string> OnNotEnoughResOccurred;

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06001833 RID: 6195 RVA: 0x0007220F File Offset: 0x0007040F
	public CraftComponent CraftComponent
	{
		get
		{
			return this.craftComponent;
		}
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x00072217 File Offset: 0x00070417
	public PlayerCraftActivity(PlayerData playerData, WgoData wgoData)
	{
		this.playerData = playerData;
		this.wgoData = wgoData;
		this.craftComponent = wgoData.CraftComponent;
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x0007223C File Offset: 0x0007043C
	public override bool IsEnoughDurability(Item tool)
	{
		DurabilitySerializedItemProperty durabilitySerializedItemProperty;
		return !tool.TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty) || durabilitySerializedItemProperty.Durability > tool.Definition.durDecreaseOnUse;
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x00072268 File Offset: 0x00070468
	public override int GetActionDamage(Item tool)
	{
		if (this.craftComponent.CurrentCraftElement == null)
		{
			return 0;
		}
		int num = MainGame.PlayerController.GetMasteryLevelForTalentBranch(this.wgoData.Definition.talent, this.craftComponent.CurrentCraftElement.Def);
		int num2 = this.craftComponent.CurrentCraftElement.Def.talentLock;
		if (!this.craftComponent.CurrentCraftElement.Def.isStarCraft && !this.craftComponent.CurrentCraftElement.Def.isAutopsyCraft)
		{
			if (num < num2)
			{
				return 0;
			}
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
		else
		{
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
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x00072356 File Offset: 0x00070556
	public override bool CanStartActivity()
	{
		return !this.craftComponent.IsAutoCraftable && (this.craftComponent.CurrentCraftElement != null || this.craftComponent.IsQueueDelayed);
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x00072381 File Offset: 0x00070581
	public override void OnStartActivity()
	{
		if (this.craftComponent.Status == CraftComponentStatus.ReadyToStartCraft || this.craftComponent.IsQueueDelayed)
		{
			this.craftComponent.TryContinueFromQueue();
		}
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x000723AC File Offset: 0x000705AC
	public override bool IsEnoughMastery()
	{
		if (this.wgoData.CraftComponent.CurrentCraftElement.Def.isStarCraft)
		{
			return true;
		}
		if (this.wgoData.CraftComponent.CurrentCraftElement.Def.isAutopsyCraft)
		{
			return true;
		}
		if (this.wgoData.CraftComponent.CurrentCraftElement.Def is SurveyDef)
		{
			return true;
		}
		if (this.wgoData.CraftComponent.CurrentCraftElement is CraftElementMix)
		{
			return true;
		}
		IWorker worker = this.wgoData.Worker;
		string talent = this.wgoData.Definition.talent;
		CraftElementBase currentCraftElement = this.wgoData.CraftComponent.CurrentCraftElement;
		return worker.GetMasteryLevelForTalentBranch(talent, (currentCraftElement != null) ? currentCraftElement.Def : null) >= ((this.wgoData.CraftComponent.CurrentCraftElement != null) ? this.wgoData.CraftComponent.CurrentCraftElement.Def.talentLock : this.wgoData.Definition.MasteryLock);
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x000724AC File Offset: 0x000706AC
	public override bool CanUseTool(Item tool)
	{
		if (this.craftComponent.CurrentCraftElement == null || this.craftComponent.IsQueueDelayed)
		{
			if (!this.craftComponent.IsQueueDelayed)
			{
				return false;
			}
			if (this.craftComponent.CraftElementsQueue.FindIndex((CraftElementBase x) => x.CraftStatus == CraftStatus.OK) == -1)
			{
				return false;
			}
		}
		else if (this.craftComponent.IsFinishDelayed)
		{
			return false;
		}
		return true;
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x00072528 File Offset: 0x00070728
	public override void UseTool(Item tool, int deltaTick)
	{
		int actionDamage = this.GetActionDamage(tool);
		this.craftComponent.UpdateManual(actionDamage);
		this.wgoData.NotifyApplyTool(false);
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x00072558 File Offset: 0x00070758
	public override bool IsEnoughEnergy(Item tool, float energyPerTick)
	{
		if (!this.craftComponent.IsStarted)
		{
			if (this.craftComponent.CraftElementsQueue.Find((CraftElementBase x) => x.CraftStatus == CraftStatus.OK) != null)
			{
				return true;
			}
		}
		bool flag = PlayerEnergyGameResSystem.GetSystem().IsEnoughValue(energyPerTick);
		if (!flag)
		{
			Action<string> onNotEnoughResOccurred = PlayerCraftActivity.OnNotEnoughResOccurred;
			if (onNotEnoughResOccurred == null)
			{
				return flag;
			}
			onNotEnoughResOccurred("energy");
		}
		return flag;
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x000725C8 File Offset: 0x000707C8
	public override float GetEnergyCostPerTick(Item tool)
	{
		if (!this.craftComponent.IsQueueDelayed)
		{
			CraftElementBase currentCraftElement = this.craftComponent.CurrentCraftElement;
			if (((currentCraftElement != null) ? currentCraftElement.Def : null) != null)
			{
				return this.craftComponent.CurrentCraftElement.Def.energyPerTick.EvaluateFloat() + this.wgoData.Worker.GetPerksEnergyBonusValue(this.craftComponent.CurrentCraftElement.Def) - tool.Definition.GetGameResOnUse("energy");
			}
		}
		return 0f;
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x00072650 File Offset: 0x00070850
	public override bool CanChangeInsanity(Item tool, float insanityPerTick)
	{
		if (!this.craftComponent.IsStarted)
		{
			if (this.craftComponent.CraftElementsQueue.Find((CraftElementBase x) => x.CraftStatus == CraftStatus.OK) != null)
			{
				return true;
			}
		}
		bool flag = PlayerInsanityGameResSystem.GetSystem().CanChangeInsanity(insanityPerTick);
		if (!flag)
		{
			Action<string> onNotEnoughResOccurred = PlayerCraftActivity.OnNotEnoughResOccurred;
			if (onNotEnoughResOccurred == null)
			{
				return flag;
			}
			onNotEnoughResOccurred("insanity");
		}
		return flag;
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000726C0 File Offset: 0x000708C0
	public override void ConsumeEnergy(Item tool, float energyPerTick)
	{
		float num = -energyPerTick;
		PlayerEnergyGameResSystem.GetSystem().Add(num, false);
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x000726DC File Offset: 0x000708DC
	public override void ChangeInsanity(Item tool, float insanityPerTick)
	{
		PlayerInsanityGameResSystem.GetSystem().Add(insanityPerTick, false);
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x000726F8 File Offset: 0x000708F8
	public override float GetInsanityCostPerTick(Item tool)
	{
		if (!this.craftComponent.IsQueueDelayed)
		{
			CraftElementBase currentCraftElement = this.craftComponent.CurrentCraftElement;
			if (((currentCraftElement != null) ? currentCraftElement.Def : null) != null)
			{
				return this.craftComponent.CurrentCraftElement.Def.insanityPerTick.EvaluateFloat() + this.wgoData.Worker.GetPerksInsanityBonusValue(this.craftComponent.CurrentCraftElement.Def) - tool.Definition.GetGameResOnUse("insanity");
			}
		}
		return 0f;
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x0007277D File Offset: 0x0007097D
	public ItemType GetRequiredToolType()
	{
		return this.wgoData.Definition.toolAction.actionableTool;
	}

	// Token: 0x040017BC RID: 6076
	private CraftComponent craftComponent;
}
