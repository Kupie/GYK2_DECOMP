using System;
using UnityEngine;

// Token: 0x020005EB RID: 1515
[Serializable]
public class ZombieHPActivity : IWorkActivity
{
	// Token: 0x1400008E RID: 142
	// (add) Token: 0x0600280F RID: 10255 RVA: 0x000BAB24 File Offset: 0x000B8D24
	// (remove) Token: 0x06002810 RID: 10256 RVA: 0x000BAB5C File Offset: 0x000B8D5C
	public event Action OnActiveStateChanged;

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06002811 RID: 10257 RVA: 0x000BAB91 File Offset: 0x000B8D91
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

	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06002812 RID: 10258 RVA: 0x000BABC1 File Offset: 0x000B8DC1
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

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06002813 RID: 10259 RVA: 0x000BABE7 File Offset: 0x000B8DE7
	// (set) Token: 0x06002814 RID: 10260 RVA: 0x000BABEF File Offset: 0x000B8DEF
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

	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06002815 RID: 10261 RVA: 0x000BAC16 File Offset: 0x000B8E16
	public SGuid ZombieUniqueId
	{
		get
		{
			return this.zombieUniqueId;
		}
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000BAC20 File Offset: 0x000B8E20
	public ZombieHPActivity(WgoData wgoData, ZombieWgoData zombie)
	{
		this.wgoUniqueId.SetGuid(wgoData.UniqueId);
		this.wgoData = wgoData;
		this.zombie = zombie;
		this.zombieUniqueId.SetGuid(zombie.UniqueId);
		this.isActive = true;
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000BAC80 File Offset: 0x000B8E80
	public void Update(float deltaTime)
	{
		if (this.WgoData == null)
		{
			this.IsActive = false;
			return;
		}
		if (this.WgoData.HpComponent.Hp <= 0)
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
		this.oneTickAnimationProgress += deltaTime;
		if (this.oneTickAnimationProgress < 1f)
		{
			return;
		}
		int num = Mathf.Max(1, Mathf.FloorToInt(this.oneTickAnimationProgress));
		this.oneTickAnimationProgress -= (float)num;
		this.UseTool(hand, num);
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000BAD1C File Offset: 0x000B8F1C
	public bool IsEnoughDurability(Item tool)
	{
		DurabilitySerializedItemProperty durabilitySerializedItemProperty;
		return !tool.TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty) || durabilitySerializedItemProperty.Durability > tool.Definition.durDecreaseOnUse;
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000BAD48 File Offset: 0x000B8F48
	public int GetActionDamage(Item tool)
	{
		bool flag = this.wgoData.GetGameResInt("seed_mastery_lock") > 0;
		int num = this.Zombie.GetMasteryLevelForTalentBranch(this.wgoData.Definition.talent, null);
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
		return Math.Clamp(num / num2, -ConstDef.Get("max_cells_per_one_hit").IntValue, ConstDef.Get("max_cells_per_one_hit").IntValue);
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x000BAE0E File Offset: 0x000B900E
	public bool CanStartActivity()
	{
		return this.WgoData != null && this.WgoData.HpComponent.Hp > 0;
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x00002318 File Offset: 0x00000518
	public void OnStartActivity()
	{
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x000BAE30 File Offset: 0x000B9030
	public bool IsEnoughMastery()
	{
		return this.wgoData.GetGameResInt("seed_mastery_lock") > 0 || this.Zombie.GetMasteryLevelForTalentBranch(this.wgoData.Definition.talent, null) >= this.wgoData.Definition.MasteryLock;
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x000BAE83 File Offset: 0x000B9083
	public bool CanUseTool(Item tool)
	{
		return this.Zombie.CrafterCanUseTool(this.WgoData, null);
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x000BAE98 File Offset: 0x000B9098
	public void UseTool(Item tool, int deltaTick)
	{
		bool hasFullHp = this.WgoData.HpComponent.HasFullHp;
		int actionDamage = this.GetActionDamage(tool);
		this.WgoData.HpComponent.ApplyDamage(actionDamage);
		this.wgoData.NotifyApplyTool(hasFullHp);
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public bool IsEnoughEnergy(Item tool, float energyPerTick)
	{
		return true;
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x00002318 File Offset: 0x00000518
	public void ConsumeEnergy(Item tool, float energyPerTick)
	{
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x00059250 File Offset: 0x00057450
	public float GetEnergyCostPerTick(Item tool)
	{
		return 0f;
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public bool CanChangeInsanity(Item tool, float insanityPerTick)
	{
		return true;
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x00002318 File Offset: 0x00000518
	public void ChangeInsanity(Item tool, float insanityPerTick)
	{
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x00059250 File Offset: 0x00057450
	public float GetInsanityCostPerTick(Item tool)
	{
		return 0f;
	}

	// Token: 0x040021C4 RID: 8644
	[SerializeField]
	private float oneTickAnimationProgress;

	// Token: 0x040021C5 RID: 8645
	[SerializeField]
	private SGuid wgoUniqueId = SGuid.Empty;

	// Token: 0x040021C6 RID: 8646
	[SerializeField]
	private SGuid zombieUniqueId = SGuid.Empty;

	// Token: 0x040021C8 RID: 8648
	private WgoData wgoData;

	// Token: 0x040021C9 RID: 8649
	private ZombieWgoData zombie;

	// Token: 0x040021CA RID: 8650
	private bool isActive;
}
