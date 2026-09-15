using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000AA7 RID: 2727
[Serializable]
public class ReplacementMapping
{
	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x060049CD RID: 18893 RVA: 0x0015C9BD File Offset: 0x0015ABBD
	public string RepairedModelId
	{
		get
		{
			return this.repairedModelId;
		}
	}

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x060049CE RID: 18894 RVA: 0x0015C9C5 File Offset: 0x0015ABC5
	public string RepairedAssetPath
	{
		get
		{
			return this.repairedAssetPath;
		}
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x0015C9D0 File Offset: 0x0015ABD0
	public bool ContainsBrokenModel(string modelId)
	{
		if (string.IsNullOrEmpty(modelId))
		{
			return false;
		}
		using (List<string>.Enumerator enumerator = this.brokenModelIds.GetEnumerator())
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

	// Token: 0x060049D0 RID: 18896 RVA: 0x0015CA34 File Offset: 0x0015AC34
	public bool TryGetBrokenAssetPath(string brokenModelId, out string brokenAssetPath)
	{
		brokenAssetPath = null;
		if (string.IsNullOrEmpty(brokenModelId))
		{
			return false;
		}
		for (int i = 0; i < this.brokenModelIds.Count; i++)
		{
			if (!(this.brokenModelIds[i] != brokenModelId))
			{
				if (i < this.brokenAssetPaths.Count)
				{
					brokenAssetPath = this.brokenAssetPaths[i];
				}
				return !string.IsNullOrEmpty(brokenAssetPath);
			}
		}
		return false;
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x0015CAA0 File Offset: 0x0015ACA0
	public List<string> GetBrokenModelIds()
	{
		return new List<string>(this.brokenModelIds);
	}

	// Token: 0x04003993 RID: 14739
	[Tooltip("Addressable references for broken prefabs that can be replaced (many-to-one support)")]
	public List<AssetReferenceGameObject> brokenPrefabRefs = new List<AssetReferenceGameObject>();

	// Token: 0x04003994 RID: 14740
	[Tooltip("Addressable reference for the repaired prefab")]
	public AssetReferenceGameObject repairedPrefabRef;

	// Token: 0x04003995 RID: 14741
	[Tooltip("Broken model IDs cached for runtime lookup")]
	[SerializeField]
	private List<string> brokenModelIds = new List<string>();

	// Token: 0x04003996 RID: 14742
	[Tooltip("Addressables paths for broken prefabs (parallel to brokenModelIds, editor-synced)")]
	[SerializeField]
	private List<string> brokenAssetPaths = new List<string>();

	// Token: 0x04003997 RID: 14743
	[Tooltip("Repaired model ID cached for runtime lookup")]
	[SerializeField]
	private string repairedModelId;

	// Token: 0x04003998 RID: 14744
	[Tooltip("Addressables path for the repaired prefab (auto-populated in editor)")]
	[SerializeField]
	private string repairedAssetPath;
}
