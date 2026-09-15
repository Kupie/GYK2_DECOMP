using System;
using System.Text;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AAC RID: 2732
public static class Param
{
	// Token: 0x060049E0 RID: 18912 RVA: 0x0015CE70 File Offset: 0x0015B070
	public static string FormIconFromPlayerRes(string param)
	{
		return param.FontIcon() + MainGame.PlayerData.GetRes(param, 0f).ToString();
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x0015CEA0 File Offset: 0x0015B0A0
	public static string ToFormattedString(this GameRes gameRes, bool showOnlyType = false, Func<string, string, string> overrodePattern = null, bool ignoreZeroValues = false, bool appendSpace = true, GameResIconType iconType = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (GameResAtom gameResAtom in gameRes.List)
		{
			stringBuilder.Append(gameResAtom.ToFormattedString(showOnlyType, overrodePattern, ignoreZeroValues, appendSpace, iconType));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x0015CF0C File Offset: 0x0015B10C
	public static string ToFormattedString(this GameResAtom atom, bool showOnlyType = false, Func<string, string, string> overrodePattern = null, bool ignoreZeroValues = false, bool appendSpace = true, GameResIconType iconType = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (iconType == null)
		{
			iconType = GameResIconType.Common;
		}
		if (atom.type == "money")
		{
			stringBuilder.Append(Trading.FormatMoney(Mathf.CeilToInt(atom.value), false, " ", iconType));
			return stringBuilder.ToString();
		}
		int num = (int)atom.value;
		if (num == 0 && !showOnlyType && !ignoreZeroValues)
		{
			return string.Empty;
		}
		if (stringBuilder.Length > 0 && appendSpace)
		{
			stringBuilder.Append(" ");
		}
		GameResIconConfig configForRes = GameResDisplayConfig.GetConfigForRes(atom.type, iconType);
		string text;
		if (configForRes != null)
		{
			text = configForRes.iconName.FontIcon();
		}
		else if (atom.type.StartsWith("wz_"))
		{
			string text2 = atom.type.Replace("wz_", "");
			WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.worldData.GetWorldZoneDataById(text2);
			text = ((worldZoneDataById != null) ? worldZoneDataById.Definition.qualityIcon.FontIcon() : null);
		}
		else if (atom.type.EndsWith("_REP"))
		{
			text = "icon_smile02".FontIcon();
		}
		else
		{
			text = atom.type.FontIcon();
		}
		if (overrodePattern == null)
		{
			stringBuilder.Append(text);
			if (!showOnlyType)
			{
				stringBuilder.Append((num >= 0) ? "+" : "-");
				stringBuilder.Append(Mathf.Abs(num));
			}
		}
		else
		{
			string text3 = overrodePattern(text, num.ToString());
			stringBuilder.Append(text3);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x0015D08B File Offset: 0x0015B28B
	public static Item ItemFromAtom(this GameResAtom atom)
	{
		return new Item("game_res_" + atom.type, (int)atom.value);
	}
}
