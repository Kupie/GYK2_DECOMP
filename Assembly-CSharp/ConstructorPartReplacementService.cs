using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000AA8 RID: 2728
public static class ConstructorPartReplacementService
{
	// Token: 0x060049D3 RID: 18899 RVA: 0x0015CAD6 File Offset: 0x0015ACD6
	public static void Initialize()
	{
		if (ConstructorPartReplacementService.isInitialized)
		{
			return;
		}
		ConstructorPartReplacementService.configsByPresetId = new Dictionary<string, ConstructorPartReplacementConfig>();
		ConstructorPartReplacementService.configsById = new Dictionary<string, ConstructorPartReplacementConfig>();
		ConstructorPartReplacementService.LoadAllConfigs();
		ConstructorPartReplacementService.isInitialized = true;
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x0015CAFF File Offset: 0x0015ACFF
	public static void Reload()
	{
		ConstructorPartReplacementService.isInitialized = false;
		ConstructorPartReplacementService.Initialize();
	}

	// Token: 0x060049D5 RID: 18901 RVA: 0x0015CB0C File Offset: 0x0015AD0C
	public static ConstructorPartReplacementConfig GetReplacementConfigForPreset(string presetId)
	{
		if (string.IsNullOrEmpty(presetId))
		{
			return null;
		}
		if (!ConstructorPartReplacementService.isInitialized)
		{
			ConstructorPartReplacementService.Initialize();
		}
		ConstructorPartReplacementConfig constructorPartReplacementConfig;
		ConstructorPartReplacementService.configsByPresetId.TryGetValue(presetId, out constructorPartReplacementConfig);
		return constructorPartReplacementConfig;
	}

	// Token: 0x060049D6 RID: 18902 RVA: 0x0015CB40 File Offset: 0x0015AD40
	public static ConstructorPartReplacementConfig GetReplacementConfig(string configId)
	{
		if (string.IsNullOrEmpty(configId))
		{
			return null;
		}
		if (!ConstructorPartReplacementService.isInitialized)
		{
			ConstructorPartReplacementService.Initialize();
		}
		ConstructorPartReplacementConfig constructorPartReplacementConfig;
		ConstructorPartReplacementService.configsById.TryGetValue(configId, out constructorPartReplacementConfig);
		return constructorPartReplacementConfig;
	}

	// Token: 0x060049D7 RID: 18903 RVA: 0x0015CB74 File Offset: 0x0015AD74
	public static bool TryGetRepairedModel(string brokenModelId, out string repairedModelId)
	{
		repairedModelId = null;
		if (string.IsNullOrEmpty(brokenModelId))
		{
			return false;
		}
		if (!ConstructorPartReplacementService.isInitialized)
		{
			ConstructorPartReplacementService.Initialize();
		}
		string text = ConstructorPartReplacementService.ExtractPresetId(brokenModelId);
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		ConstructorPartReplacementConfig constructorPartReplacementConfig;
		if (ConstructorPartReplacementService.configsByPresetId.TryGetValue(text, out constructorPartReplacementConfig))
		{
			return constructorPartReplacementConfig.TryGetRepairedModel(brokenModelId, out repairedModelId);
		}
		foreach (KeyValuePair<string, ConstructorPartReplacementConfig> keyValuePair in ConstructorPartReplacementService.configsByPresetId)
		{
			if (keyValuePair.Value.TryGetRepairedModel(brokenModelId, out repairedModelId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x0015CC1C File Offset: 0x0015AE1C
	public static int ApplyRepairToWso(Wso wso)
	{
		if (wso == null || wso.Data == null)
		{
			return 0;
		}
		WsoRepairablePartData componentData = wso.Data.GetComponentData<WsoRepairablePartData>();
		if (componentData == null)
		{
			return 0;
		}
		ConstructorPartReplacementConfig constructorPartReplacementConfig = wso.Data.Definition.ReplacementConfig;
		if (constructorPartReplacementConfig == null && wso.RuntimeConstructorParts.Count > 0)
		{
			ConstructorPart constructorPart = wso.RuntimeConstructorParts[0];
			if (constructorPart != null)
			{
				ConstructorPartChildData constructorPartChildData = constructorPart.constructorPartChildData;
				constructorPartReplacementConfig = ConstructorPartReplacementService.GetReplacementConfigForPreset(ConstructorPartReplacementService.ExtractPresetId(((constructorPartChildData != null) ? constructorPartChildData.pathToObject : null) ?? ""));
			}
		}
		if (constructorPartReplacementConfig == null)
		{
			Debug.LogWarning("[ConstructorPartReplacementService] No replacement config found for Wso: " + wso.name);
			return 0;
		}
		return componentData.RepairAllStages(constructorPartReplacementConfig);
	}

	// Token: 0x060049D9 RID: 18905 RVA: 0x0015CCD8 File Offset: 0x0015AED8
	private static string ExtractPresetId(string modelName)
	{
		if (string.IsNullOrEmpty(modelName))
		{
			return null;
		}
		int num = modelName.IndexOf('-');
		if (num <= 0)
		{
			return null;
		}
		return modelName.Substring(0, num);
	}

	// Token: 0x060049DA RID: 18906 RVA: 0x0015CD08 File Offset: 0x0015AF08
	private static void LoadAllConfigs()
	{
		foreach (ConstructorPartReplacementConfig constructorPartReplacementConfig in Addressables.LoadAssetsAsync<ConstructorPartReplacementConfig>("ConstructorPartReplacements", null).WaitForCompletion())
		{
			ConstructorPartReplacementService.RegisterConfig(constructorPartReplacementConfig);
		}
	}

	// Token: 0x060049DB RID: 18907 RVA: 0x0015CD60 File Offset: 0x0015AF60
	private static void RegisterConfig(ConstructorPartReplacementConfig config)
	{
		if (config == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(config.presetId))
		{
			if (!ConstructorPartReplacementService.configsByPresetId.ContainsKey(config.presetId))
			{
				ConstructorPartReplacementService.configsByPresetId[config.presetId] = config;
			}
			else
			{
				Debug.LogWarning("[ConstructorPartReplacementService] Duplicate preset ID '" + config.presetId + "' found. Using first config.");
			}
		}
		string name = config.name;
		if (!string.IsNullOrEmpty(name))
		{
			ConstructorPartReplacementService.configsById[name] = config;
		}
	}

	// Token: 0x04003999 RID: 14745
	private const string ADDRESSABLES_LABEL = "ConstructorPartReplacements";

	// Token: 0x0400399A RID: 14746
	private static Dictionary<string, ConstructorPartReplacementConfig> configsByPresetId;

	// Token: 0x0400399B RID: 14747
	private static Dictionary<string, ConstructorPartReplacementConfig> configsById;

	// Token: 0x0400399C RID: 14748
	private static bool isInitialized;
}
