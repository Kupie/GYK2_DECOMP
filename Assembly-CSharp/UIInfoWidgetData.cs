using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200089E RID: 2206
public class UIInfoWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x060038E5 RID: 14565 RVA: 0x00111D9B File Offset: 0x0010FF9B
	// (set) Token: 0x060038E6 RID: 14566 RVA: 0x00111DA3 File Offset: 0x0010FFA3
	public CraftComponent CraftComponent { get; set; }

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x060038E7 RID: 14567 RVA: 0x00111DAC File Offset: 0x0010FFAC
	// (set) Token: 0x060038E8 RID: 14568 RVA: 0x00111DB4 File Offset: 0x0010FFB4
	public Sprite Icon { get; set; }

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x060038E9 RID: 14569 RVA: 0x00111DBD File Offset: 0x0010FFBD
	// (set) Token: 0x060038EA RID: 14570 RVA: 0x00111DC5 File Offset: 0x0010FFC5
	public string Header { get; private set; }

	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x060038EB RID: 14571 RVA: 0x00111DCE File Offset: 0x0010FFCE
	// (set) Token: 0x060038EC RID: 14572 RVA: 0x00111DD6 File Offset: 0x0010FFD6
	public string Description { get; private set; }

	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x060038ED RID: 14573 RVA: 0x00111DDF File Offset: 0x0010FFDF
	// (set) Token: 0x060038EE RID: 14574 RVA: 0x00111DE7 File Offset: 0x0010FFE7
	public string WorldZoneQuality { get; private set; }

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x060038EF RID: 14575 RVA: 0x00111DF0 File Offset: 0x0010FFF0
	// (set) Token: 0x060038F0 RID: 14576 RVA: 0x00111DF8 File Offset: 0x0010FFF8
	public WgoData WgoData { get; private set; }

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x060038F1 RID: 14577 RVA: 0x00111E01 File Offset: 0x00110001
	// (set) Token: 0x060038F2 RID: 14578 RVA: 0x00111E09 File Offset: 0x00110009
	public CraftDef CraftDef { get; private set; }

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x060038F3 RID: 14579 RVA: 0x00111E12 File Offset: 0x00110012
	// (set) Token: 0x060038F4 RID: 14580 RVA: 0x00111E1A File Offset: 0x0011001A
	public bool ShowTickDuration { get; private set; }

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x060038F5 RID: 14581 RVA: 0x00111E23 File Offset: 0x00110023
	// (set) Token: 0x060038F6 RID: 14582 RVA: 0x00111E2B File Offset: 0x0011002B
	public bool ExcludePlayerFromMultiinventoryWhenCountItemsForFuel { get; set; } = true;

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x060038F7 RID: 14583 RVA: 0x00111E34 File Offset: 0x00110034
	// (set) Token: 0x060038F8 RID: 14584 RVA: 0x00111E3C File Offset: 0x0011003C
	public IWorker ForcedWorker { get; set; }

	// Token: 0x1700087A RID: 2170
	// (get) Token: 0x060038F9 RID: 14585 RVA: 0x00111E45 File Offset: 0x00110045
	// (set) Token: 0x060038FA RID: 14586 RVA: 0x00111E4D File Offset: 0x0011004D
	public bool DefineIconBackgroundFromWgo { get; private set; }

	// Token: 0x1700087B RID: 2171
	// (get) Token: 0x060038FB RID: 14587 RVA: 0x00111E56 File Offset: 0x00110056
	public IWorker Worker
	{
		get
		{
			if (this.ForcedWorker != null)
			{
				return this.ForcedWorker;
			}
			return this.WgoData.Worker;
		}
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x00111E74 File Offset: 0x00110074
	public UIInfoWidgetData(WgoData wgoData, string customBuildDeskIcon = null, bool defineIconBackgroundFromWgo = true)
	{
		this.Header = LLBase.L(wgoData.id);
		string text = wgoData.id + "_d";
		this.Description = LLBase.L(wgoData.id + "_d");
		if (this.Description == text)
		{
			this.Description = string.Empty;
		}
		this.CraftComponent = wgoData.CraftComponent;
		this.WgoData = wgoData;
		this.CraftDef = null;
		this.ShowTickDuration = true;
		BuildingDef buildingDef;
		if (!string.IsNullOrEmpty(customBuildDeskIcon))
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(customBuildDeskIcon, null);
		}
		else if (wgoData.Definition.TryGetBuildingDefForWgo(out buildingDef))
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(buildingDef.BuildResultIcon, null);
		}
		else if (wgoData.Definition.interactionType == WGODef.InteractionType.Craft)
		{
			if (!string.IsNullOrEmpty(wgoData.Definition.craftIconId))
			{
				this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(wgoData.Definition.craftIconId, null);
			}
			else
			{
				this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("i_b_" + wgoData.id, null);
			}
		}
		else if (wgoData.WorldZoneData != null)
		{
			if (wgoData.Definition.interactionType != WGODef.InteractionType.Embalm && !string.IsNullOrEmpty(wgoData.WorldZoneData.Definition.qualityIcon))
			{
				this.WorldZoneQuality = string.Format("{0}{1}", wgoData.WorldZoneData.Definition.qualityIcon.FontIcon(), wgoData.WorldZoneData.GetTotalQuality());
			}
			string text2 = (string.IsNullOrEmpty(wgoData.WorldZoneData.Definition.buildDeskIcon) ? ("i_z_" + wgoData.WorldZoneData.id) : wgoData.WorldZoneData.Definition.buildDeskIcon);
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text2, null);
		}
		else
		{
			WGODef.InteractionType interactionType = wgoData.Definition.interactionType;
			WorldZoneData worldZoneData;
			if ((interactionType == WGODef.InteractionType.Builder || interactionType == WGODef.InteractionType.FightBuilder) && wgoData.TryGetNearestBuilderWorldZone(out worldZoneData))
			{
				this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(worldZoneData.Definition.buildDeskIcon, null);
			}
		}
		this.TryFillWorldZoneQuality(wgoData);
		if (this.Icon == null)
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("i_b_hammer", null);
		}
		this.DefineIconBackgroundFromWgo = defineIconBackgroundFromWgo;
	}

	// Token: 0x060038FD RID: 14589 RVA: 0x001120E0 File Offset: 0x001102E0
	private void TryFillWorldZoneQuality(WgoData wgoData)
	{
		if (!string.IsNullOrEmpty(this.WorldZoneQuality) || wgoData == null)
		{
			return;
		}
		WGODef definition = wgoData.Definition;
		if (definition != null && definition.interactionType == WGODef.InteractionType.Embalm)
		{
			return;
		}
		WorldZoneData worldZoneData = wgoData.WorldZoneData;
		if (worldZoneData == null)
		{
			wgoData.TryGetNearestBuilderWorldZone(out worldZoneData);
		}
		if (((worldZoneData != null) ? worldZoneData.Definition : null) == null || string.IsNullOrEmpty(worldZoneData.Definition.qualityIcon))
		{
			return;
		}
		string qualityIcon = worldZoneData.Definition.qualityIcon;
		if (qualityIcon != "gear" && qualityIcon != "corpse" && qualityIcon != "body")
		{
			return;
		}
		this.WorldZoneQuality = string.Format("{0}{1}", qualityIcon.FontIcon(), worldZoneData.GetTotalQuality());
	}

	// Token: 0x060038FE RID: 14590 RVA: 0x001121A0 File Offset: 0x001103A0
	public UIInfoWidgetData(WgoData wgoData, Item item, bool defineIconBackgroundFromWgo = true)
	{
		this.Header = item.Definition.GetHeader();
		string descriptionLocale = item.Definition.GetDescriptionLocale();
		this.Description = LLBase.L(descriptionLocale);
		if (this.Description == descriptionLocale)
		{
			this.Description = string.Empty;
		}
		BuildingDef buildingDef;
		if (wgoData != null && wgoData.Definition.TryGetBuildingDefForWgo(out buildingDef))
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(buildingDef.BuildResultIcon, null);
		}
		else
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("i_b_hammer", null);
		}
		this.WgoData = wgoData;
		this.CraftDef = null;
		this.DefineIconBackgroundFromWgo = defineIconBackgroundFromWgo;
	}

	// Token: 0x060038FF RID: 14591 RVA: 0x00112254 File Offset: 0x00110454
	public UIInfoWidgetData(WgoData wgoData, CraftDef craftDef, bool defineIconBackgroundFromWgo = true)
	{
		this.Header = LLBase.L(wgoData.id);
		string text = craftDef.id + "_d";
		this.Description = LLBase.L(text);
		if (this.Description == text)
		{
			this.Description = string.Empty;
		}
		BuildingDef buildingDef;
		if (!string.IsNullOrEmpty(wgoData.Definition.craftIconId))
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(wgoData.Definition.craftIconId, null);
		}
		else if (wgoData.Definition.TryGetBuildingDefForWgo(out buildingDef))
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(buildingDef.BuildResultIcon, null);
		}
		else
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("i_b_hammer", null);
		}
		this.CraftComponent = wgoData.CraftComponent;
		this.WgoData = wgoData;
		this.CraftDef = craftDef;
		this.ShowTickDuration = false;
		this.DefineIconBackgroundFromWgo = defineIconBackgroundFromWgo;
	}
}
