using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000450 RID: 1104
[Serializable]
public class TownSystem
{
	// Token: 0x14000047 RID: 71
	// (add) Token: 0x06001CDD RID: 7389 RVA: 0x0008701C File Offset: 0x0008521C
	// (remove) Token: 0x06001CDE RID: 7390 RVA: 0x00087050 File Offset: 0x00085250
	public static event Action OnClearTownPalettes;

	// Token: 0x14000048 RID: 72
	// (add) Token: 0x06001CDF RID: 7391 RVA: 0x00087084 File Offset: 0x00085284
	// (remove) Token: 0x06001CE0 RID: 7392 RVA: 0x000870B8 File Offset: 0x000852B8
	public static event Action OnQualityChanged;

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000870EB File Offset: 0x000852EB
	// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x000870F4 File Offset: 0x000852F4
	public int Quality
	{
		get
		{
			return this.quality;
		}
		set
		{
			bool flag = this.quality != value;
			this.quality = value;
			if (flag)
			{
				Action onQualityChanged = TownSystem.OnQualityChanged;
				if (onQualityChanged != null)
				{
					onQualityChanged();
				}
			}
			if (GUIElements.Instance != null)
			{
				GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
			}
		}
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x00002318 File Offset: 0x00000518
	public void UpdateSystemAtTheEndOfDay(int day)
	{
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x00087148 File Offset: 0x00085348
	public void ResetVendors()
	{
		for (int i = 0; i < MainGame.Instance.GameSave.vendorSystem.vendors.Count; i++)
		{
			Vendor vendor = MainGame.Instance.GameSave.vendorSystem.vendors[i];
			if (vendor.Definition.townVendor)
			{
				vendor.UsedHappinessThisWeek = 0f;
				vendor.SoldItemsWithHappinessThisWeek.Clear();
			}
		}
		Debug.Log("#economy# ResetVendors");
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x000871C4 File Offset: 0x000853C4
	public void ClearTownPalettes()
	{
		TownSystem.moneyForPalettes = 0;
		MainGame.Instance.GameSave.vendorSystem.TryResolveOrders();
		Action onClearTownPalettes = TownSystem.OnClearTownPalettes;
		if (onClearTownPalettes != null)
		{
			onClearTownPalettes();
		}
		if (TownSystem.moneyForPalettes > 0)
		{
			MainGame.Instance.GameSave.worldData.GetWgoData("town_chalk_board").StorePaletteTradingResult(TownSystem.moneyForPalettes);
		}
		Debug.Log(string.Format("#economy# ClearTownPalettes moneyForPalettes:[{0}]", TownSystem.moneyForPalettes));
	}

	// Token: 0x06001CE6 RID: 7398 RVA: 0x00087240 File Offset: 0x00085440
	public void StartTownBuildingCraftOnWgoFromScript(TownBuildingDef def, WgoData wgoData)
	{
		if (def == null)
		{
			Debug.LogError("Trying StartTownBuildingCraftOnWgoFromScript with null def!!!");
			return;
		}
		if (wgoData == null)
		{
			Debug.LogError("Trying StartTownBuildingCraftOnWgoFromScript with null wgoData!!!");
			return;
		}
		Debug.Log(string.Concat(new string[] { "StartTownBuildingCraftOnWgoFromScript def:[", def.id, "] wgoData:[", wgoData.id, "]" }));
		string text = "town_building_craft:" + def.id;
		CraftElementBase craftElementBase = new CraftElement(text, 1, new CraftParamsData(text, wgoData, CraftParamsData.CraftParamsType.Common, -1));
		if (wgoData.CraftComponent.TryStartCraft(craftElementBase))
		{
			wgoData.CraftComponent.LastStartedCraftWithRequirements = craftElementBase;
		}
	}

	// Token: 0x06001CE7 RID: 7399 RVA: 0x000872E0 File Offset: 0x000854E0
	public void CreateTownBuildingOnWgo(WgoData wgoData)
	{
		TownBuildingWgoComponent townBuildingWgoComponent = wgoData.TownBuildingWgoComponent;
		townBuildingWgoComponent.TownBuildingId = wgoData.CraftComponent.LastStartedCraftWithRequirements.CraftId.Replace("town_building_craft:", "");
		TownBuildingDef data = GameBalance.Me.GetData<TownBuildingDef>(townBuildingWgoComponent.TownBuildingId);
		int tierIndex = townBuildingWgoComponent.TierIndex;
		string customTag = wgoData.CustomTag;
		string text = ((wgoData.Definition.interactionType == WGODef.InteractionType.TownBuildingPlace) ? "t_b_signboard_" : "t_b_character_");
		if (townBuildingWgoComponent.SceneConfiguration.tierDataList.Count > 0)
		{
			TownBuildingTierSceneConfiguration townBuildingTierSceneConfiguration = null;
			if (tierIndex > 0)
			{
				townBuildingTierSceneConfiguration = townBuildingWgoComponent.SceneConfiguration.tierDataList[tierIndex - 1];
			}
			TownBuildingTierSceneConfiguration townBuildingTierSceneConfiguration2 = townBuildingWgoComponent.SceneConfiguration.tierDataList[tierIndex];
			if (!string.IsNullOrEmpty(townBuildingTierSceneConfiguration2.yard.wgoId))
			{
				WgoData wgoData2 = new WgoData(townBuildingTierSceneConfiguration2.yard.wgoId, townBuildingTierSceneConfiguration2.yard.position, wgoData.WorldId);
				wgoData2.Scale = ((townBuildingTierSceneConfiguration2.yard.scale == Vector3.zero) ? Vector3.one : townBuildingTierSceneConfiguration2.yard.scale);
				wgoData2.CustomTag = customTag.Replace(text, "t_b_yard_");
				wgoData2.MainWgoPartData.variationId = data.variationId;
				wgoData2.MainWgoPartData.rotationIndex = -1;
				MainGame.WorldData.AddWgoData(wgoData2);
				townBuildingTierSceneConfiguration2.yard.createdWgoUniqueId = wgoData2.UniqueId;
			}
			if (!string.IsNullOrEmpty(townBuildingTierSceneConfiguration2.decor1.wgoId))
			{
				WgoData wgoData3 = new WgoData(townBuildingTierSceneConfiguration2.decor1.wgoId, townBuildingTierSceneConfiguration2.decor1.position, wgoData.WorldId);
				wgoData3.CustomTag = customTag.Replace(text, "t_b_decor_1_");
				wgoData3.Scale = ((townBuildingTierSceneConfiguration2.decor1.scale == Vector3.zero) ? Vector3.one : townBuildingTierSceneConfiguration2.decor1.scale);
				wgoData3.MainWgoPartData.variationId = data.variationId;
				wgoData3.MainWgoPartData.rotationIndex = -1;
				MainGame.WorldData.AddWgoData(wgoData3);
				townBuildingTierSceneConfiguration2.decor1.createdWgoUniqueId = wgoData3.UniqueId;
			}
			if (!string.IsNullOrEmpty(townBuildingTierSceneConfiguration2.decor2.wgoId))
			{
				WgoData wgoData4 = new WgoData(townBuildingTierSceneConfiguration2.decor2.wgoId, townBuildingTierSceneConfiguration2.decor2.position, wgoData.WorldId);
				wgoData4.Scale = ((townBuildingTierSceneConfiguration2.decor2.scale == Vector3.zero) ? Vector3.one : townBuildingTierSceneConfiguration2.decor2.scale);
				wgoData4.CustomTag = customTag.Replace(text, "t_b_decor_2_");
				wgoData4.MainWgoPartData.variationId = data.variationId;
				wgoData4.MainWgoPartData.rotationIndex = -1;
				MainGame.WorldData.AddWgoData(wgoData4);
				townBuildingTierSceneConfiguration2.decor2.createdWgoUniqueId = wgoData4.UniqueId;
			}
			if (!string.IsNullOrEmpty(townBuildingTierSceneConfiguration2.decor3.wgoId))
			{
				WgoData wgoData5 = new WgoData(townBuildingTierSceneConfiguration2.decor3.wgoId, townBuildingTierSceneConfiguration2.decor3.position, wgoData.WorldId);
				wgoData5.CustomTag = customTag.Replace(text, "t_b_decor_3_");
				wgoData5.Scale = ((townBuildingTierSceneConfiguration2.decor3.scale == Vector3.zero) ? Vector3.one : townBuildingTierSceneConfiguration2.decor3.scale);
				wgoData5.MainWgoPartData.variationId = data.variationId;
				wgoData5.MainWgoPartData.rotationIndex = -1;
				MainGame.WorldData.AddWgoData(wgoData5);
				townBuildingTierSceneConfiguration2.decor3.createdWgoUniqueId = wgoData5.UniqueId;
			}
			if (!string.IsNullOrEmpty(townBuildingTierSceneConfiguration2.sign.wgoId))
			{
				WgoData wgoData6 = new WgoData(townBuildingTierSceneConfiguration2.sign.wgoId, townBuildingTierSceneConfiguration2.sign.position, wgoData.WorldId);
				wgoData6.Scale = ((townBuildingTierSceneConfiguration2.sign.scale == Vector3.zero) ? Vector3.one : townBuildingTierSceneConfiguration2.sign.scale);
				wgoData6.CustomTag = customTag.Replace(text, "t_b_sign_");
				wgoData6.MainWgoPartData.variationId = data.variationId;
				wgoData6.MainWgoPartData.rotationIndex = -1;
				MainGame.WorldData.AddWgoData(wgoData6);
				townBuildingTierSceneConfiguration2.sign.createdWgoUniqueId = wgoData6.UniqueId;
			}
			WgoData wgoData7 = new WgoData(townBuildingTierSceneConfiguration2.tent.wgoId, townBuildingTierSceneConfiguration2.tent.position, wgoData.WorldId);
			wgoData7.Scale = ((townBuildingTierSceneConfiguration2.tent.scale == Vector3.zero) ? Vector3.one : townBuildingTierSceneConfiguration2.tent.scale);
			wgoData7.CustomTag = customTag.Replace(text, "t_b_tent_");
			wgoData7.TownBuildingWgoComponent = wgoData.TownBuildingWgoComponent;
			TownBuildingWgoComponent townBuildingWgoComponent2 = wgoData7.TownBuildingWgoComponent;
			int num = townBuildingWgoComponent2.TierIndex;
			townBuildingWgoComponent2.TierIndex = num + 1;
			wgoData7.MainWgoPartData.variationId = data.variationId;
			wgoData7.MainWgoPartData.rotationIndex = -1;
			MainGame.WorldData.AddWgoData(wgoData7);
			townBuildingTierSceneConfiguration2.tent.createdWgoUniqueId = wgoData7.UniqueId;
			if (townBuildingTierSceneConfiguration != null)
			{
				WgoData wgoData8 = MainGame.WorldData.GetWgoData(townBuildingTierSceneConfiguration.tent.createdWgoUniqueId);
				if (!wgoData8.LinkedFromTownBuildingUniqueId.IsEmpty)
				{
					WgoData wgoData9 = MainGame.WorldData.GetWgoData(wgoData8.LinkedFromTownBuildingUniqueId);
					wgoData9.LinkedToTownBuildingUniqueId = wgoData7.UniqueId;
					wgoData7.LinkedFromTownBuildingUniqueId = wgoData8.LinkedFromTownBuildingUniqueId;
					wgoData7.SetGameRes("available_by_time", wgoData9.GetGameResInt("available_by_time"));
				}
				if (!townBuildingTierSceneConfiguration.yard.createdWgoUniqueId.IsEmpty)
				{
					MainGame.WorldData.RemoveWgoDataFromGameScene(townBuildingTierSceneConfiguration.yard.createdWgoUniqueId);
				}
				if (!townBuildingTierSceneConfiguration.decor1.createdWgoUniqueId.IsEmpty)
				{
					MainGame.WorldData.RemoveWgoDataFromGameScene(townBuildingTierSceneConfiguration.decor1.createdWgoUniqueId);
				}
				if (!townBuildingTierSceneConfiguration.decor2.createdWgoUniqueId.IsEmpty)
				{
					MainGame.WorldData.RemoveWgoDataFromGameScene(townBuildingTierSceneConfiguration.decor2.createdWgoUniqueId);
				}
				if (!townBuildingTierSceneConfiguration.decor3.createdWgoUniqueId.IsEmpty)
				{
					MainGame.WorldData.RemoveWgoDataFromGameScene(townBuildingTierSceneConfiguration.decor3.createdWgoUniqueId);
				}
				if (!townBuildingTierSceneConfiguration.sign.createdWgoUniqueId.IsEmpty)
				{
					MainGame.WorldData.RemoveWgoDataFromGameScene(townBuildingTierSceneConfiguration.sign.createdWgoUniqueId);
				}
				MainGame.WorldData.RemoveWgoDataFromGameScene(townBuildingTierSceneConfiguration.tent.createdWgoUniqueId);
				using (List<LazyExpression>.Enumerator enumerator = data.expressionOnCharCreate.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LazyExpression lazyExpression = enumerator.Current;
						lazyExpression.EvaluateBool(wgoData);
					}
					goto IL_08E2;
				}
			}
			WgoData wgoData10 = new WgoData(data.characterId, wgoData.Position, wgoData.WorldId);
			wgoData10.CustomTag = customTag.Replace(text, "t_b_character_");
			wgoData10.LinkedToTownBuildingUniqueId = wgoData7.UniqueId;
			wgoData7.LinkedFromTownBuildingUniqueId = wgoData10.UniqueId;
			MainGame.WorldData.AddWgoData(wgoData10);
			foreach (LazyExpression lazyExpression2 in data.expressionOnCharCreate)
			{
				lazyExpression2.EvaluateBool(wgoData10);
			}
			MainGame.WorldData.RemoveWgoDataFromGameScene(wgoData.UniqueId);
		}
		else
		{
			WgoData wgoData11 = new WgoData(data.id, wgoData.Position, wgoData.WorldId);
			wgoData11.CustomTag = customTag.Replace(text, "t_b_tent_");
			wgoData11.TownBuildingWgoComponent = wgoData.TownBuildingWgoComponent;
			TownBuildingWgoComponent townBuildingWgoComponent3 = wgoData11.TownBuildingWgoComponent;
			int num = townBuildingWgoComponent3.TierIndex;
			townBuildingWgoComponent3.TierIndex = num + 1;
			wgoData11.MainWgoPartData.variationId = data.variationId;
			wgoData11.MainWgoPartData.rotationIndex = -1;
			MainGame.WorldData.AddWgoData(wgoData11);
			if (!string.IsNullOrEmpty(data.characterId))
			{
				WgoData wgoData12 = new WgoData(data.characterId, wgoData.Position, wgoData.WorldId);
				wgoData12.CustomTag = customTag.Replace(text, "t_b_character_");
				wgoData12.LinkedToTownBuildingUniqueId = wgoData11.UniqueId;
				wgoData11.LinkedFromTownBuildingUniqueId = wgoData12.UniqueId;
				MainGame.WorldData.AddWgoData(wgoData12);
				using (List<LazyExpression>.Enumerator enumerator = data.expressionOnCharCreate.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LazyExpression lazyExpression3 = enumerator.Current;
						lazyExpression3.EvaluateBool(wgoData12);
					}
					goto IL_08D2;
				}
			}
			if (!wgoData.LinkedFromTownBuildingUniqueId.IsEmpty)
			{
				WgoData wgoData13 = MainGame.WorldData.GetWgoData(wgoData.LinkedFromTownBuildingUniqueId);
				wgoData13.LinkedToTownBuildingUniqueId = wgoData11.UniqueId;
				wgoData11.LinkedFromTownBuildingUniqueId = wgoData.LinkedFromTownBuildingUniqueId;
				wgoData11.SetGameRes("available_by_time", wgoData13.GetGameResInt("available_by_time"));
			}
			IL_08D2:
			MainGame.WorldData.RemoveWgoDataFromGameScene(wgoData.UniqueId);
		}
		IL_08E2:
		this.UpdateTownBuildingVendor(data, tierIndex > 0);
	}

	// Token: 0x06001CE8 RID: 7400 RVA: 0x00087C04 File Offset: 0x00085E04
	private void UpdateTownBuildingVendor(TownBuildingDef def, bool forceLevelUp)
	{
		if (string.IsNullOrEmpty(def.vendorId))
		{
			return;
		}
		KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
		if (!knowledgeSystem.IsVendorForOrdersUnlocked(def.vendorId))
		{
			knowledgeSystem.UnlockVendorForOrders(def.vendorId);
		}
		if (forceLevelUp)
		{
			MainGame.Instance.GameSave.vendorSystem.ForceLevelUpVendor(def.vendorId);
		}
	}

	// Token: 0x04001AE5 RID: 6885
	public const string CHALK_BOARD_ID = "town_chalk_board";

	// Token: 0x04001AE8 RID: 6888
	[SerializeField]
	private int quality;

	// Token: 0x04001AE9 RID: 6889
	public static int moneyForPalettes;
}
