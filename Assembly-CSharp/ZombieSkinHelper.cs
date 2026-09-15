using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public static class ZombieSkinHelper
{
	// Token: 0x060029C6 RID: 10694 RVA: 0x000C50CC File Offset: 0x000C32CC
	public static SkinPresetGK2 GetPresetForCustomizationData(string dataId, int body, int head, string bodyLut, string headLut)
	{
		SkinPresetGK2 skinPresetGK = ScriptableObject.CreateInstance<SkinPresetGK2>();
		skinPresetGK.body = new SkinPresetPartGK2
		{
			id = body,
			colorReplaceType = ColorReplaceType.USE_LUT_COLOR_REPLACE,
			palette = (string.IsNullOrEmpty(bodyLut) ? null : ZombieCustomizationConfig.GetBodyTextureByName(dataId, bodyLut))
		};
		skinPresetGK.head = new SkinPresetPartGK2
		{
			id = head,
			colorReplaceType = ColorReplaceType.USE_LUT_COLOR_REPLACE,
			palette = (string.IsNullOrEmpty(headLut) ? null : ZombieCustomizationConfig.GetHeadTextureByName(dataId, headLut))
		};
		skinPresetGK.arms = new SkinPresetPartGK2();
		skinPresetGK.beard = new SkinPresetPartGK2();
		skinPresetGK.hairstyle = new SkinPresetPartGK2();
		return skinPresetGK;
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x000C5164 File Offset: 0x000C3364
	public static SkinPresetGK2 GetPresetForWgoData(WgoData wgoData, string dataId)
	{
		return ZombieSkinHelper.GetPresetForCustomizationData(dataId, wgoData.GetGameResInt("zombie_body_id"), wgoData.GetGameResInt("zombie_head_id"), wgoData.GameResStr.Get("zombie_body_lut", ""), wgoData.GameResStr.Get("zombie_head_lut", ""));
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x000C51B8 File Offset: 0x000C33B8
	public static SkinPresetGK2 CopySkinPresetAndChangeHead(SkinPresetGK2 source, WgoData wgoData, string dataId)
	{
		SkinPresetGK2 skinPresetGK = ScriptableObject.CreateInstance<SkinPresetGK2>();
		skinPresetGK.body = source.body;
		skinPresetGK.hairstyle = source.hairstyle;
		skinPresetGK.arms = source.arms;
		skinPresetGK.beard = source.beard;
		int gameResInt = wgoData.GetGameResInt("zombie_head_id");
		string text = wgoData.GameResStr.Get("zombie_head_lut", "");
		skinPresetGK.head = new SkinPresetPartGK2
		{
			id = gameResInt,
			colorReplaceType = ColorReplaceType.USE_LUT_COLOR_REPLACE,
			palette = (string.IsNullOrEmpty(text) ? null : ZombieCustomizationConfig.GetHeadTextureByName(dataId, text))
		};
		return skinPresetGK;
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x000C524D File Offset: 0x000C344D
	[return: TupleElementNames(new string[] { "body", "head", "bodyLut", "headLut" })]
	public static ValueTuple<int, int, string, string> RollZombie(string dataId)
	{
		int randomBody = ZombieCustomizationConfig.GetRandomBody(dataId);
		int randomHead = ZombieCustomizationConfig.GetRandomHead(dataId);
		Texture2D randomBodyLut = ZombieCustomizationConfig.GetRandomBodyLut(dataId);
		string text = ((randomBodyLut != null) ? randomBodyLut.name : null);
		Texture2D randomHeadLut = ZombieCustomizationConfig.GetRandomHeadLut(dataId);
		return new ValueTuple<int, int, string, string>(randomBody, randomHead, text, (randomHeadLut != null) ? randomHeadLut.name : null);
	}

	// Token: 0x060029CA RID: 10698 RVA: 0x000C5284 File Offset: 0x000C3484
	public static void RollAndApplyZombieSkinToWgoData(WgoData wgoData, string dataId)
	{
		ValueTuple<int, int, string, string> valueTuple = ZombieSkinHelper.RollZombie(dataId);
		ZombieSkinHelper.ApplySkinToZombieWgoData(wgoData, valueTuple.Item1, valueTuple.Item2, valueTuple.Item3, valueTuple.Item4);
	}

	// Token: 0x060029CB RID: 10699 RVA: 0x000C52B6 File Offset: 0x000C34B6
	public static void ApplySkinToZombieWgoData(WgoData wgoData, int body, int head, string bodyLut, string headLut)
	{
		wgoData.SetGameRes("zombie_body_id", body);
		wgoData.SetGameRes("zombie_head_id", head);
		wgoData.GameResStr.Set("zombie_body_lut", bodyLut);
		wgoData.GameResStr.Set("zombie_head_lut", headLut);
	}

	// Token: 0x040022D4 RID: 8916
	public const string ZOMBIE_WORKER_DATA_ID = "zombie_worker";

	// Token: 0x040022D5 RID: 8917
	public const string ZOMBIE_ASSISTANT_DATA_ID = "zombie_assistant";
}
