using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000AA6 RID: 2726
[CreateAssetMenu(fileName = "ConstructorPartReplacement", menuName = "GK2/Constructor/Replacement Config")]
public class ConstructorPartReplacementConfig : ScriptableObject
{
	// Token: 0x060049C7 RID: 18887 RVA: 0x0015C758 File Offset: 0x0015A958
	public bool TryGetLutAssetPath(bool useRepairedLut, string lutName, out string lutAssetPath)
	{
		lutAssetPath = null;
		if (string.IsNullOrEmpty(lutName) || lutName == LazyConsts.NO_LUT)
		{
			return false;
		}
		string text = (useRepairedLut ? this.repairedLutsDirectoryPath : this.brokenLutsDirectoryPath);
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		text = text.Replace('\\', '/').TrimEnd('/');
		string text2 = (lutName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? lutName : (lutName + ".png"));
		lutAssetPath = text + "/" + text2;
		return true;
	}

	// Token: 0x060049C8 RID: 18888 RVA: 0x0015C7DC File Offset: 0x0015A9DC
	public bool TryGetRepairedModel(string brokenModelId, out string repairedModelId)
	{
		repairedModelId = null;
		foreach (ReplacementMapping replacementMapping in this.mappings)
		{
			if (replacementMapping.ContainsBrokenModel(brokenModelId))
			{
				repairedModelId = replacementMapping.RepairedModelId;
				return !string.IsNullOrEmpty(repairedModelId);
			}
		}
		return false;
	}

	// Token: 0x060049C9 RID: 18889 RVA: 0x0015C84C File Offset: 0x0015AA4C
	public bool TryGetBrokenAssetPath(string brokenModelId, out string brokenAssetPath)
	{
		brokenAssetPath = null;
		using (List<ReplacementMapping>.Enumerator enumerator = this.mappings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.TryGetBrokenAssetPath(brokenModelId, out brokenAssetPath))
				{
					return !string.IsNullOrEmpty(brokenAssetPath);
				}
			}
		}
		return false;
	}

	// Token: 0x060049CA RID: 18890 RVA: 0x0015C8B4 File Offset: 0x0015AAB4
	public bool TryGetRepairedModelWithPath(string brokenModelId, out string repairedModelId, out string repairedAssetPath)
	{
		repairedModelId = null;
		repairedAssetPath = null;
		foreach (ReplacementMapping replacementMapping in this.mappings)
		{
			if (replacementMapping.ContainsBrokenModel(brokenModelId))
			{
				repairedModelId = replacementMapping.RepairedModelId;
				repairedAssetPath = replacementMapping.RepairedAssetPath;
				return !string.IsNullOrEmpty(repairedModelId);
			}
		}
		return false;
	}

	// Token: 0x060049CB RID: 18891 RVA: 0x0015C930 File Offset: 0x0015AB30
	public bool ShouldDeleteModel(string modelId)
	{
		if (string.IsNullOrEmpty(modelId))
		{
			return false;
		}
		using (List<string>.Enumerator enumerator = this.deletionModelIds.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == modelId)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0400398D RID: 14733
	[Tooltip("ID of the preset this config belongs to (e.g. 'broken', 'broken_2')")]
	public string presetId;

	// Token: 0x0400398E RID: 14734
	[Tooltip("List of replacement mappings from broken to repaired models")]
	public List<ReplacementMapping> mappings = new List<ReplacementMapping>();

	// Token: 0x0400398F RID: 14735
	[Tooltip("Addressable references for prefabs that should be deleted (not replaced) during repair")]
	public List<AssetReferenceGameObject> deletionPrefabRefs = new List<AssetReferenceGameObject>();

	// Token: 0x04003990 RID: 14736
	[Tooltip("Model IDs for prefabs that should be deleted (not replaced) during repair")]
	[SerializeField]
	private List<string> deletionModelIds = new List<string>();

	// Token: 0x04003991 RID: 14737
	[Tooltip("Directory path for broken LUTs")]
	public string brokenLutsDirectoryPath;

	// Token: 0x04003992 RID: 14738
	[Tooltip("Directory path for repaired LUTs")]
	public string repairedLutsDirectoryPath;
}
