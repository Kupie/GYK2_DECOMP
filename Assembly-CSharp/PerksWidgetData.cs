using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200093E RID: 2366
public class PerksWidgetData : LazyWidgetDataBase
{
	// Token: 0x140000BE RID: 190
	// (add) Token: 0x06003E64 RID: 15972 RVA: 0x00129C34 File Offset: 0x00127E34
	// (remove) Token: 0x06003E65 RID: 15973 RVA: 0x00129C6C File Offset: 0x00127E6C
	public event Action<PerkData> OnPerkAdded;

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x06003E66 RID: 15974 RVA: 0x00129CA4 File Offset: 0x00127EA4
	// (remove) Token: 0x06003E67 RID: 15975 RVA: 0x00129CDC File Offset: 0x00127EDC
	public event Action<PerkData> OnPerkRemoved;

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x06003E68 RID: 15976 RVA: 0x00129D14 File Offset: 0x00127F14
	// (remove) Token: 0x06003E69 RID: 15977 RVA: 0x00129D4C File Offset: 0x00127F4C
	public event Action<PerkData> OnPerkUpdated;

	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x06003E6A RID: 15978 RVA: 0x00129D81 File Offset: 0x00127F81
	// (set) Token: 0x06003E6B RID: 15979 RVA: 0x00129D89 File Offset: 0x00127F89
	public List<PerkData> Perks { get; private set; }

	// Token: 0x06003E6C RID: 15980 RVA: 0x00129D92 File Offset: 0x00127F92
	public PerksWidgetData()
		: this(MainGame.Instance.GameSave, new List<PerkType> { PerkType.Default })
	{
	}

	// Token: 0x06003E6D RID: 15981 RVA: 0x00129DB0 File Offset: 0x00127FB0
	public PerksWidgetData(GameSave gameSave, List<PerkType> drawingTypes)
	{
		this.drawingTypes = drawingTypes;
		this.Perks = new List<PerkData>();
		for (int i = 0; i < gameSave.perkSystemData.activePerks.Count; i++)
		{
			PerkData perkData = gameSave.perkSystemData.activePerks[i];
			if (!perkData.Definition.isHidden && drawingTypes.Contains(perkData.Definition.perkType))
			{
				this.Perks.Add(perkData);
			}
		}
		MainGame.Instance.GameSave.perkSystemData.OnPerkAdded += this.AddPerkData;
		MainGame.Instance.GameSave.perkSystemData.OnPerksUpdated += this.UpdatePerkData;
		MainGame.Instance.GameSave.perkSystemData.OnPerkRemoved += this.RemovePerkData;
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x00129E90 File Offset: 0x00128090
	private void AddPerkData(PerkData perkData)
	{
		if (perkData.Definition.isHidden)
		{
			return;
		}
		if (!this.drawingTypes.Contains(perkData.Definition.perkType))
		{
			return;
		}
		if (this.Perks.Find((PerkData x) => x.id == perkData.id) != null)
		{
			Debug.LogWarning("[PerkWidgetData]: perk [" + perkData.id + "] has been already added");
			return;
		}
		this.Perks.Add(perkData);
		Action<PerkData> onPerkAdded = this.OnPerkAdded;
		if (onPerkAdded == null)
		{
			return;
		}
		onPerkAdded(perkData);
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x00129F3C File Offset: 0x0012813C
	private void RemovePerkData(PerkData perkData)
	{
		if (perkData.Definition.isHidden)
		{
			return;
		}
		if (!this.drawingTypes.Contains(perkData.Definition.perkType))
		{
			return;
		}
		PerkData perkData2 = this.Perks.Find((PerkData x) => x.id == perkData.id);
		if (perkData2 == null)
		{
			Debug.LogWarning("[PerksWidgetData]: perk [" + perkData.id + "] has been already removed");
			return;
		}
		this.Perks.Remove(perkData2);
		Action<PerkData> onPerkRemoved = this.OnPerkRemoved;
		if (onPerkRemoved == null)
		{
			return;
		}
		onPerkRemoved(perkData2);
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x00129FE0 File Offset: 0x001281E0
	private void UpdatePerkData(List<PerkData> perksData)
	{
		using (List<PerkData>.Enumerator enumerator = perksData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PerkData perkData = enumerator.Current;
				if (!perkData.Definition.isHidden && this.drawingTypes.Contains(perkData.Definition.perkType))
				{
					PerkData perkData2 = this.Perks.Find((PerkData x) => x.id == perkData.id);
					if (perkData2 == null)
					{
						Debug.LogWarning("[PerksWidgetData]: perk [" + perkData.id + "] tries to update data but it has been removed");
					}
					else
					{
						Action<PerkData> onPerkUpdated = this.OnPerkUpdated;
						if (onPerkUpdated != null)
						{
							onPerkUpdated(perkData2);
						}
					}
				}
			}
		}
	}

	// Token: 0x04003110 RID: 12560
	private List<PerkType> drawingTypes;
}
