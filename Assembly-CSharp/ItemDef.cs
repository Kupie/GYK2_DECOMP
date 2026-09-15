using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F5 RID: 501
[Serializable]
public class ItemDef : BalanceBaseObject
{
	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000C60 RID: 3168 RVA: 0x0003E80C File Offset: 0x0003CA0C
	public bool CanBeUsed
	{
		get
		{
			return this.canBeUsed.HasExpression && this.canBeUsed.EvaluateBool();
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000C61 RID: 3169 RVA: 0x0003E828 File Offset: 0x0003CA28
	public bool CanBePinnedToHotBar
	{
		get
		{
			return this.CanBeUsed || this.isSeed || this.isFertilizer || this.isBattlePotion;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0003E84A File Offset: 0x0003CA4A
	public bool CanNotBeDestroyed
	{
		get
		{
			return this.canNotBeDestroyed.EvaluateBool();
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000C63 RID: 3171 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public bool IsAnimationDrivenTool
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x0003E858 File Offset: 0x0003CA58
	public ItemDef Copy()
	{
		return new ItemDef
		{
			itemGroupIds = this.itemGroupIds,
			stackCount = this.stackCount,
			customIcon = this.customIcon,
			type = this.type,
			itemSize = this.itemSize,
			inventorySize = this.inventorySize,
			quality = this.quality,
			qualityType = this.qualityType,
			talentBonus = this.talentBonus,
			talentType = this.talentType,
			talentValue = this.talentValue,
			canBeUsed = this.canBeUsed,
			gameResOnUse = this.gameResOnUse,
			onUseExpressions = this.onUseExpressions,
			canNotBeDestroyed = this.canNotBeDestroyed,
			hasDurability = this.hasDurability,
			durDecreaseOnUse = this.durDecreaseOnUse,
			canBeUsedInAlchemy = this.canBeUsedInAlchemy,
			runesRed = this.runesRed,
			runesGreen = this.runesGreen,
			runesBlue = this.runesBlue,
			onDropCollected = this.onDropCollected,
			isLinkedToWgo = this.isLinkedToWgo,
			redSkulls = this.redSkulls,
			whiteSkulls = this.whiteSkulls,
			isProduct = this.isProduct,
			basePrice = this.basePrice,
			isStaticCost = this.isStaticCost,
			bagItemGroups = this.bagItemGroups,
			bagSizeX = this.bagSizeX,
			bagSizeY = this.bagSizeY,
			sortOrder = this.sortOrder,
			talentIds = this.talentIds,
			isBag = this.isBag,
			isSeed = this.isSeed,
			isFertilizer = this.isFertilizer,
			isTechPoint = this.isTechPoint,
			isTool = this.isTool,
			isWeapon = this.isWeapon,
			isFuel = this.isFuel,
			isMainOrgan = this.isMainOrgan,
			isOrganMistake = this.isOrganMistake,
			iconId = this.iconId
		};
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x0003EA70 File Offset: 0x0003CC70
	public GameRes GetGameResOnUse()
	{
		GameRes gameRes = new GameRes();
		for (int i = 0; i < this.gameResOnUse.Count; i++)
		{
			ExpressionGameRes expressionGameRes = this.gameResOnUse[i];
			float num = expressionGameRes.expression.EvaluateFloat();
			gameRes.Add(expressionGameRes.name, num);
		}
		return gameRes;
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x0003EAC0 File Offset: 0x0003CCC0
	public float GetGameResOnUse(string resId)
	{
		float num = 0f;
		for (int i = 0; i < this.gameResOnUse.Count; i++)
		{
			ExpressionGameRes expressionGameRes = this.gameResOnUse[i];
			if (expressionGameRes.name == resId)
			{
				num += expressionGameRes.expression.EvaluateFloat();
			}
		}
		return num;
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x0003EB14 File Offset: 0x0003CD14
	public bool HasGameResOnUse(string resId)
	{
		for (int i = 0; i < this.gameResOnUse.Count; i++)
		{
			if (this.gameResOnUse[i].name == resId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x0003EB53 File Offset: 0x0003CD53
	public bool HasAnyGameResOnUse()
	{
		return this.gameResOnUse.Count > 0 && !this.GetGameResOnUse().IsEmpty();
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x0003EB74 File Offset: 0x0003CD74
	public bool CanBeInsertedInBag(ItemDef bag)
	{
		if (this.isBag)
		{
			return false;
		}
		foreach (string text in bag.bagItemGroups)
		{
			if (this.itemGroupIds.Contains(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x0003EBE0 File Offset: 0x0003CDE0
	public bool CanItemBeEquipped()
	{
		ItemType itemType = this.type;
		return itemType - ItemType.Axe <= 4 || itemType - ItemType.Sword <= 1 || itemType - ItemType.Bow <= 5;
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x0003EC0C File Offset: 0x0003CE0C
	public bool IsFightingEquipment()
	{
		ItemType itemType = this.type;
		return itemType == ItemType.BodyArmor || itemType == ItemType.Sword || itemType == ItemType.Bow;
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x0003EC38 File Offset: 0x0003CE38
	public Vector3Int GetRunesAsVector3Int()
	{
		if (this.cachedBoostRunesAsVector3Int == default(Vector3Int))
		{
			this.cachedBoostRunesAsVector3Int = new Vector3Int(this.runesRed.EvaluateInt(), this.runesGreen.EvaluateInt(), this.runesBlue.EvaluateInt());
		}
		return this.cachedBoostRunesAsVector3Int;
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x0003EC90 File Offset: 0x0003CE90
	public string GetRunesAsString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = this.runesRed.EvaluateInt();
		int num2 = this.runesGreen.EvaluateInt();
		int num3 = this.runesBlue.EvaluateInt();
		if (num > 0)
		{
			stringBuilder.Append(string.Format("{0}{1}", "rune_r".FontIcon(), num));
		}
		if (num2 > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(string.Format("{0}{1}", "rune_g".FontIcon(), num2));
		}
		if (num3 > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(string.Format("{0}{1}", "rune_b".FontIcon(), num3));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x0003ED66 File Offset: 0x0003CF66
	public string GetSkullsAsString(TextStyle minusStyle)
	{
		return ItemDef.GetSkullsRangeAsString(this.redSkulls, this.redSkulls, this.whiteSkulls, this.whiteSkulls, minusStyle, null);
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x0003ED88 File Offset: 0x0003CF88
	public static string GetSkullsRangeAsString(int redMin, int redMax, int whiteMin, int whiteMax, TextStyle minusStyle, TextStyle valueStyle)
	{
		string empty = string.Empty;
		ItemDef.AppendSkullRange(ref empty, redMin, redMax, "rskull", minusStyle, valueStyle);
		ItemDef.AppendSkullRange(ref empty, whiteMin, whiteMax, "skull", minusStyle, valueStyle);
		return empty;
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x0003EDC0 File Offset: 0x0003CFC0
	private static void AppendSkullRange(ref string result, int min, int max, string iconId, TextStyle minusStyle, TextStyle valueStyle)
	{
		if (min == 0 && max == 0)
		{
			return;
		}
		if (!string.IsNullOrEmpty(result))
		{
			result += " ";
		}
		if (min == max)
		{
			if (min < 0)
			{
				result += minusStyle.ApplyStyleToString("-", false, true);
			}
			for (int i = 0; i < Math.Abs(min); i++)
			{
				result += iconId.FontIcon();
			}
			return;
		}
		if (max <= 0)
		{
			result += minusStyle.ApplyStyleToString("-", false, true);
			int num = Math.Min(Math.Abs(min), Math.Abs(max));
			int num2 = Math.Max(Math.Abs(min), Math.Abs(max));
			result = result + iconId.FontIcon() + ItemDef.FormatSkullRangeValue(num, num2, valueStyle);
			return;
		}
		result = result + iconId.FontIcon() + ItemDef.FormatSkullRangeValue(min, max, valueStyle);
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0003EEA0 File Offset: 0x0003D0A0
	private static string FormatSkullRangeValue(int min, int max, TextStyle valueStyle)
	{
		string text = string.Format("{0}-{1}", min, max);
		if (!(valueStyle != null))
		{
			return text;
		}
		return valueStyle.ApplyStyleToString(text, false, true);
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0003EED8 File Offset: 0x0003D0D8
	public List<PerkDef> GetPerksOnUse()
	{
		if (!this.perksCached)
		{
			this.perksCached = true;
			this.perksOnUseCache = new List<PerkDef>();
			for (int i = 0; i < this.onUseExpressions.Count; i++)
			{
				string rawExpressionString = this.onUseExpressions[i].GetRawExpressionString();
				if (rawExpressionString.StartsWith("AddPerk"))
				{
					PerkDef data = GameBalance.Me.GetData<PerkDef>(Regex.Match(rawExpressionString, "\\\"([^\\\"]+)\\\"").Groups[1].Value);
					if (data != null)
					{
						this.perksOnUseCache.Add(data);
					}
				}
			}
		}
		return this.perksOnUseCache;
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0003EF70 File Offset: 0x0003D170
	public bool HasOnUseFunction(string functionName)
	{
		if (this.onUseExpressions == null || string.IsNullOrEmpty(functionName))
		{
			return false;
		}
		for (int i = 0; i < this.onUseExpressions.Count; i++)
		{
			LazyExpression lazyExpression = this.onUseExpressions[i];
			string text = ((lazyExpression != null) ? lazyExpression.GetRawExpressionString() : null);
			if (!string.IsNullOrEmpty(text) && text.Contains(functionName))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0003EFD2 File Offset: 0x0003D1D2
	public string GetItemCraftPrefix()
	{
		if (this.itemGroupIds.Contains("bodypart"))
		{
			return LLBase.L("ui_extract");
		}
		if (this.type == ItemType.Embalm)
		{
			return LLBase.L("ui_insert");
		}
		return LLBase.L("ui_create");
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0003F010 File Offset: 0x0003D210
	public string GetHeader()
	{
		if (this.qualityType != ItemDef.QualityType.Star)
		{
			return LLBase.L(this.id);
		}
		if (LLBase.HasL(this.id))
		{
			return LLBase.L(this.id);
		}
		return LLBase.L(this.id.Split(':', StringSplitOptions.None)[0]);
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0003F060 File Offset: 0x0003D260
	public string GetDescription()
	{
		return LLBase.L(this.GetDescriptionLocale());
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0003F070 File Offset: 0x0003D270
	public string GetDescriptionLocale()
	{
		if (this.qualityType != ItemDef.QualityType.Star)
		{
			return this.id + "_d";
		}
		string text = this.id + "_d";
		if (LLBase.HasL(text))
		{
			return text;
		}
		return this.id.Split(':', StringSplitOptions.None)[0] + "_d";
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0003F0D0 File Offset: 0x0003D2D0
	public ItemDef GetMistakeForThisItem()
	{
		string text = null;
		ItemType itemType = this.type;
		switch (itemType)
		{
		case ItemType.Brain:
			text = "surgeon_mistake_brain";
			break;
		case ItemType.Heart:
			text = "surgeon_mistake_heart";
			break;
		case ItemType.Flesh:
			break;
		case ItemType.Bones:
			text = "surgeon_mistake_bones";
			break;
		default:
			switch (itemType)
			{
			case ItemType.Skull:
				text = "surgeon_mistake_skull";
				break;
			case ItemType.Guts:
				text = "surgeon_mistake_guts";
				break;
			case ItemType.Skin:
				text = "surgeon_mistake_skin";
				break;
			}
			break;
		}
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return GameBalance.Me.GetData<ItemDef>(text);
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x0003F15C File Offset: 0x0003D35C
	public bool SkullsInBorders(int white, int red)
	{
		if (this.type != ItemType.Collar)
		{
			Debug.LogError("Do not check skull borders on not collar items!!!");
			return false;
		}
		return (this.redSkullsMaxCollar <= this.redSkullsMinCollar || (red >= this.redSkullsMinCollar && red <= this.redSkullsMaxCollar)) && (this.whiteSkullsMaxCollar <= this.whiteSkullsMinCollar || (white >= this.whiteSkullsMinCollar && white <= this.whiteSkullsMaxCollar));
	}

	// Token: 0x04000E10 RID: 3600
	public const string EMPTY_ITEM_ID = "empty";

	// Token: 0x04000E11 RID: 3601
	public const string FAITH_ITEM_ID = "faith";

	// Token: 0x04000E12 RID: 3602
	[AutoParse("group")]
	public List<string> itemGroupIds = new List<string>();

	// Token: 0x04000E13 RID: 3603
	[AutoParse("stack_count")]
	public int stackCount;

	// Token: 0x04000E14 RID: 3604
	[SerializeField]
	[AutoParse("custom_icon")]
	private string customIcon;

	// Token: 0x04000E15 RID: 3605
	[AutoParse("type")]
	public ItemType type;

	// Token: 0x04000E16 RID: 3606
	public ItemSize itemSize;

	// Token: 0x04000E17 RID: 3607
	[AutoParse("inventory_size")]
	public int inventorySize;

	// Token: 0x04000E18 RID: 3608
	[AutoParse("quality")]
	public int quality;

	// Token: 0x04000E19 RID: 3609
	[AutoParse("quality_type")]
	public ItemDef.QualityType qualityType;

	// Token: 0x04000E1A RID: 3610
	[AutoParse("quality_icon")]
	public string qualityIcon;

	// Token: 0x04000E1B RID: 3611
	[AutoParse("talent_bonus")]
	public int talentBonus;

	// Token: 0x04000E1C RID: 3612
	[AutoParse("talent_type")]
	public string talentType;

	// Token: 0x04000E1D RID: 3613
	[AutoParse("talent_value")]
	public int talentValue;

	// Token: 0x04000E1E RID: 3614
	[AutoParse("can_be_used")]
	[LazyExpressionPureValueType(PureValueType.Bool)]
	public LazyExpression canBeUsed = new LazyExpression();

	// Token: 0x04000E1F RID: 3615
	[AutoParse("stay_on_use")]
	public bool stayOnUse;

	// Token: 0x04000E20 RID: 3616
	[AutoParse("add_res_on_use")]
	public List<ExpressionGameRes> gameResOnUse = new List<ExpressionGameRes>();

	// Token: 0x04000E21 RID: 3617
	[AutoParse("exp_on_use")]
	public List<LazyExpression> onUseExpressions;

	// Token: 0x04000E22 RID: 3618
	[AutoParse("on_use_sound")]
	public string onUseSound;

	// Token: 0x04000E23 RID: 3619
	[AutoParse("cannot_be_destr")]
	[LazyExpressionPureValueType(PureValueType.Bool)]
	public LazyExpression canNotBeDestroyed = new LazyExpression();

	// Token: 0x04000E24 RID: 3620
	[AutoParse("has_durability")]
	public bool hasDurability;

	// Token: 0x04000E25 RID: 3621
	[AutoParse("dur_decrease_on_use")]
	public float durDecreaseOnUse;

	// Token: 0x04000E26 RID: 3622
	[AutoParse("can_be_used_in_alchemy")]
	public bool canBeUsedInAlchemy;

	// Token: 0x04000E27 RID: 3623
	[AutoParse("runes_r")]
	public LazyExpression runesRed = new LazyExpression();

	// Token: 0x04000E28 RID: 3624
	[AutoParse("runes_g")]
	public LazyExpression runesGreen = new LazyExpression();

	// Token: 0x04000E29 RID: 3625
	[AutoParse("runes_b")]
	public LazyExpression runesBlue = new LazyExpression();

	// Token: 0x04000E2A RID: 3626
	[AutoParse("on_drop_collected")]
	public List<LazyExpression> onDropCollected = new List<LazyExpression>();

	// Token: 0x04000E2B RID: 3627
	[AutoParse("is_linked_to_wgo")]
	public bool isLinkedToWgo;

	// Token: 0x04000E2C RID: 3628
	[AutoParse("red_skulls")]
	public int redSkulls;

	// Token: 0x04000E2D RID: 3629
	[AutoParse("white_skulls")]
	public int whiteSkulls;

	// Token: 0x04000E2E RID: 3630
	[AutoParse("body_linked_perk")]
	public string bodyLinkedPerk;

	// Token: 0x04000E2F RID: 3631
	public int redSkullsMinCollar;

	// Token: 0x04000E30 RID: 3632
	public int redSkullsMaxCollar;

	// Token: 0x04000E31 RID: 3633
	public int whiteSkullsMinCollar;

	// Token: 0x04000E32 RID: 3634
	public int whiteSkullsMaxCollar;

	// Token: 0x04000E33 RID: 3635
	[AutoParse("is_product")]
	public bool isProduct;

	// Token: 0x04000E34 RID: 3636
	[AutoParse("base_price")]
	public int basePrice;

	// Token: 0x04000E35 RID: 3637
	[AutoParse("is_static_cost")]
	public bool isStaticCost;

	// Token: 0x04000E36 RID: 3638
	[AutoParse("bag_item_groups")]
	public List<string> bagItemGroups;

	// Token: 0x04000E37 RID: 3639
	[AutoParse("bag_size_x")]
	public int bagSizeX;

	// Token: 0x04000E38 RID: 3640
	[AutoParse("bag_size_y")]
	public int bagSizeY;

	// Token: 0x04000E39 RID: 3641
	[AutoParse("stamina_cost")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression staminaCost = new LazyExpression();

	// Token: 0x04000E3A RID: 3642
	[AutoParse("damage")]
	public LazyExpression damage;

	// Token: 0x04000E3B RID: 3643
	[AutoParse("atk_range")]
	public float atkRange;

	// Token: 0x04000E3C RID: 3644
	[AutoParse("atk_pause")]
	public float atkPause;

	// Token: 0x04000E3D RID: 3645
	[AutoParse("knockback_force")]
	public float knockbackForce;

	// Token: 0x04000E3E RID: 3646
	[AutoParse("target_filter_dock_point_tag")]
	public DockPointTag dockPointTag;

	// Token: 0x04000E3F RID: 3647
	[AutoParse("expr_on_sell")]
	public List<LazyExpression> expressionsOnSell = new List<LazyExpression>();

	// Token: 0x04000E40 RID: 3648
	[AutoParse("expr_on_buy")]
	public List<LazyExpression> expressionsOnBuy = new List<LazyExpression>();

	// Token: 0x04000E41 RID: 3649
	public bool isBag;

	// Token: 0x04000E42 RID: 3650
	public bool isSeed;

	// Token: 0x04000E43 RID: 3651
	public bool isFertilizer;

	// Token: 0x04000E44 RID: 3652
	public bool isBattlePotion;

	// Token: 0x04000E45 RID: 3653
	public bool isTechPoint;

	// Token: 0x04000E46 RID: 3654
	public bool isTool;

	// Token: 0x04000E47 RID: 3655
	public bool isWeapon;

	// Token: 0x04000E48 RID: 3656
	public bool isFuel;

	// Token: 0x04000E49 RID: 3657
	public bool isMainOrgan;

	// Token: 0x04000E4A RID: 3658
	public bool isOrganMistake;

	// Token: 0x04000E4B RID: 3659
	public string iconId;

	// Token: 0x04000E4C RID: 3660
	public int bagSize;

	// Token: 0x04000E4D RID: 3661
	public int sortOrder;

	// Token: 0x04000E4E RID: 3662
	public List<string> talentIds = new List<string>();

	// Token: 0x04000E4F RID: 3663
	private List<PerkDef> perksOnUseCache;

	// Token: 0x04000E50 RID: 3664
	private bool perksCached;

	// Token: 0x04000E51 RID: 3665
	private Vector3Int cachedBoostRunesAsVector3Int;

	// Token: 0x020001F6 RID: 502
	public enum QualityType
	{
		// Token: 0x04000E53 RID: 3667
		None,
		// Token: 0x04000E54 RID: 3668
		Star
	}
}
