using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000497 RID: 1175
[Serializable]
public class PerkSystemData
{
	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06001F48 RID: 8008 RVA: 0x0009435C File Offset: 0x0009255C
	// (remove) Token: 0x06001F49 RID: 8009 RVA: 0x00094394 File Offset: 0x00092594
	public event Action<PerkData> OnPerkAdded;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06001F4A RID: 8010 RVA: 0x000943CC File Offset: 0x000925CC
	// (remove) Token: 0x06001F4B RID: 8011 RVA: 0x00094404 File Offset: 0x00092604
	public event Action<PerkData> OnPerkRemoved;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06001F4C RID: 8012 RVA: 0x0009443C File Offset: 0x0009263C
	// (remove) Token: 0x06001F4D RID: 8013 RVA: 0x00094474 File Offset: 0x00092674
	public event Action<List<PerkData>> OnPerksUpdated;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06001F4E RID: 8014 RVA: 0x000944AC File Offset: 0x000926AC
	// (remove) Token: 0x06001F4F RID: 8015 RVA: 0x000944E4 File Offset: 0x000926E4
	public event Action<PerkData> OnPerkReapplied;

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06001F50 RID: 8016 RVA: 0x0008522C File Offset: 0x0008342C
	private PlayerData PlayerData
	{
		get
		{
			return MainGame.PlayerData;
		}
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x0009451C File Offset: 0x0009271C
	public void AddPerk(string id)
	{
		PerkData perkData = this.activePerks.Find((PerkData x) => x.id == id);
		if (perkData == null)
		{
			this.AddNewPerk(new PerkData(id));
			return;
		}
		switch (perkData.Definition.perkAddType)
		{
		case PerkAddType.Update:
		{
			perkData.currentDuration = perkData.Definition.duration;
			perkData.tickTimer = 0f;
			Action<PerkData> onPerkReapplied = this.OnPerkReapplied;
			if (onPerkReapplied != null)
			{
				onPerkReapplied(perkData);
			}
			Action<List<PerkData>> onPerksUpdated = this.OnPerksUpdated;
			if (onPerksUpdated != null)
			{
				onPerksUpdated(new List<PerkData> { perkData });
			}
			Debug.Log("[PerkSystemData]: perk duration [" + perkData.id + "] was set to start value");
			return;
		}
		case PerkAddType.Sum:
		{
			perkData.currentDuration += perkData.Definition.duration;
			Action<PerkData> onPerkReapplied2 = this.OnPerkReapplied;
			if (onPerkReapplied2 != null)
			{
				onPerkReapplied2(perkData);
			}
			Action<List<PerkData>> onPerksUpdated2 = this.OnPerksUpdated;
			if (onPerksUpdated2 != null)
			{
				onPerksUpdated2(new List<PerkData> { perkData });
			}
			Debug.Log("[PerkSystemData]: added perk duration [" + perkData.id + "]");
			return;
		}
		case PerkAddType.AsNew:
			this.AddNewPerk(new PerkData(id));
			return;
		default:
			return;
		}
	}

	// Token: 0x06001F52 RID: 8018 RVA: 0x0009465C File Offset: 0x0009285C
	public void RemovePerk(string id, bool silent = false)
	{
		PerkData perkData = this.activePerks.Find((PerkData x) => x.id == id);
		if (perkData != null)
		{
			this.RemovePerk(perkData, silent);
		}
	}

	// Token: 0x06001F53 RID: 8019 RVA: 0x0009469C File Offset: 0x0009289C
	public void RemovePerk(PerkData perk, bool silent = false)
	{
		this.activePerks.Remove(perk);
		if (perk.Definition.setGameResOnRemove.List.Count > 0)
		{
			this.PlayerData.SetRes(perk.Definition.setGameResOnRemove);
		}
		if (!perk.Definition.addGameResOnRemove.IsEmpty())
		{
			this.PlayerData.AddRes(perk.Definition.addGameResOnRemove);
		}
		foreach (LazyExpression lazyExpression in perk.Definition.onRemoveExpressions)
		{
			lazyExpression.Evaluate();
		}
		if (!silent)
		{
			Action<PerkData> onPerkRemoved = this.OnPerkRemoved;
			if (onPerkRemoved != null)
			{
				onPerkRemoved(perk);
			}
		}
		Debug.Log("[PerkSystemData]: perk [" + perk.id + "] removed");
	}

	// Token: 0x06001F54 RID: 8020 RVA: 0x00094784 File Offset: 0x00092984
	public bool HasPerk(string id)
	{
		return this.activePerks.Find((PerkData x) => x.id == id) != null;
	}

	// Token: 0x06001F55 RID: 8021 RVA: 0x000947B8 File Offset: 0x000929B8
	public void NotifyUpdated()
	{
		Action<List<PerkData>> onPerksUpdated = this.OnPerksUpdated;
		if (onPerksUpdated == null)
		{
			return;
		}
		onPerksUpdated(this.activePerks);
	}

	// Token: 0x06001F56 RID: 8022 RVA: 0x000947D0 File Offset: 0x000929D0
	private void AddNewPerk(PerkData perk)
	{
		perk.currentDuration = perk.Definition.duration;
		if (!perk.Definition.setGameResOnAdd.IsEmpty())
		{
			this.PlayerData.SetRes(perk.Definition.setGameResOnAdd);
		}
		if (!perk.Definition.addGameResOnAdd.IsEmpty())
		{
			this.PlayerData.AddRes(perk.Definition.addGameResOnAdd);
		}
		foreach (LazyExpression lazyExpression in perk.Definition.onAddExpressions)
		{
			lazyExpression.Evaluate();
		}
		this.activePerks.Add(perk);
		Action<PerkData> onPerkAdded = this.OnPerkAdded;
		if (onPerkAdded != null)
		{
			onPerkAdded(perk);
		}
		Debug.Log("[PerkSystemData]: new perk [" + perk.id + "] was added");
	}

	// Token: 0x06001F57 RID: 8023 RVA: 0x000948C0 File Offset: 0x00092AC0
	public static string GetFormattedDuration(float duration)
	{
		return string.Format("{0}:{1:00}", (int)(duration / 60f), (int)(duration % 60f));
	}

	// Token: 0x04001C15 RID: 7189
	public List<PerkData> activePerks = new List<PerkData>();
}
