using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000618 RID: 1560
public class ZombieCustomizationConfig : LazySingletonSO<ZombieCustomizationConfig>
{
	// Token: 0x060029B3 RID: 10675 RVA: 0x000C4C78 File Offset: 0x000C2E78
	private void TryCreateCache()
	{
		if (!this.isCacheCreated)
		{
			this.isCacheCreated = true;
			this.zombieRolledDatasDict = new Dictionary<string, ZombieCustomizationConfig.ZombieRolledData>();
			foreach (ZombieCustomizationConfig.ZombieRolledData zombieRolledData in this.zombieRolledDatas)
			{
				this.zombieRolledDatasDict.Add(zombieRolledData.id, zombieRolledData);
				zombieRolledData.CreateCache();
			}
		}
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x000C4CF8 File Offset: 0x000C2EF8
	public static Texture2D GetBodyTextureByName(string dataId, string textureName)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		return LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].bodyTexturesDict[textureName];
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x000C4D1F File Offset: 0x000C2F1F
	public static Texture2D GetHeadTextureByName(string dataId, string textureName)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		return LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].headTexturesDict[textureName];
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x000C4D46 File Offset: 0x000C2F46
	public static int GetRandomBody(string dataId)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		return LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].bodyIds.GetRandom<int>();
	}

	// Token: 0x060029B7 RID: 10679 RVA: 0x000C4D6C File Offset: 0x000C2F6C
	public static int GetRandomHead(string dataId)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		return LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].headIds.GetRandom<int>();
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x000C4D94 File Offset: 0x000C2F94
	public static Texture2D GetRandomBodyLut(string dataId)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		if (LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].bodyTextures.Count <= 0)
		{
			return null;
		}
		return LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].bodyTextures.GetRandom<Texture2D>();
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x000C4DE4 File Offset: 0x000C2FE4
	public static Texture2D GetRandomHeadLut(string dataId)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		if (LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].headTextures.Count <= 0)
		{
			return null;
		}
		return LazySingletonSO<ZombieCustomizationConfig>.Instance.zombieRolledDatasDict[dataId].headTextures.GetRandom<Texture2D>();
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x000C4E34 File Offset: 0x000C3034
	public static ColorReplacePalette GetBodyReplacePalette(string flagVariationId)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType colorType = ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType.Blue;
		if (!(flagVariationId == "blue"))
		{
			if (!(flagVariationId == "red"))
			{
				if (!(flagVariationId == "green"))
				{
					if (!(flagVariationId == "violet"))
					{
						if (flagVariationId == "turquoise")
						{
							colorType = ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType.Turquoise;
						}
					}
					else
					{
						colorType = ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType.Violet;
					}
				}
				else
				{
					colorType = ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType.Green;
				}
			}
			else
			{
				colorType = ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType.Red;
			}
		}
		else
		{
			colorType = ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType.Blue;
		}
		ZombieCustomizationConfig.ZombieFighterBodyPallete zombieFighterBodyPallete = LazySingletonSO<ZombieCustomizationConfig>.Instance.fightersBodyPalettes.Find((ZombieCustomizationConfig.ZombieFighterBodyPallete x) => x.colorType == colorType);
		if (zombieFighterBodyPallete == null)
		{
			Debug.LogError(string.Format("ZombieFighterColorPallete for color [{0}] doesn't exist", colorType));
			return null;
		}
		return PaletteReplaceHelper.CombinePalettes(zombieFighterBodyPallete.palette.GetPalettes(), LazySingletonSO<ZombieCustomizationConfig>.Instance.sourceColorPalette);
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x000C4F1C File Offset: 0x000C311C
	public static ColorReplacePalette GetArmsArmorReplacePalette(string armorItemId)
	{
		LazySingletonSO<ZombieCustomizationConfig>.Instance.TryCreateCache();
		ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier tier = ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier.Leather;
		if (!(armorItemId == "armor_0"))
		{
			if (!(armorItemId == "armor_1"))
			{
				if (!(armorItemId == "armor_2"))
				{
					if (!(armorItemId == "armor_3"))
					{
						if (armorItemId == "armor_4")
						{
							tier = ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier.Elite;
						}
					}
					else
					{
						tier = ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier.Iron;
					}
				}
				else
				{
					tier = ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier.Metall;
				}
			}
			else
			{
				tier = ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier.Rusty;
			}
		}
		else
		{
			tier = ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier.Leather;
		}
		ZombieCustomizationConfig.ZombieFighterArmsArmorPallete zombieFighterArmsArmorPallete = LazySingletonSO<ZombieCustomizationConfig>.Instance.fightersArmsArmorPalettes.Find((ZombieCustomizationConfig.ZombieFighterArmsArmorPallete x) => x.tier == tier);
		if (zombieFighterArmsArmorPallete == null)
		{
			Debug.LogError(string.Format("ZombieFighterArmorPallete for tier [{0}] doesn't exist", tier));
			return null;
		}
		return PaletteReplaceHelper.CombinePalettes(zombieFighterArmsArmorPallete.palette.GetPalettes(), LazySingletonSO<ZombieCustomizationConfig>.Instance.sourceArmorPalette);
	}

	// Token: 0x040022B0 RID: 8880
	[SerializeField]
	private List<ZombieCustomizationConfig.ZombieRolledData> zombieRolledDatas;

	// Token: 0x040022B1 RID: 8881
	private Dictionary<string, ZombieCustomizationConfig.ZombieRolledData> zombieRolledDatasDict;

	// Token: 0x040022B2 RID: 8882
	[Space]
	[SerializeField]
	private List<ZombieCustomizationConfig.ZombieFighterBodyPallete> fightersBodyPalettes;

	// Token: 0x040022B3 RID: 8883
	[SerializeField]
	private Texture2D sourceColorPalette;

	// Token: 0x040022B4 RID: 8884
	[Space]
	[SerializeField]
	private List<ZombieCustomizationConfig.ZombieFighterArmsArmorPallete> fightersArmsArmorPalettes;

	// Token: 0x040022B5 RID: 8885
	[SerializeField]
	private Texture2D sourceArmorPalette;

	// Token: 0x040022B6 RID: 8886
	private bool isCacheCreated;

	// Token: 0x02000619 RID: 1561
	[Serializable]
	private class ZombieRolledData
	{
		// Token: 0x060029BD RID: 10685 RVA: 0x000C500C File Offset: 0x000C320C
		public void CreateCache()
		{
			this.bodyTexturesDict = new Dictionary<string, Texture2D>();
			for (int i = 0; i < this.bodyTextures.Count; i++)
			{
				this.bodyTexturesDict.Add(this.bodyTextures[i].name, this.bodyTextures[i]);
			}
			this.headTexturesDict = new Dictionary<string, Texture2D>();
			for (int j = 0; j < this.headTextures.Count; j++)
			{
				this.headTexturesDict.Add(this.headTextures[j].name, this.headTextures[j]);
			}
		}

		// Token: 0x040022B7 RID: 8887
		public string id;

		// Token: 0x040022B8 RID: 8888
		public List<Texture2D> bodyTextures;

		// Token: 0x040022B9 RID: 8889
		public List<Texture2D> headTextures;

		// Token: 0x040022BA RID: 8890
		public List<int> bodyIds;

		// Token: 0x040022BB RID: 8891
		public List<int> headIds;

		// Token: 0x040022BC RID: 8892
		public Dictionary<string, Texture2D> bodyTexturesDict;

		// Token: 0x040022BD RID: 8893
		public Dictionary<string, Texture2D> headTexturesDict;
	}

	// Token: 0x0200061A RID: 1562
	[Serializable]
	public class ZombieFighterBodyPallete
	{
		// Token: 0x040022BE RID: 8894
		public ZombieCustomizationConfig.ZombieFighterBodyPallete.ColorType colorType;

		// Token: 0x040022BF RID: 8895
		public ArmorColorPalette palette;

		// Token: 0x0200061B RID: 1563
		public enum ColorType
		{
			// Token: 0x040022C1 RID: 8897
			Blue,
			// Token: 0x040022C2 RID: 8898
			Red = 2,
			// Token: 0x040022C3 RID: 8899
			Green,
			// Token: 0x040022C4 RID: 8900
			Violet,
			// Token: 0x040022C5 RID: 8901
			Turquoise
		}
	}

	// Token: 0x0200061C RID: 1564
	[Serializable]
	public class ZombieFighterArmsArmorPallete
	{
		// Token: 0x040022C6 RID: 8902
		public ZombieCustomizationConfig.ZombieFighterArmsArmorPallete.Tier tier;

		// Token: 0x040022C7 RID: 8903
		public ArmorColorPalette palette;

		// Token: 0x0200061D RID: 1565
		public enum Tier
		{
			// Token: 0x040022C9 RID: 8905
			Leather,
			// Token: 0x040022CA RID: 8906
			Rusty = 2,
			// Token: 0x040022CB RID: 8907
			Metall,
			// Token: 0x040022CC RID: 8908
			Iron,
			// Token: 0x040022CD RID: 8909
			Elite
		}
	}
}
