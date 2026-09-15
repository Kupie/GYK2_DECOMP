using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000720 RID: 1824
public class SceneWgoContentPart : MonoBehaviour
{
	// Token: 0x1700075F RID: 1887
	// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x000E5060 File Offset: 0x000E3260
	public bool AutoLoadContent
	{
		get
		{
			return this.autoLoadContent;
		}
	}

	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x000E5068 File Offset: 0x000E3268
	public IReadOnlyList<WgoData> Wgos
	{
		get
		{
			return this.wgos;
		}
	}

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x06002FB9 RID: 12217 RVA: 0x000E5070 File Offset: 0x000E3270
	public IReadOnlyList<WsoData> Wsos
	{
		get
		{
			return this.wsos;
		}
	}

	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000E5078 File Offset: 0x000E3278
	public IReadOnlyList<SGuid> WgoUniqueIds
	{
		get
		{
			return this.wgoUniqueIds;
		}
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x06002FBB RID: 12219 RVA: 0x000E5080 File Offset: 0x000E3280
	public IReadOnlyList<SGuid> WsoUniqueIds
	{
		get
		{
			return this.wsoUniqueIds;
		}
	}

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000E5088 File Offset: 0x000E3288
	public IReadOnlyList<WorldZoneBakedData> WorldZones
	{
		get
		{
			return this.worldZones;
		}
	}

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000E5090 File Offset: 0x000E3290
	public string ExplicitWorldId
	{
		get
		{
			return this.explicitWorldId;
		}
	}

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x06002FBE RID: 12222 RVA: 0x000E5098 File Offset: 0x000E3298
	public string WorldId
	{
		get
		{
			if (this.explicitSetWorldId)
			{
				return this.explicitWorldId;
			}
			int num = base.name.IndexOf("_ContentPart", StringComparison.Ordinal);
			if (num < 0)
			{
				return base.name;
			}
			return base.name.Substring(0, num);
		}
	}

	// Token: 0x040026A1 RID: 9889
	public static bool IS_BAKING_IN_PROGRESS;

	// Token: 0x040026A2 RID: 9890
	private const string WORLD_ZONE_DATA_FOLDER = "Assets/AddressableAssets/WorldZones/Data";

	// Token: 0x040026A3 RID: 9891
	[SerializeField]
	private bool autoLoadContent = true;

	// Token: 0x040026A4 RID: 9892
	[SerializeField]
	private bool explicitSetWorldId;

	// Token: 0x040026A5 RID: 9893
	[SerializeField]
	private string explicitWorldId;

	// Token: 0x040026A6 RID: 9894
	[SerializeField]
	private List<WgoData> wgos = new List<WgoData>();

	// Token: 0x040026A7 RID: 9895
	[SerializeField]
	private List<SGuid> wgoUniqueIds = new List<SGuid>();

	// Token: 0x040026A8 RID: 9896
	[SerializeField]
	private List<SGuid> wsoUniqueIds = new List<SGuid>();

	// Token: 0x040026A9 RID: 9897
	[SerializeField]
	private List<WsoData> wsos = new List<WsoData>();

	// Token: 0x040026AA RID: 9898
	[SerializeField]
	private List<WorldZoneBakedData> worldZones = new List<WorldZoneBakedData>();
}
