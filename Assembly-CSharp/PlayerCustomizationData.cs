using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000596 RID: 1430
[Serializable]
public class PlayerCustomizationData
{
	// Token: 0x060024C6 RID: 9414 RVA: 0x000AC9DC File Offset: 0x000AABDC
	public static PlayerCustomizationData CreateDefault()
	{
		PlayerCustomizationData playerCustomizationData = new PlayerCustomizationData();
		foreach (PlayerCustomizationPartData playerCustomizationPartData in PlayerSkinHelper.DefaultCustomizationParts)
		{
			playerCustomizationData.customizationPartsData.Add(new PlayerCustomizationPartData(playerCustomizationPartData.id, playerCustomizationPartData.type));
			playerCustomizationData.AddUnlockedPartId(playerCustomizationPartData.id, playerCustomizationPartData.type);
		}
		foreach (object obj in Enum.GetValues(typeof(PlayerColorCustomizationType)))
		{
			PlayerColorCustomizationType playerColorCustomizationType = (PlayerColorCustomizationType)obj;
			playerCustomizationData.SetColorCustomizationIndexForType(playerColorCustomizationType, 0);
			int partSkinIdForColorCustomizationType = playerCustomizationData.GetPartSkinIdForColorCustomizationType(playerColorCustomizationType);
			string colorPaletteName = PlayerCustomizationData.GetColorPaletteName(playerColorCustomizationType, partSkinIdForColorCustomizationType, 0, PlayerSkinHelper.CharacterCustomizationData);
			playerCustomizationData.AddUnlockedColorName(playerColorCustomizationType, partSkinIdForColorCustomizationType, colorPaletteName);
		}
		playerCustomizationData.UnlockHairAndBeardCustomization(PlayerSkinHelper.CharacterCustomizationData);
		return playerCustomizationData;
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x000ACAE8 File Offset: 0x000AACE8
	public static PlayerCustomizationData Copy(PlayerCustomizationData source)
	{
		PlayerCustomizationData playerCustomizationData = new PlayerCustomizationData();
		for (int i = 0; i < source.customizationPartsData.Count; i++)
		{
			playerCustomizationData.customizationPartsData.Add(new PlayerCustomizationPartData
			{
				id = source.customizationPartsData[i].id,
				type = source.customizationPartsData[i].type
			});
		}
		for (int j = 0; j < source.colorCustomizationPairData.Count; j++)
		{
			playerCustomizationData.colorCustomizationPairData.Add(new PlayerColorCustomizationPairData
			{
				index = source.colorCustomizationPairData[j].index,
				type = source.colorCustomizationPairData[j].type
			});
		}
		for (int k = 0; k < source.unlockedCustomizationPartsData.Count; k++)
		{
			playerCustomizationData.unlockedCustomizationPartsData.Add(new UnlockedCustomizationPartData(source.unlockedCustomizationPartsData[k].type, new List<string>(source.unlockedCustomizationPartsData[k].unlockedIds)));
		}
		for (int l = 0; l < source.unlockedColorCustomizationData.Count; l++)
		{
			UnlockedColorCustomizationData unlockedColorCustomizationData = new UnlockedColorCustomizationData(source.unlockedColorCustomizationData[l].type, source.unlockedColorCustomizationData[l].partSkinId, new List<string>(source.unlockedColorCustomizationData[l].unlockedNames));
			unlockedColorCustomizationData.unlockedIndices = new List<int>(source.unlockedColorCustomizationData[l].unlockedIndices);
			playerCustomizationData.unlockedColorCustomizationData.Add(unlockedColorCustomizationData);
		}
		return playerCustomizationData;
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x000ACC7C File Offset: 0x000AAE7C
	public bool UnlockCustomizationPart(string id, CustomizablePartType type)
	{
		if (string.IsNullOrEmpty(id))
		{
			return false;
		}
		if (!this.AddUnlockedPartId(id, type))
		{
			return false;
		}
		if (type == CustomizablePartType.Body)
		{
			string text = id.Replace("bdy_", "arm_");
			this.AddUnlockedPartId(text, CustomizablePartType.Arms);
		}
		return true;
	}

	// Token: 0x060024C9 RID: 9417 RVA: 0x000ACCBE File Offset: 0x000AAEBE
	public bool UnlockCustomizationColor(PlayerColorCustomizationType type, string paletteName, int partSkinId)
	{
		return this.UnlockCustomizationColor(type, paletteName, partSkinId, PlayerSkinHelper.CharacterCustomizationData);
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x000ACCCE File Offset: 0x000AAECE
	public bool UnlockCustomizationColor(PlayerColorCustomizationType type, string paletteName, int partSkinId, PlayerColorCustomizationData colorCustomizationData)
	{
		return PlayerCustomizationData.IsValidColorPaletteName(type, partSkinId, paletteName, colorCustomizationData) && this.AddUnlockedColorName(type, partSkinId, paletteName);
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x000ACCE8 File Offset: 0x000AAEE8
	public void UnlockAllCustomization(PlayerColorCustomizationData colorCustomizationData)
	{
		IList<CustomizablePart> list = Addressables.LoadAssetsAsync<CustomizablePart>("player_skins", null).WaitForCompletion();
		for (int i = 0; i < list.Count; i++)
		{
			this.UnlockCustomizationPart(list[i].name, list[i].type);
		}
		for (int j = 0; j < colorCustomizationData.customizationElements.Count; j++)
		{
			PlayerColorCustomizationElementData playerColorCustomizationElementData = colorCustomizationData.customizationElements[j];
			for (int k = 0; k < playerColorCustomizationElementData.SkinElements.Count; k++)
			{
				PlayerColorCustomizationSkinElementData playerColorCustomizationSkinElementData = playerColorCustomizationElementData.SkinElements[k];
				for (int l = 0; l < playerColorCustomizationSkinElementData.palettes.Count; l++)
				{
					Texture2D texture2D = playerColorCustomizationSkinElementData.palettes[l];
					if (texture2D != null)
					{
						this.UnlockCustomizationColor(playerColorCustomizationElementData.playerColorCustomizationType, texture2D.name, playerColorCustomizationSkinElementData.skinId, colorCustomizationData);
					}
				}
			}
		}
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x000ACDE0 File Offset: 0x000AAFE0
	public void UnlockHairAndBeardCustomization(PlayerColorCustomizationData colorCustomizationData)
	{
		IList<CustomizablePart> list = Addressables.LoadAssetsAsync<CustomizablePart>("player_skins", null).WaitForCompletion();
		List<int> list2 = new List<int>();
		for (int i = 0; i < list.Count; i++)
		{
			CustomizablePart customizablePart = list[i];
			if (!(customizablePart == null))
			{
				if (customizablePart.type == CustomizablePartType.Hair || customizablePart.type == CustomizablePartType.Beard)
				{
					this.UnlockCustomizationPart(customizablePart.name, customizablePart.type);
				}
				int num;
				if (PlayerCustomizationData.TryGetHeadColorSkinId(customizablePart.name, customizablePart.type, out num) && !list2.Contains(num))
				{
					list2.Add(num);
				}
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			this.UnlockAllColorPalettes(PlayerColorCustomizationType.Hed, list2[j], colorCustomizationData);
		}
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000ACEA4 File Offset: 0x000AB0A4
	public bool SuperUnlockBodyCustomization(string id, PlayerColorCustomizationData colorCustomizationData, Item sourceItem = null)
	{
		int num;
		if (!PlayerCustomizationData.TryParsePartSkinId(id, "bdy_", out num))
		{
			return false;
		}
		bool flag = this.UnlockCustomizationPart(id, CustomizablePartType.Body);
		for (int i = 0; i < PlayerCustomizationData.BodyColorCustomizationTypes.Length; i++)
		{
			flag |= this.UnlockAllColorPalettes(PlayerCustomizationData.BodyColorCustomizationTypes[i], num, colorCustomizationData);
		}
		if (sourceItem != null && flag)
		{
			Action<Item> onCustomizationUnlocked = PlayerCustomizationData.OnCustomizationUnlocked;
			if (onCustomizationUnlocked != null)
			{
				onCustomizationUnlocked(sourceItem);
			}
		}
		return flag;
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x000ACF0C File Offset: 0x000AB10C
	public List<string> GetUnlockedPartIds(CustomizablePartType type)
	{
		UnlockedCustomizationPartData unlockedCustomizationPartData = this.unlockedCustomizationPartsData.Find((UnlockedCustomizationPartData x) => x.type == type);
		if (unlockedCustomizationPartData == null)
		{
			return new List<string>();
		}
		return unlockedCustomizationPartData.unlockedIds;
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x000ACF50 File Offset: 0x000AB150
	public List<string> GetUnlockedColorNames(PlayerColorCustomizationType type, int partSkinId)
	{
		UnlockedColorCustomizationData unlockedColorCustomizationData = this.unlockedColorCustomizationData.Find((UnlockedColorCustomizationData x) => x.type == type && x.partSkinId == partSkinId);
		if (unlockedColorCustomizationData == null)
		{
			return new List<string>();
		}
		return unlockedColorCustomizationData.unlockedNames;
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x000ACF98 File Offset: 0x000AB198
	public List<int> GetUnlockedColorIndices(PlayerColorCustomizationType type, int partSkinId, PlayerColorCustomizationData colorCustomizationData)
	{
		List<int> list = new List<int>();
		if (colorCustomizationData == null)
		{
			return list;
		}
		PlayerColorCustomizationElementData playerColorCustomizationElementData = colorCustomizationData.customizationElements.Find((PlayerColorCustomizationElementData e) => e.playerColorCustomizationType == type);
		PlayerColorCustomizationSkinElementData playerColorCustomizationSkinElementData = ((playerColorCustomizationElementData != null) ? playerColorCustomizationElementData.GetSkinElement(partSkinId) : null);
		if (playerColorCustomizationSkinElementData == null)
		{
			return list;
		}
		UnlockedColorCustomizationData unlockedColorCustomizationData = this.unlockedColorCustomizationData.Find((UnlockedColorCustomizationData x) => x.type == type && x.partSkinId == partSkinId);
		List<string> list2 = ((unlockedColorCustomizationData != null) ? unlockedColorCustomizationData.unlockedNames : new List<string>());
		for (int i = 0; i < playerColorCustomizationSkinElementData.palettes.Count; i++)
		{
			Texture2D texture2D = playerColorCustomizationSkinElementData.palettes[i];
			if (texture2D != null && list2.Contains(texture2D.name))
			{
				list.Add(i);
			}
		}
		if (unlockedColorCustomizationData != null)
		{
			for (int j = 0; j < unlockedColorCustomizationData.unlockedIndices.Count; j++)
			{
				int num = unlockedColorCustomizationData.unlockedIndices[j];
				if (num >= 0 && num < playerColorCustomizationSkinElementData.palettes.Count && !list.Contains(num))
				{
					list.Add(num);
				}
			}
			list.Sort();
		}
		return list;
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000AD0C8 File Offset: 0x000AB2C8
	public string GetCustomizationPartId(CustomizablePartType type)
	{
		PlayerCustomizationPartData playerCustomizationPartData = this.customizationPartsData.Find((PlayerCustomizationPartData x) => x.type == type);
		if (playerCustomizationPartData != null)
		{
			return playerCustomizationPartData.id;
		}
		Debug.LogError(string.Format("Cannot find skin part in player data with type[{0}]", type));
		return string.Empty;
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x000AD124 File Offset: 0x000AB324
	public int GetColorCustomizationIndexByType(PlayerColorCustomizationType type)
	{
		PlayerColorCustomizationPairData playerColorCustomizationPairData = this.colorCustomizationPairData.Find((PlayerColorCustomizationPairData d) => d.type == type);
		if (playerColorCustomizationPairData != null)
		{
			return playerColorCustomizationPairData.index;
		}
		return 0;
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x000AD164 File Offset: 0x000AB364
	public void SetColorCustomizationIndexForType(PlayerColorCustomizationType type, int index)
	{
		PlayerColorCustomizationPairData playerColorCustomizationPairData = this.colorCustomizationPairData.Find((PlayerColorCustomizationPairData d) => d.type == type);
		if (playerColorCustomizationPairData != null)
		{
			playerColorCustomizationPairData.index = index;
			return;
		}
		playerColorCustomizationPairData = new PlayerColorCustomizationPairData(index, type);
		this.colorCustomizationPairData.Add(playerColorCustomizationPairData);
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x000AD1BC File Offset: 0x000AB3BC
	public int GetPartSkinIdForColorCustomizationType(PlayerColorCustomizationType type)
	{
		CustomizablePartType customizationPartTypeForColorCustomizationType = PlayerCustomizationData.GetCustomizationPartTypeForColorCustomizationType(type);
		string customizationPartId = this.GetCustomizationPartId(customizationPartTypeForColorCustomizationType);
		int num = customizationPartId.LastIndexOf('_');
		int num2;
		if (num >= 0 && int.TryParse(customizationPartId.Substring(num + 1), out num2))
		{
			return num2;
		}
		Debug.LogError(string.Format("Cannot parse skin id from customization part id:[{0}] for color type:[{1}]", customizationPartId, type));
		return -1;
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x000AD210 File Offset: 0x000AB410
	private bool AddUnlockedPartId(string id, CustomizablePartType type)
	{
		UnlockedCustomizationPartData unlockedCustomizationPartData = this.unlockedCustomizationPartsData.Find((UnlockedCustomizationPartData x) => x.type == type);
		if (unlockedCustomizationPartData == null)
		{
			unlockedCustomizationPartData = new UnlockedCustomizationPartData(type, new List<string>());
			this.unlockedCustomizationPartsData.Add(unlockedCustomizationPartData);
		}
		if (unlockedCustomizationPartData.unlockedIds.Contains(id))
		{
			return false;
		}
		unlockedCustomizationPartData.unlockedIds.Add(id);
		return true;
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x000AD280 File Offset: 0x000AB480
	private bool AddUnlockedColorName(PlayerColorCustomizationType type, int partSkinId, string paletteName)
	{
		if (string.IsNullOrEmpty(paletteName))
		{
			return false;
		}
		UnlockedColorCustomizationData unlockedColorCustomizationData = this.unlockedColorCustomizationData.Find((UnlockedColorCustomizationData x) => x.type == type && x.partSkinId == partSkinId);
		if (unlockedColorCustomizationData == null)
		{
			unlockedColorCustomizationData = new UnlockedColorCustomizationData(type, partSkinId, new List<string>());
			this.unlockedColorCustomizationData.Add(unlockedColorCustomizationData);
		}
		if (unlockedColorCustomizationData.unlockedNames.Contains(paletteName))
		{
			return false;
		}
		unlockedColorCustomizationData.unlockedNames.Add(paletteName);
		unlockedColorCustomizationData.unlockedNames.Sort();
		return true;
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x000AD314 File Offset: 0x000AB514
	private bool UnlockAllColorPalettes(PlayerColorCustomizationType type, int partSkinId, PlayerColorCustomizationData colorCustomizationData)
	{
		if (partSkinId < 0 || colorCustomizationData == null)
		{
			return false;
		}
		PlayerColorCustomizationElementData playerColorCustomizationElementData = colorCustomizationData.customizationElements.Find((PlayerColorCustomizationElementData e) => e.playerColorCustomizationType == type);
		PlayerColorCustomizationSkinElementData playerColorCustomizationSkinElementData = ((playerColorCustomizationElementData != null) ? playerColorCustomizationElementData.GetSkinElement(partSkinId) : null);
		if (playerColorCustomizationSkinElementData == null)
		{
			return false;
		}
		bool flag = false;
		for (int i = 0; i < playerColorCustomizationSkinElementData.palettes.Count; i++)
		{
			Texture2D texture2D = playerColorCustomizationSkinElementData.palettes[i];
			if (texture2D != null)
			{
				flag |= this.AddUnlockedColorName(type, partSkinId, texture2D.name);
			}
		}
		return flag;
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x000AD3B0 File Offset: 0x000AB5B0
	private static bool IsValidColorPaletteName(PlayerColorCustomizationType type, int partSkinId, string paletteName, PlayerColorCustomizationData colorCustomizationData)
	{
		if (partSkinId < 0 || string.IsNullOrEmpty(paletteName) || colorCustomizationData == null)
		{
			return false;
		}
		PlayerColorCustomizationElementData playerColorCustomizationElementData = colorCustomizationData.customizationElements.Find((PlayerColorCustomizationElementData e) => e.playerColorCustomizationType == type);
		if (playerColorCustomizationElementData == null)
		{
			Debug.LogError(string.Format("Cannot find color customization element for type[{0}]", type));
			return false;
		}
		PlayerColorCustomizationSkinElementData skinElement = playerColorCustomizationElementData.GetSkinElement(partSkinId);
		return skinElement != null && skinElement.palettes.Exists((Texture2D palette) => palette != null && palette.name == paletteName);
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000AD450 File Offset: 0x000AB650
	private static string GetColorPaletteName(PlayerColorCustomizationType type, int partSkinId, int index, PlayerColorCustomizationData colorCustomizationData)
	{
		if (partSkinId < 0 || index < 0 || colorCustomizationData == null)
		{
			return string.Empty;
		}
		PlayerColorCustomizationElementData playerColorCustomizationElementData = colorCustomizationData.customizationElements.Find((PlayerColorCustomizationElementData e) => e.playerColorCustomizationType == type);
		PlayerColorCustomizationSkinElementData playerColorCustomizationSkinElementData = ((playerColorCustomizationElementData != null) ? playerColorCustomizationElementData.GetSkinElement(partSkinId) : null);
		if (playerColorCustomizationSkinElementData == null || index >= playerColorCustomizationSkinElementData.palettes.Count || playerColorCustomizationSkinElementData.palettes[index] == null)
		{
			return string.Empty;
		}
		return playerColorCustomizationSkinElementData.palettes[index].name;
	}

	// Token: 0x060024DA RID: 9434 RVA: 0x000AD4E2 File Offset: 0x000AB6E2
	private static bool TryParsePartSkinId(string id, string prefix, out int partSkinId)
	{
		partSkinId = -1;
		return !string.IsNullOrEmpty(id) && id.StartsWith(prefix, StringComparison.Ordinal) && int.TryParse(id.Substring(prefix.Length), out partSkinId);
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000AD50D File Offset: 0x000AB70D
	private static bool TryGetHeadColorSkinId(string id, CustomizablePartType type, out int partSkinId)
	{
		if (type == CustomizablePartType.Hair)
		{
			return PlayerCustomizationData.TryParsePartSkinId(id, "hrs_", out partSkinId);
		}
		if (type != CustomizablePartType.Beard)
		{
			partSkinId = -1;
			return false;
		}
		return PlayerCustomizationData.TryParsePartSkinId(id, "brd_", out partSkinId);
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x000AD536 File Offset: 0x000AB736
	private static CustomizablePartType GetCustomizationPartTypeForColorCustomizationType(PlayerColorCustomizationType type)
	{
		if (type == PlayerColorCustomizationType.Hed)
		{
			return CustomizablePartType.Hair;
		}
		if (type - PlayerColorCustomizationType.Bdy1 > 2)
		{
			throw new ArgumentOutOfRangeException("type", type, null);
		}
		return CustomizablePartType.Body;
	}

	// Token: 0x04002079 RID: 8313
	public static Action<Item> OnCustomizationUnlocked;

	// Token: 0x0400207A RID: 8314
	private static readonly PlayerColorCustomizationType[] BodyColorCustomizationTypes = new PlayerColorCustomizationType[]
	{
		PlayerColorCustomizationType.Bdy1,
		PlayerColorCustomizationType.Bdy2,
		PlayerColorCustomizationType.Bdy3
	};

	// Token: 0x0400207B RID: 8315
	public List<PlayerCustomizationPartData> customizationPartsData = new List<PlayerCustomizationPartData>();

	// Token: 0x0400207C RID: 8316
	public List<PlayerColorCustomizationPairData> colorCustomizationPairData = new List<PlayerColorCustomizationPairData>();

	// Token: 0x0400207D RID: 8317
	public List<UnlockedCustomizationPartData> unlockedCustomizationPartsData = new List<UnlockedCustomizationPartData>();

	// Token: 0x0400207E RID: 8318
	public List<UnlockedColorCustomizationData> unlockedColorCustomizationData = new List<UnlockedColorCustomizationData>();
}
