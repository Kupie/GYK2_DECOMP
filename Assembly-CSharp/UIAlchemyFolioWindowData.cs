using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020008E6 RID: 2278
public class UIAlchemyFolioWindowData : LazyWidgetDataBase
{
	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x06003B98 RID: 15256 RVA: 0x0011CEAC File Offset: 0x0011B0AC
	// (set) Token: 0x06003B99 RID: 15257 RVA: 0x0011CEB4 File Offset: 0x0011B0B4
	public WgoData WgoData { get; private set; }

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x06003B9A RID: 15258 RVA: 0x0011CEBD File Offset: 0x0011B0BD
	// (set) Token: 0x06003B9B RID: 15259 RVA: 0x0011CEC5 File Offset: 0x0011B0C5
	public Dictionary<AlchemyFormulaTab, List<AlchemyFormulaDef>> Data { get; private set; }

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x06003B9C RID: 15260 RVA: 0x0011CECE File Offset: 0x0011B0CE
	// (set) Token: 0x06003B9D RID: 15261 RVA: 0x0011CED6 File Offset: 0x0011B0D6
	public Dictionary<AlchemyFormulaTab, List<ItemDef>> RuneItems { get; private set; }

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x06003B9E RID: 15262 RVA: 0x0011CEDF File Offset: 0x0011B0DF
	public bool HasAnyTabs
	{
		get
		{
			return this.Data.Count > 0 || this.RuneItems.Count > 0;
		}
	}

	// Token: 0x06003B9F RID: 15263 RVA: 0x0011CF00 File Offset: 0x0011B100
	public void FillFromGaveSave(WgoData wgoData)
	{
		this.WgoData = wgoData;
		this.Data = new Dictionary<AlchemyFormulaTab, List<AlchemyFormulaDef>>();
		for (int i = 0; i < GameBalance.Me.alchemyFormulaDefs.Count; i++)
		{
			AlchemyFormulaDef alchemyFormulaDef = GameBalance.Me.alchemyFormulaDefs[i];
			if (!MainGame.Instance.GameSave.knowledgeSystem.hiddenAlchemyFormulas.Contains(alchemyFormulaDef.id) || MainGame.Instance.GameSave.knowledgeSystem.unlockedAlchemyFormulas.Contains(alchemyFormulaDef.id))
			{
				if (!this.Data.ContainsKey(alchemyFormulaDef.tab))
				{
					this.Data.Add(alchemyFormulaDef.tab, new List<AlchemyFormulaDef>());
				}
				this.Data[alchemyFormulaDef.tab].Add(alchemyFormulaDef);
			}
		}
		this.FillRuneItems();
	}

	// Token: 0x06003BA0 RID: 15264 RVA: 0x0011CFD8 File Offset: 0x0011B1D8
	public bool HasTab(AlchemyFormulaTab tab)
	{
		if (tab.IsRuneTab())
		{
			return this.RuneItems.ContainsKey(tab);
		}
		return this.Data.ContainsKey(tab);
	}

	// Token: 0x06003BA1 RID: 15265 RVA: 0x0011CFFC File Offset: 0x0011B1FC
	private void FillRuneItems()
	{
		this.RuneItems = new Dictionary<AlchemyFormulaTab, List<ItemDef>>();
		List<ItemDef> list = new List<ItemDef>();
		List<ItemDef> list2 = new List<ItemDef>();
		List<ItemDef> list3 = new List<ItemDef>();
		for (int i = 0; i < GameBalance.Me.itemDefs.Count; i++)
		{
			ItemDef itemDef = GameBalance.Me.itemDefs[i];
			if (itemDef.canBeUsedInAlchemy)
			{
				Vector3Int runesAsVector3Int = itemDef.GetRunesAsVector3Int();
				if (!(runesAsVector3Int == Vector3Int.zero) && UIAlchemyFolioWindowData.IsItemRunesKnown(itemDef))
				{
					if (runesAsVector3Int.x > 0)
					{
						list.Add(itemDef);
					}
					if (runesAsVector3Int.y > 0)
					{
						list2.Add(itemDef);
					}
					if (runesAsVector3Int.z > 0)
					{
						list3.Add(itemDef);
					}
				}
			}
		}
		this.TryAddRuneTab(AlchemyFormulaTab.RedRune, list);
		this.TryAddRuneTab(AlchemyFormulaTab.GreenRune, list2);
		this.TryAddRuneTab(AlchemyFormulaTab.BlueRune, list3);
	}

	// Token: 0x06003BA2 RID: 15266 RVA: 0x0011D0CC File Offset: 0x0011B2CC
	private void TryAddRuneTab(AlchemyFormulaTab tab, List<ItemDef> items)
	{
		if (items.Count <= 0)
		{
			return;
		}
		items.Sort((ItemDef a, ItemDef b) => UIAlchemyFolioWindowData.CompareRuneItems(a, b, tab));
		this.RuneItems.Add(tab, items);
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0011D114 File Offset: 0x0011B314
	private static bool IsItemRunesKnown(ItemDef itemDef)
	{
		SurveyDef surveyDefForItemOrNull = GameBalance.GetSurveyDefForItemOrNull(itemDef.id);
		return surveyDefForItemOrNull != null && !surveyDefForItemOrNull.surveyedAtStart && MainGame.Instance.GameSave.knowledgeSystem.IsSurveyCompleted(surveyDefForItemOrNull);
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0011D150 File Offset: 0x0011B350
	private static int CompareRuneItems(ItemDef a, ItemDef b, AlchemyFormulaTab tab)
	{
		Vector3Int runesAsVector3Int = a.GetRunesAsVector3Int();
		Vector3Int runesAsVector3Int2 = b.GetRunesAsVector3Int();
		int num = UIAlchemyFolioWindowData.GetDistinctRuneTypeCount(runesAsVector3Int).CompareTo(UIAlchemyFolioWindowData.GetDistinctRuneTypeCount(runesAsVector3Int2));
		if (num != 0)
		{
			return num;
		}
		num = UIAlchemyFolioWindowData.GetSecondaryRuneOrder(runesAsVector3Int, tab).CompareTo(UIAlchemyFolioWindowData.GetSecondaryRuneOrder(runesAsVector3Int2, tab));
		if (num != 0)
		{
			return num;
		}
		num = UIAlchemyFolioWindowData.GetSelectedRuneCount(runesAsVector3Int, tab).CompareTo(UIAlchemyFolioWindowData.GetSelectedRuneCount(runesAsVector3Int2, tab));
		if (num != 0)
		{
			return num;
		}
		num = runesAsVector3Int.x.CompareTo(runesAsVector3Int2.x);
		if (num != 0)
		{
			return num;
		}
		num = runesAsVector3Int.y.CompareTo(runesAsVector3Int2.y);
		if (num != 0)
		{
			return num;
		}
		num = runesAsVector3Int.z.CompareTo(runesAsVector3Int2.z);
		if (num != 0)
		{
			return num;
		}
		return string.CompareOrdinal(a.id, b.id);
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0011D224 File Offset: 0x0011B424
	private static int GetDistinctRuneTypeCount(Vector3Int runes)
	{
		int num = 0;
		if (runes.x > 0)
		{
			num++;
		}
		if (runes.y > 0)
		{
			num++;
		}
		if (runes.z > 0)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x0011D25E File Offset: 0x0011B45E
	private static int GetSecondaryRuneOrder(Vector3Int runes, AlchemyFormulaTab tab)
	{
		if (UIAlchemyFolioWindowData.GetDistinctRuneTypeCount(runes) != 2)
		{
			return 0;
		}
		if (tab != AlchemyFormulaTab.RedRune && runes.x > 0)
		{
			return 0;
		}
		if (tab != AlchemyFormulaTab.GreenRune && runes.y > 0)
		{
			return 1;
		}
		return 2;
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x0011D28C File Offset: 0x0011B48C
	private static int GetSelectedRuneCount(Vector3Int runes, AlchemyFormulaTab tab)
	{
		switch (tab)
		{
		case AlchemyFormulaTab.RedRune:
			return runes.x;
		case AlchemyFormulaTab.GreenRune:
			return runes.y;
		case AlchemyFormulaTab.BlueRune:
			return runes.z;
		default:
			return 0;
		}
	}
}
