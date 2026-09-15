using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine.UI;

// Token: 0x02000902 RID: 2306
public class UIBuildingWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000913 RID: 2323
	// (get) Token: 0x06003C60 RID: 15456 RVA: 0x00120BB9 File Offset: 0x0011EDB9
	// (set) Token: 0x06003C61 RID: 15457 RVA: 0x00120BC1 File Offset: 0x0011EDC1
	public BuildData BuildData { get; private set; }

	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x06003C62 RID: 15458 RVA: 0x00120BCA File Offset: 0x0011EDCA
	// (set) Token: 0x06003C63 RID: 15459 RVA: 0x00120BD2 File Offset: 0x0011EDD2
	public Action<List<NeedItemData>> OnPress { get; private set; }

	// Token: 0x17000915 RID: 2325
	// (get) Token: 0x06003C64 RID: 15460 RVA: 0x00120BDB File Offset: 0x0011EDDB
	// (set) Token: 0x06003C65 RID: 15461 RVA: 0x00120BE3 File Offset: 0x0011EDE3
	public Func<List<NeedItemData>, bool> CanBuild { get; private set; }

	// Token: 0x17000916 RID: 2326
	// (get) Token: 0x06003C66 RID: 15462 RVA: 0x00120BEC File Offset: 0x0011EDEC
	// (set) Token: 0x06003C67 RID: 15463 RVA: 0x00120BF4 File Offset: 0x0011EDF4
	public Action OnOver { get; private set; }

	// Token: 0x17000917 RID: 2327
	// (get) Token: 0x06003C68 RID: 15464 RVA: 0x00120BFD File Offset: 0x0011EDFD
	// (set) Token: 0x06003C69 RID: 15465 RVA: 0x00120C05 File Offset: 0x0011EE05
	public Action OnOut { get; private set; }

	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x06003C6A RID: 15466 RVA: 0x00120C0E File Offset: 0x0011EE0E
	// (set) Token: 0x06003C6B RID: 15467 RVA: 0x00120C16 File Offset: 0x0011EE16
	public List<UICraftItemCellData> CraftItemCellsData { get; private set; }

	// Token: 0x17000919 RID: 2329
	// (get) Token: 0x06003C6C RID: 15468 RVA: 0x00120C1F File Offset: 0x0011EE1F
	// (set) Token: 0x06003C6D RID: 15469 RVA: 0x00120C27 File Offset: 0x0011EE27
	public MultiInventory MultiInventory { get; private set; }

	// Token: 0x1700091A RID: 2330
	// (get) Token: 0x06003C6E RID: 15470 RVA: 0x00120C30 File Offset: 0x0011EE30
	// (set) Token: 0x06003C6F RID: 15471 RVA: 0x00120C38 File Offset: 0x0011EE38
	public string Name { get; private set; }

	// Token: 0x1700091B RID: 2331
	// (get) Token: 0x06003C70 RID: 15472 RVA: 0x00120C41 File Offset: 0x0011EE41
	// (set) Token: 0x06003C71 RID: 15473 RVA: 0x00120C49 File Offset: 0x0011EE49
	public string Description { get; private set; }

	// Token: 0x1700091C RID: 2332
	// (get) Token: 0x06003C72 RID: 15474 RVA: 0x00120C52 File Offset: 0x0011EE52
	// (set) Token: 0x06003C73 RID: 15475 RVA: 0x00120C5A File Offset: 0x0011EE5A
	public string DescriptionModules { get; private set; }

	// Token: 0x1700091D RID: 2333
	// (get) Token: 0x06003C74 RID: 15476 RVA: 0x00120C63 File Offset: 0x0011EE63
	// (set) Token: 0x06003C75 RID: 15477 RVA: 0x00120C6B File Offset: 0x0011EE6B
	public WorldZoneData WorldZoneData { get; private set; }

	// Token: 0x06003C76 RID: 15478 RVA: 0x00120C74 File Offset: 0x0011EE74
	public UIBuildingWidgetData(BuildData buildData, MultiInventory multiInventory, Action<BuildData, List<NeedItemData>> onPress, Func<BuildData, List<NeedItemData>, bool> canBuild, Action onOver, Action onOut, WorldZoneData worldZoneData)
	{
		UIBuildingWidgetData.<>c__DisplayClass44_0 CS$<>8__locals1 = new UIBuildingWidgetData.<>c__DisplayClass44_0();
		CS$<>8__locals1.onPress = onPress;
		CS$<>8__locals1.canBuild = canBuild;
		base..ctor();
		CS$<>8__locals1.<>4__this = this;
		this.BuildData = buildData;
		this.MultiInventory = multiInventory;
		this.WorldZoneData = worldZoneData;
		this.OnPress = new Action<List<NeedItemData>>(CS$<>8__locals1.<.ctor>g__OnPressAction|0);
		this.CanBuild = new Func<List<NeedItemData>, bool>(CS$<>8__locals1.<.ctor>g__CanBuildFunc|1);
		this.OnOver = onOver;
		this.OnOut = onOut;
		this.FillCraftItemCellsData();
		this.Name = ((buildData.BuildingMode == BuildingDef.BuildingMode.Remove) ? LLBase.L("remove") : buildData.Definition.id);
		if (buildData.Definition != null)
		{
			WgoPartBakedData wgoPartBakedData = LazySingletonSerializedSO<WgoPartBakedDataCollection>.Instance.Get(buildData.Definition.wgoId);
			if (wgoPartBakedData != null && wgoPartBakedData.ModuleBuildingTypes != null && !wgoPartBakedData.ModuleBuildingTypes.IsEmpty())
			{
				this.DescriptionModules = string.Empty;
				foreach (GameResAtom gameResAtom in wgoPartBakedData.ModuleBuildingTypes.List)
				{
					this.DescriptionModules += string.Format("{0}{1} ", gameResAtom.type.FontIcon(), gameResAtom.value);
				}
				this.DescriptionModules.TrimEnd();
				return;
			}
			WGODef wgodef = GameBalance.Me.GetData<WGODef>(buildData.Definition.wgoId);
			if (wgodef != null && wgodef.replaceToWgoOnDie.HasExpression)
			{
				wgodef = GameBalance.Me.GetData<WGODef>(wgodef.replaceToWgoOnDie.Evaluate());
			}
			if (worldZoneData != null && wgodef != null && !string.IsNullOrEmpty(worldZoneData.Definition.qualityIcon) && wgodef.qualityDisplayType == WGODef.QualityDisplayType.Show)
			{
				float num = wgodef.quality.EvaluateFloat();
				if (num != 0f)
				{
					string text = ((num > 0f) ? "+" : "-");
					this.Description = string.Format("{0}{1}{2}", worldZoneData.Definition.qualityIcon.FontIcon(), text, Math.Abs(num));
				}
			}
		}
	}

	// Token: 0x06003C77 RID: 15479 RVA: 0x00120EB0 File Offset: 0x0011F0B0
	public List<NeedItemData> GetCurrentNeedItems()
	{
		List<NeedItemData> list = new List<NeedItemData>();
		for (int i = 0; i < this.CraftItemCellsData.Count; i++)
		{
			list.Add(this.CraftItemCellsData[i].currentItem);
		}
		return list;
	}

	// Token: 0x06003C78 RID: 15480 RVA: 0x00120EF4 File Offset: 0x0011F0F4
	private void FillCraftItemCellsData()
	{
		if (this.CraftItemCellsData != null)
		{
			this.CraftItemCellsData.Clear();
		}
		else
		{
			this.CraftItemCellsData = new List<UICraftItemCellData>();
		}
		if (this.BuildData.NeedItems != null)
		{
			foreach (NeedItemData needItemData in this.BuildData.NeedItems)
			{
				this.CraftItemCellsData.Add(new UICraftItemCellData(needItemData, this.MultiInventory, null, 0f, "", null));
			}
		}
	}
}
