using System;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class PlayerHPActivity : PlayerActivity
{
	// Token: 0x1400003F RID: 63
	// (add) Token: 0x0600186B RID: 6251 RVA: 0x000734CC File Offset: 0x000716CC
	// (remove) Token: 0x0600186C RID: 6252 RVA: 0x00073500 File Offset: 0x00071700
	public static event Action<string> OnNotEnoughResOccurred;

	// Token: 0x0600186D RID: 6253 RVA: 0x00073533 File Offset: 0x00071733
	public PlayerHPActivity(PlayerData playerData, WgoData wgoData)
	{
		this.playerData = playerData;
		this.wgoData = wgoData;
		this.hpComponent = wgoData.HpComponent;
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x00073558 File Offset: 0x00071758
	public override bool IsEnoughDurability(Item tool)
	{
		DurabilitySerializedItemProperty durabilitySerializedItemProperty;
		return !tool.TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty) || durabilitySerializedItemProperty.Durability > tool.Definition.durDecreaseOnUse;
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x00073584 File Offset: 0x00071784
	public override int GetActionDamage(Item tool)
	{
		bool flag = this.wgoData.GetGameResInt("seed_mastery_lock") > 0;
		int num = MainGame.PlayerController.GetMasteryLevelForTalentBranch(this.wgoData.Definition.talent, null);
		int num2 = (flag ? this.wgoData.GetGameResInt("seed_mastery_lock") : this.wgoData.Definition.MasteryLock);
		if (num <= num2 && flag)
		{
			return 1;
		}
		if (this.wgoData.Definition.noMasteryLock)
		{
			return 1;
		}
		if (num < num2)
		{
			return 0;
		}
		Debug.Log("HPActivity: Rolling HP Damage");
		if (num2 == 0)
		{
			num2 = 1;
			if (num == 0)
			{
				num = 1;
			}
		}
		return this.wgoData.Definition.playerHpActivityMod * Math.Clamp(num / num2, -ConstDef.Get("max_cells_per_one_hit").IntValue, ConstDef.Get("max_cells_per_one_hit").IntValue);
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0007365C File Offset: 0x0007185C
	public override bool CanStartActivity()
	{
		int playerHpActivityMod = this.wgoData.Definition.playerHpActivityMod;
		return (this.hpComponent.Hp > 0 && playerHpActivityMod > 0) || (this.hpComponent.Hp < this.hpComponent.MaxHpValue && playerHpActivityMod < 0);
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x00002318 File Offset: 0x00000518
	public override void OnStartActivity()
	{
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x000736AC File Offset: 0x000718AC
	public override bool IsEnoughMastery()
	{
		return this.wgoData.GetGameResInt("seed_mastery_lock") > 0 || this.wgoData.Worker.GetMasteryLevelForTalentBranch(this.wgoData.Definition.talent, null) >= this.wgoData.Definition.MasteryLock;
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x00073704 File Offset: 0x00071904
	public override bool CanUseTool(Item tool)
	{
		return (!this.hpComponent.isDeathDelayed && this.CanStartActivity()) || this.wgoData.Definition.reviveOnDie;
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x00073730 File Offset: 0x00071930
	public override void UseTool(Item tool, int deltaTick)
	{
		bool hasFullHp = this.hpComponent.HasFullHp;
		int actionDamage = this.GetActionDamage(tool);
		this.hpComponent.ApplyDamage(actionDamage);
		this.wgoData.NotifyApplyTool(hasFullHp);
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x00073769 File Offset: 0x00071969
	public override bool IsEnoughEnergy(Item tool, float energyPerTick)
	{
		bool flag = PlayerEnergyGameResSystem.GetSystem().IsEnoughValue(energyPerTick);
		if (!flag)
		{
			Action<string> onNotEnoughResOccurred = PlayerHPActivity.OnNotEnoughResOccurred;
			if (onNotEnoughResOccurred == null)
			{
				return flag;
			}
			onNotEnoughResOccurred("energy");
		}
		return flag;
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x0007378D File Offset: 0x0007198D
	public override float GetEnergyCostPerTick(Item tool)
	{
		return this.wgoData.Definition.energyPerTick.EvaluateFloat() - tool.Definition.GetGameResOnUse("energy");
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x000737B5 File Offset: 0x000719B5
	public override bool CanChangeInsanity(Item tool, float insanityPerTick)
	{
		bool flag = PlayerInsanityGameResSystem.GetSystem().CanChangeInsanity(insanityPerTick);
		if (!flag)
		{
			Action<string> onNotEnoughResOccurred = PlayerHPActivity.OnNotEnoughResOccurred;
			if (onNotEnoughResOccurred == null)
			{
				return flag;
			}
			onNotEnoughResOccurred("insanity");
		}
		return flag;
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x000737DC File Offset: 0x000719DC
	public override void ConsumeEnergy(Item tool, float energyPerTick)
	{
		float num = -energyPerTick;
		PlayerEnergyGameResSystem.GetSystem().Add(num, false);
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x000737F8 File Offset: 0x000719F8
	public override void ChangeInsanity(Item tool, float insanityPerTick)
	{
		PlayerInsanityGameResSystem.GetSystem().Add(insanityPerTick, false);
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x00073813 File Offset: 0x00071A13
	public override float GetInsanityCostPerTick(Item tool)
	{
		return this.wgoData.Definition.insanityPerTick.EvaluateFloat() - tool.Definition.GetGameResOnUse("insanity");
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x0007277D File Offset: 0x0007097D
	public ItemType GetRequiredToolTypes()
	{
		return this.wgoData.Definition.toolAction.actionableTool;
	}

	// Token: 0x040017F3 RID: 6131
	private HPComponent hpComponent;
}
