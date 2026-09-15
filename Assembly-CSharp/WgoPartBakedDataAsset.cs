using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

// Token: 0x020005CB RID: 1483
[CreateAssetMenu(menuName = "WgoPartBakedDataAsset")]
public class WgoPartBakedDataAsset : SerializedScriptableObject
{
	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x060026E1 RID: 9953 RVA: 0x000B6BD7 File Offset: 0x000B4DD7
	public WgoPartBakedData Data
	{
		get
		{
			return this.bakedData;
		}
	}

	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x060026E2 RID: 9954 RVA: 0x000B6BDF File Offset: 0x000B4DDF
	public string Id
	{
		get
		{
			WgoPartBakedData wgoPartBakedData = this.bakedData;
			if (wgoPartBakedData == null)
			{
				return null;
			}
			return wgoPartBakedData.id;
		}
	}

	// Token: 0x060026E3 RID: 9955 RVA: 0x000B6BF2 File Offset: 0x000B4DF2
	public void SetData(WgoPartBakedData data)
	{
		this.bakedData = data;
	}

	// Token: 0x04002171 RID: 8561
	[OdinSerialize]
	private WgoPartBakedData bakedData;
}
