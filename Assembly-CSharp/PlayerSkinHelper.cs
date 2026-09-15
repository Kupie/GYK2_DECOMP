using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000616 RID: 1558
public static class PlayerSkinHelper
{
	// Token: 0x170006AD RID: 1709
	// (get) Token: 0x060029A0 RID: 10656 RVA: 0x000C47CF File Offset: 0x000C29CF
	public static SkinPresetGK2 CurrentPreset
	{
		get
		{
			return PlayerSkinHelper.currentPreset;
		}
	}

	// Token: 0x170006AE RID: 1710
	// (get) Token: 0x060029A1 RID: 10657 RVA: 0x000C47D6 File Offset: 0x000C29D6
	private static SkinPresetGK2 DefaultPreset
	{
		get
		{
			if (PlayerSkinHelper.defaultPreset == null)
			{
				PlayerSkinHelper.defaultPreset = SkinPresetGK2.LoadAsset("9003_main_character");
			}
			return PlayerSkinHelper.defaultPreset;
		}
	}

	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x060029A2 RID: 10658 RVA: 0x000C47F9 File Offset: 0x000C29F9
	public static SkinPresetGK2 ArmorPreset
	{
		get
		{
			if (PlayerSkinHelper.armorPreset == null)
			{
				PlayerSkinHelper.armorPreset = SkinPresetGK2.LoadAsset("9019_main_character_armor");
			}
			return PlayerSkinHelper.armorPreset;
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x060029A3 RID: 10659 RVA: 0x000C481C File Offset: 0x000C2A1C
	public static PlayerColorCustomizationData CharacterCustomizationData
	{
		get
		{
			return MainGame.PlayerController.CharacterCustomizationData;
		}
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x000C4828 File Offset: 0x000C2A28
	public static void ApplySkin(PlayerCustomizationData customization, bool onlyForCustomizationCharacter)
	{
		PlayerSkinHelper.currentPreset = PlayerSkinHelper.GetPresetForCustomizationData(customization);
		MainGame.PlayerController.View.SetPlayerPreset(PlayerSkinHelper.currentPreset, onlyForCustomizationCharacter);
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x000C484A File Offset: 0x000C2A4A
	public static void ApplyPlayerColorsByData(PlayerCustomizationData customizationData, bool onlyForCustomizationCharacter)
	{
		PlayerSkinHelper.ApplyPlayerColors(PlayerSkinHelper.GetColorReplacementPalette(customizationData), PlayerSkinHelper.CharacterCustomizationData.affectedPartTypes, onlyForCustomizationCharacter);
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x000C4862 File Offset: 0x000C2A62
	public static void ApplyPlayerColors(ColorReplacePalette palette, List<CustomizablePartType> affectedPartTypes, bool onlyForCustomizationCharacter)
	{
		MainGame.PlayerController.View.ApplyPlayerColors(palette.palette, affectedPartTypes, onlyForCustomizationCharacter);
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000C487C File Offset: 0x000C2A7C
	public static ColorReplacePalette GetColorReplacementPalette(PlayerCustomizationData customization)
	{
		List<Texture2D> list = new List<Texture2D>();
		for (int i = 0; i < PlayerSkinHelper.CharacterCustomizationData.customizationElements.Count; i++)
		{
			PlayerColorCustomizationType playerColorCustomizationType = PlayerSkinHelper.CharacterCustomizationData.customizationElements[i].playerColorCustomizationType;
			int skinPresetPartId = PlayerSkinHelper.CurrentPreset.GetSkinPresetPartId(playerColorCustomizationType);
			list.Add(PlayerSkinHelper.CharacterCustomizationData.customizationElements[i].GetPaletteByIndex(PlayerSkinHelper.GetSavedColorPaletteIndex(playerColorCustomizationType, customization), skinPresetPartId));
		}
		return PaletteReplaceHelper.CombinePalettes(list, PlayerSkinHelper.CharacterCustomizationData.sourcePalette);
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x000C4900 File Offset: 0x000C2B00
	public static void ApplyArmorColorsByIndex(int index, bool includeHead = true)
	{
		ColorReplacePalette colorReplacePalette = PlayerSkinHelper.CharacterCustomizationData.armorPresetData.GetColorReplacePalette(index);
		List<CustomizablePartType> list = PlayerSkinHelper.CharacterCustomizationData.armorPresetData.affectedPartTypes;
		if (!includeHead)
		{
			list = list.FindAll((CustomizablePartType type) => type == CustomizablePartType.Body || type == CustomizablePartType.Arms);
		}
		MainGame.PlayerController.View.PlayerAnimation.ApplyPlayerColors(colorReplacePalette.palette, list, includeHead ? PlayerSkinHelper.ArmorPreset : PlayerSkinHelper.GetArmorNoHelmetPreset());
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x000C4984 File Offset: 0x000C2B84
	public static SkinPresetGK2 GetArmorNoHelmetPreset()
	{
		if (PlayerSkinHelper.armorNoHelmetPreset == null)
		{
			PlayerSkinHelper.armorNoHelmetPreset = ScriptableObject.CreateInstance<SkinPresetGK2>();
		}
		SkinPresetGK2 skinPresetGK = ((PlayerSkinHelper.currentPreset != null) ? PlayerSkinHelper.currentPreset : PlayerSkinHelper.DefaultPreset);
		PlayerSkinHelper.armorNoHelmetPreset.isPlayerPreset = true;
		PlayerSkinHelper.armorNoHelmetPreset.body = PlayerSkinHelper.CopyPart(PlayerSkinHelper.ArmorPreset.body);
		PlayerSkinHelper.armorNoHelmetPreset.arms = PlayerSkinHelper.CopyPart(PlayerSkinHelper.ArmorPreset.arms);
		PlayerSkinHelper.armorNoHelmetPreset.head = PlayerSkinHelper.CopyPart(skinPresetGK.head);
		PlayerSkinHelper.armorNoHelmetPreset.hairstyle = PlayerSkinHelper.CopyPart(skinPresetGK.hairstyle);
		PlayerSkinHelper.armorNoHelmetPreset.beard = PlayerSkinHelper.CopyPart(skinPresetGK.beard);
		return PlayerSkinHelper.armorNoHelmetPreset;
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x000C4A44 File Offset: 0x000C2C44
	private static SkinPresetPartGK2 CopyPart(SkinPresetPartGK2 source)
	{
		if (source == null)
		{
			return new SkinPresetPartGK2();
		}
		return new SkinPresetPartGK2
		{
			id = source.id,
			color = source.color,
			hue = source.hue,
			saturation = source.saturation,
			velocity = source.velocity,
			palette = source.palette,
			colorReplaceType = source.colorReplaceType
		};
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x000C4AB3 File Offset: 0x000C2CB3
	private static int GetSavedColorPaletteIndex(PlayerColorCustomizationType type, PlayerCustomizationData data)
	{
		return data.GetColorCustomizationIndexByType(type);
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x000C4ABC File Offset: 0x000C2CBC
	public static SkinPresetGK2 GetPresetForCustomizationData(PlayerCustomizationData customization)
	{
		SkinPresetGK2 skinPresetGK = ScriptableObject.CreateInstance<SkinPresetGK2>();
		SkinPresetPartGK2 skinPresetPartGK = PlayerSkinHelper.LoadSkinPresetPart(customization.GetCustomizationPartId(CustomizablePartType.Body));
		SkinPresetPartGK2 skinPresetPartGK2 = PlayerSkinHelper.LoadSkinPresetPart(customization.GetCustomizationPartId(CustomizablePartType.Arms));
		SkinPresetPartGK2 skinPresetPartGK3 = PlayerSkinHelper.LoadSkinPresetPart(customization.GetCustomizationPartId(CustomizablePartType.Hair));
		SkinPresetPartGK2 skinPresetPartGK4 = PlayerSkinHelper.LoadSkinPresetPart(customization.GetCustomizationPartId(CustomizablePartType.Beard));
		skinPresetGK.head = PlayerSkinHelper.DefaultPreset.head;
		skinPresetGK.isPlayerPreset = true;
		skinPresetGK.body = new SkinPresetPartGK2
		{
			id = skinPresetPartGK.id,
			colorReplaceType = ColorReplaceType.USE_PALETTE_COLOR_REPLACE
		};
		skinPresetGK.hairstyle = new SkinPresetPartGK2
		{
			id = skinPresetPartGK3.id,
			colorReplaceType = ColorReplaceType.USE_PALETTE_COLOR_REPLACE
		};
		skinPresetGK.arms = new SkinPresetPartGK2
		{
			id = skinPresetPartGK2.id,
			colorReplaceType = ColorReplaceType.USE_PALETTE_COLOR_REPLACE
		};
		skinPresetGK.beard = new SkinPresetPartGK2
		{
			id = skinPresetPartGK4.id,
			colorReplaceType = ColorReplaceType.USE_PALETTE_COLOR_REPLACE
		};
		return skinPresetGK;
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x000C4B94 File Offset: 0x000C2D94
	private static SkinPresetPartGK2 LoadSkinPresetPart(string id)
	{
		CustomizablePart customizablePart = PlayerSkinHelper.LoadCustomizablePart(id);
		if (customizablePart == null)
		{
			Debug.LogError("Couldn't load skin preset part: " + id);
			return null;
		}
		return customizablePart.skinPart;
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x000C4BCC File Offset: 0x000C2DCC
	private static CustomizablePart LoadCustomizablePart(string id)
	{
		return Addressables.LoadAssetAsync<CustomizablePart>("Assets/AddressableAssets/PlayerSkinParts/" + id + ".asset").WaitForCompletion();
	}

	// Token: 0x040022A8 RID: 8872
	private static SkinPresetGK2 currentPreset;

	// Token: 0x040022A9 RID: 8873
	private static SkinPresetGK2 armorPreset;

	// Token: 0x040022AA RID: 8874
	private static SkinPresetGK2 armorNoHelmetPreset;

	// Token: 0x040022AB RID: 8875
	private static SkinPresetGK2 defaultPreset;

	// Token: 0x040022AC RID: 8876
	public static readonly List<PlayerCustomizationPartData> DefaultCustomizationParts = new List<PlayerCustomizationPartData>
	{
		new PlayerCustomizationPartData("bdy_9003", CustomizablePartType.Body),
		new PlayerCustomizationPartData("arm_9003", CustomizablePartType.Arms),
		new PlayerCustomizationPartData("hrs_9002", CustomizablePartType.Hair),
		new PlayerCustomizationPartData("brd_9002", CustomizablePartType.Beard)
	};

	// Token: 0x040022AD RID: 8877
	public static readonly PlayerCustomizationData playerStandardCustomizationData = PlayerCustomizationData.CreateDefault();
}
