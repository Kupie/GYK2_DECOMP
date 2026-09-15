using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020008C2 RID: 2242
public class LinkedEntityWidgetData : LazyWidgetDataBase
{
	// Token: 0x170008C8 RID: 2248
	// (get) Token: 0x06003A79 RID: 14969 RVA: 0x00117D3D File Offset: 0x00115F3D
	// (set) Token: 0x06003A7A RID: 14970 RVA: 0x00117D45 File Offset: 0x00115F45
	public Sprite Icon { get; private set; }

	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x06003A7B RID: 14971 RVA: 0x00117D4E File Offset: 0x00115F4E
	// (set) Token: 0x06003A7C RID: 14972 RVA: 0x00117D56 File Offset: 0x00115F56
	public string FontIcon { get; private set; }

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x06003A7D RID: 14973 RVA: 0x00117D5F File Offset: 0x00115F5F
	// (set) Token: 0x06003A7E RID: 14974 RVA: 0x00117D67 File Offset: 0x00115F67
	public Sprite QualityIcon { get; private set; }

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x06003A7F RID: 14975 RVA: 0x00117D70 File Offset: 0x00115F70
	// (set) Token: 0x06003A80 RID: 14976 RVA: 0x00117D78 File Offset: 0x00115F78
	public LinkedEntityType LinkedEntityType { get; private set; }

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x06003A81 RID: 14977 RVA: 0x00117D81 File Offset: 0x00115F81
	// (set) Token: 0x06003A82 RID: 14978 RVA: 0x00117D89 File Offset: 0x00115F89
	public CraftDef CraftDef { get; private set; }

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x06003A83 RID: 14979 RVA: 0x00117D92 File Offset: 0x00115F92
	// (set) Token: 0x06003A84 RID: 14980 RVA: 0x00117D9A File Offset: 0x00115F9A
	public AlchemyFormulaDef AlchemyFormulaDef { get; private set; }

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x06003A85 RID: 14981 RVA: 0x00117DA3 File Offset: 0x00115FA3
	// (set) Token: 0x06003A86 RID: 14982 RVA: 0x00117DAB File Offset: 0x00115FAB
	public ItemDef ItemDef { get; private set; }

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x06003A87 RID: 14983 RVA: 0x00117DB4 File Offset: 0x00115FB4
	// (set) Token: 0x06003A88 RID: 14984 RVA: 0x00117DBC File Offset: 0x00115FBC
	public Item Item { get; private set; }

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x06003A89 RID: 14985 RVA: 0x00117DC5 File Offset: 0x00115FC5
	// (set) Token: 0x06003A8A RID: 14986 RVA: 0x00117DCD File Offset: 0x00115FCD
	public VendorOrderDef VendorOrderDef { get; private set; }

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x06003A8B RID: 14987 RVA: 0x00117DD6 File Offset: 0x00115FD6
	// (set) Token: 0x06003A8C RID: 14988 RVA: 0x00117DDE File Offset: 0x00115FDE
	public GameRes GameRes { get; private set; }

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x06003A8D RID: 14989 RVA: 0x00117DE7 File Offset: 0x00115FE7
	// (set) Token: 0x06003A8E RID: 14990 RVA: 0x00117DEF File Offset: 0x00115FEF
	public BuildingDef BuildingDef { get; private set; }

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x06003A8F RID: 14991 RVA: 0x00117DF8 File Offset: 0x00115FF8
	// (set) Token: 0x06003A90 RID: 14992 RVA: 0x00117E00 File Offset: 0x00116000
	public TownBuildingDef TownBuildingDef { get; private set; }

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x06003A91 RID: 14993 RVA: 0x00117E09 File Offset: 0x00116009
	// (set) Token: 0x06003A92 RID: 14994 RVA: 0x00117E11 File Offset: 0x00116011
	public PerkDef PerkDef { get; private set; }

	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x06003A93 RID: 14995 RVA: 0x00117E1A File Offset: 0x0011601A
	// (set) Token: 0x06003A94 RID: 14996 RVA: 0x00117E22 File Offset: 0x00116022
	public Action OnClicked { get; set; }

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x06003A95 RID: 14997 RVA: 0x00117E2B File Offset: 0x0011602B
	// (set) Token: 0x06003A96 RID: 14998 RVA: 0x00117E33 File Offset: 0x00116033
	public string LabelText { get; private set; }

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x06003A97 RID: 14999 RVA: 0x00117E3C File Offset: 0x0011603C
	// (set) Token: 0x06003A98 RID: 15000 RVA: 0x00117E44 File Offset: 0x00116044
	public TextStyle LabelCustomStyle { get; set; }

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x06003A99 RID: 15001 RVA: 0x00117E4D File Offset: 0x0011604D
	// (set) Token: 0x06003A9A RID: 15002 RVA: 0x00117E55 File Offset: 0x00116055
	public bool NoSelectionFrames { get; set; }

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x06003A9B RID: 15003 RVA: 0x00117E5E File Offset: 0x0011605E
	// (set) Token: 0x06003A9C RID: 15004 RVA: 0x00117E66 File Offset: 0x00116066
	public bool NotShowStudyWidgetInItemTooltips { get; set; }

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x06003A9D RID: 15005 RVA: 0x00117E6F File Offset: 0x0011606F
	// (set) Token: 0x06003A9E RID: 15006 RVA: 0x00117E77 File Offset: 0x00116077
	public bool ShowCraftedAtFromCraftDefInsteadOfItem { get; set; }

	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x06003A9F RID: 15007 RVA: 0x00117E80 File Offset: 0x00116080
	// (set) Token: 0x06003AA0 RID: 15008 RVA: 0x00117E88 File Offset: 0x00116088
	public bool IsInactive { get; set; }

	// Token: 0x06003AA1 RID: 15009 RVA: 0x00117E94 File Offset: 0x00116094
	public LinkedEntityWidgetData(CraftDef craftDef, Action onClicked)
	{
		ItemDef itemDef = craftDef.TryGetResultingItemDef(true);
		string craftResultIcon = craftDef.GetCraftResultIcon(null);
		if (string.IsNullOrEmpty(craftResultIcon))
		{
			if (itemDef != null)
			{
				this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(itemDef.iconId, null);
			}
			else
			{
				this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(craftDef.GetOutputPreview(null).IconId, null);
			}
		}
		else
		{
			this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(craftResultIcon, null);
		}
		this.LinkedEntityType = LinkedEntityType.CraftDef;
		this.CraftDef = craftDef;
		this.QualityIcon = null;
		this.OnClicked = onClicked;
		if (!craftDef.isStarCraft && itemDef != null && itemDef.qualityType == ItemDef.QualityType.Star)
		{
			this.QualityIcon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + itemDef.quality.ToString(), null);
		}
	}

	// Token: 0x06003AA2 RID: 15010 RVA: 0x00117F64 File Offset: 0x00116164
	public LinkedEntityWidgetData(AlchemyFormulaDef alchemyFormulaDef, Action onClicked)
	{
		ItemDef itemDef = alchemyFormulaDef.ItemDef;
		this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(itemDef.iconId, null);
		this.LinkedEntityType = LinkedEntityType.AlchemyFormula;
		this.AlchemyFormulaDef = alchemyFormulaDef;
		this.QualityIcon = null;
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AA3 RID: 15011 RVA: 0x00117FB4 File Offset: 0x001161B4
	public LinkedEntityWidgetData(ItemDef itemDef, Action onClicked)
	{
		this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(itemDef.iconId, null);
		this.QualityIcon = null;
		this.ItemDef = itemDef;
		this.LinkedEntityType = LinkedEntityType.ItemDef;
		if (itemDef.qualityType == ItemDef.QualityType.Star)
		{
			this.QualityIcon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + itemDef.quality.ToString(), null);
		}
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AA4 RID: 15012 RVA: 0x0011802C File Offset: 0x0011622C
	public LinkedEntityWidgetData(Item item, Action onClicked)
	{
		this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(item.Definition.iconId, null);
		this.QualityIcon = null;
		this.Item = item;
		this.LinkedEntityType = LinkedEntityType.Item;
		if (item.Definition.qualityType == ItemDef.QualityType.Star)
		{
			this.QualityIcon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite("item_star_" + item.Definition.quality.ToString(), null);
		}
		if (item.Count > 1)
		{
			this.LabelText = item.Count.ToString();
		}
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AA5 RID: 15013 RVA: 0x001180D0 File Offset: 0x001162D0
	public LinkedEntityWidgetData(string res, int count, Action onClicked)
	{
		this.Icon = null;
		this.GameRes = new GameRes(res, (float)count);
		this.FontIcon = this.GameRes.ToFormattedString(false, (string s, string s1) => s + s1, false, true, null);
		this.QualityIcon = null;
		this.LinkedEntityType = LinkedEntityType.GameRes;
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AA6 RID: 15014 RVA: 0x00118141 File Offset: 0x00116341
	public LinkedEntityWidgetData(string dayNumber, Action onClicked)
	{
		this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(dayNumber, null);
		this.QualityIcon = null;
		this.LinkedEntityType = LinkedEntityType.DayNumber;
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AA7 RID: 15015 RVA: 0x00118170 File Offset: 0x00116370
	public LinkedEntityWidgetData(VendorOrderDef vendorOrder, Action onClicked)
	{
		this.VendorOrderDef = vendorOrder;
		this.ItemDef = GameBalance.Me.GetData<ItemDef>(vendorOrder.itemId);
		this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.ItemDef.iconId, null);
		this.QualityIcon = null;
		this.LinkedEntityType = LinkedEntityType.Order;
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AA8 RID: 15016 RVA: 0x001181D4 File Offset: 0x001163D4
	public LinkedEntityWidgetData(BuildingDef buildingDef, Action onClicked, int count = 1, bool isInactive = false)
	{
		this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(buildingDef.BuildResultIcon, null);
		this.BuildingDef = buildingDef;
		this.LinkedEntityType = LinkedEntityType.BuildingDef;
		this.QualityIcon = null;
		this.OnClicked = onClicked;
		if (count > 1)
		{
			this.LabelText = count.ToString();
		}
		this.IsInactive = isInactive;
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x00118233 File Offset: 0x00116433
	public LinkedEntityWidgetData(PerkDef perkDef, Action onClicked)
	{
		this.Icon = perkDef.Icon;
		this.PerkDef = perkDef;
		this.LinkedEntityType = LinkedEntityType.PerkDef;
		this.QualityIcon = null;
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x00118263 File Offset: 0x00116463
	public LinkedEntityWidgetData(TownBuildingDef townBuildingDef, Action onClicked)
	{
		this.Icon = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(townBuildingDef.BuildResultIcon, null);
		this.TownBuildingDef = townBuildingDef;
		this.LinkedEntityType = LinkedEntityType.TownBuildingDef;
		this.QualityIcon = null;
		this.OnClicked = onClicked;
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x001182A0 File Offset: 0x001164A0
	public string GetLinkedEntityPrefix()
	{
		switch (this.LinkedEntityType)
		{
		case LinkedEntityType.CraftDef:
		{
			if (this.CraftDef.id.StartsWith("fake_"))
			{
				return LLBase.L("ui_create");
			}
			ItemDef itemDef = this.CraftDef.TryGetResultingItemDef(true);
			if (itemDef != null)
			{
				return itemDef.GetItemCraftPrefix();
			}
			return string.Empty;
		}
		case LinkedEntityType.ItemDef:
			return this.ItemDef.GetItemCraftPrefix();
		case LinkedEntityType.BuildingDef:
			return this.BuildingDef.GetHeaderPrefix();
		case LinkedEntityType.PerkDef:
			return this.PerkDef.GetHeaderPrefix();
		case LinkedEntityType.Item:
			return this.Item.Definition.GetItemCraftPrefix();
		case LinkedEntityType.AlchemyFormula:
			return this.AlchemyFormulaDef.ItemDef.GetItemCraftPrefix();
		case LinkedEntityType.Order:
			return LLBase.L("ui_order_header_tooltip");
		case LinkedEntityType.TownBuildingDef:
			return this.TownBuildingDef.GetHeaderPrefix();
		}
		throw new ArgumentOutOfRangeException("LinkedEntityType", this.LinkedEntityType, null);
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x0011839C File Offset: 0x0011659C
	public string GetLinkedEntityHeader()
	{
		switch (this.LinkedEntityType)
		{
		case LinkedEntityType.CraftDef:
		{
			if (this.CraftDef.id.StartsWith("fake_"))
			{
				return LLBase.L(this.CraftDef.id);
			}
			ItemDef itemDef = this.CraftDef.TryGetResultingItemDef(true);
			if (itemDef != null)
			{
				return itemDef.GetHeader();
			}
			return LLBase.L(this.CraftDef.id);
		}
		case LinkedEntityType.ItemDef:
			return this.ItemDef.GetHeader();
		case LinkedEntityType.BuildingDef:
			return this.BuildingDef.GetHeader();
		case LinkedEntityType.PerkDef:
			return this.PerkDef.GetHeader();
		case LinkedEntityType.Item:
			return this.Item.Definition.GetHeader();
		case LinkedEntityType.AlchemyFormula:
			return this.AlchemyFormulaDef.ItemDef.GetHeader();
		case LinkedEntityType.Order:
		{
			ItemDef data = GameBalance.Me.GetData<ItemDef>(this.VendorOrderDef.itemId);
			return string.Format("[{0}]x{1}", data.GetHeader(), this.VendorOrderDef.count);
		}
		case LinkedEntityType.TownBuildingDef:
			return this.TownBuildingDef.GetHeader();
		}
		throw new ArgumentOutOfRangeException("LinkedEntityType", this.LinkedEntityType, null);
	}
}
