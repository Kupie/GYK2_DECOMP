using System;
using System.Collections.Generic;
using System.IO;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public class ConveyorPreset : ISerializableData
{
	// Token: 0x060023D7 RID: 9175 RVA: 0x000A7E08 File Offset: 0x000A6008
	public static void SaveLayoutToPreset(string presetName)
	{
		if (File.Exists(Application.dataPath + "/AddressableAssets/ConveyorPresets/" + presetName + ".bytes"))
		{
			File.Delete(Application.dataPath + "/AddressableAssets/ConveyorPresets/" + presetName + ".bytes");
		}
		ConveyorPreset conveyorPreset = new ConveyorPreset();
		conveyorPreset.presetName = presetName;
		foreach (ConveyorComponent conveyorComponent in MainGame.Instance.GameSave.conveyorSystemData.conveyorComponents)
		{
			ConveyorWgoData conveyorWgoData = conveyorComponent.WgoData;
			conveyorPreset.wgoData.Add(conveyorWgoData);
			if (conveyorWgoData.Worker != null)
			{
				conveyorPreset.workers.Add(conveyorWgoData.Worker as ZombieWgoData);
			}
			if (conveyorComponent is ConveyorPowerSourceComponent)
			{
				List<DockPointData> dockPoints = conveyorWgoData.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyOccupied, DockPointData.Filter.OnlyZombie);
				if (dockPoints.Count > 0)
				{
					foreach (DockPointData dockPointData in dockPoints)
					{
						SGuid occupiedBy = dockPointData.OccupiedBy;
						ZombieWgoData zombie = MainGame.Instance.GameSave.zombieSystemData.GetZombie(occupiedBy);
						conveyorPreset.carouselWorkers.Add(zombie);
					}
				}
			}
			foreach (SGuid sguid in conveyorComponent.WgoData.AttachedWorkbenchExtensions)
			{
				conveyorPreset.extensions.Add(MainGame.WorldData.GetWgoData(sguid));
			}
		}
		ConveyorSaveSystem.SaveConveyorPreset(conveyorPreset, Application.dataPath + "/AddressableAssets/ConveyorPresets/", presetName, null, null, true);
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000A7FF4 File Offset: 0x000A61F4
	public static void LoadPreset(string presetName)
	{
		MainGame.Instance.conveyorSystem.IsPaused = true;
		MainGame.Instance.conveyorSystem.isReconstructLocked = true;
		MainGame.Instance.conveyorSystem.Clear();
		ConveyorSaveSystem.LoadConveyorPreset("Assets/AddressableAssets/ConveyorPresets/", presetName, new Action<ConveyorPreset>(ConveyorPreset.ApplyPreset));
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000A8048 File Offset: 0x000A6248
	private static void ApplyPreset(ConveyorPreset preset)
	{
		Dictionary<Guid, ZombieWgoData> cache = MainGame.Instance.GameSave.zombieSystemData.Cache;
		List<SGuid> zombieOnSceneWgoIds = MainGame.Instance.GameSave.zombieSystemData.zombieOnSceneWgoIds;
		WorldData worldData = MainGame.Instance.GameSave.worldData;
		List<ConveyorComponent> conveyorComponents = MainGame.ConveyorSystemData.conveyorComponents;
		foreach (ZombieWgoData zombieWgoData in preset.workers)
		{
			zombieOnSceneWgoIds.Add(zombieWgoData.UniqueId);
			cache.Add(zombieWgoData.UniqueId.Guid, zombieWgoData);
			zombieWgoData.PrepareForGame();
			worldData.AddWgoData(zombieWgoData, true);
		}
		if (preset.carouselWorkers != null)
		{
			foreach (ZombieWgoData zombieWgoData2 in preset.carouselWorkers)
			{
				zombieOnSceneWgoIds.Add(zombieWgoData2.UniqueId);
				cache.Add(zombieWgoData2.UniqueId.Guid, zombieWgoData2);
				zombieWgoData2.PrepareForGame();
				worldData.AddWgoData(zombieWgoData2, true);
			}
		}
		foreach (ConveyorWgoData conveyorWgoData in preset.wgoData)
		{
			conveyorComponents.Add(conveyorWgoData.ConveyorComponent);
			conveyorWgoData.PrepareForGame();
			worldData.AddWgoData(conveyorWgoData, true);
		}
		if (preset.extensions != null)
		{
			foreach (WgoData wgoData in preset.extensions)
			{
				wgoData.PrepareForGame();
				worldData.AddWgoData(wgoData, true);
			}
		}
		foreach (ZombieWgoData zombieWgoData3 in preset.workers)
		{
			zombieWgoData3.PrepareForGame();
		}
		MainGame.Instance.conveyorSystem.isReconstructLocked = false;
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x00002318 File Offset: 0x00000518
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x00002318 File Offset: 0x00000518
	public void OnAfterSerialize()
	{
	}

	// Token: 0x04001FEE RID: 8174
	public const string CONVEYOR_PRESETS_LOCAL_DIRECTORY = "/AddressableAssets/ConveyorPresets/";

	// Token: 0x04001FEF RID: 8175
	public const string CONVEYOR_PRESETS_ADDRESSABLES_FOLDER = "Assets/AddressableAssets/ConveyorPresets/";

	// Token: 0x04001FF0 RID: 8176
	public const string CONVEYOR_PRESETS_ADDRESSABLES_LABEL = "ConveyorPresets";

	// Token: 0x04001FF1 RID: 8177
	public string presetName;

	// Token: 0x04001FF2 RID: 8178
	public List<ConveyorWgoData> wgoData = new List<ConveyorWgoData>();

	// Token: 0x04001FF3 RID: 8179
	public List<ZombieWgoData> workers = new List<ZombieWgoData>();

	// Token: 0x04001FF4 RID: 8180
	public List<ZombieWgoData> carouselWorkers = new List<ZombieWgoData>();

	// Token: 0x04001FF5 RID: 8181
	public List<WgoData> extensions = new List<WgoData>();
}
