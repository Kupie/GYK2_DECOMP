using System;
using System.Collections.Generic;
using JetBrains.Annotations;

// Token: 0x0200055C RID: 1372
public class BuildData
{
	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06002329 RID: 9001 RVA: 0x000A4837 File Offset: 0x000A2A37
	public BuildingDef.BuildingMode BuildingMode
	{
		get
		{
			return this.buildingMode;
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x0600232A RID: 9002 RVA: 0x000A483F File Offset: 0x000A2A3F
	[CanBeNull]
	public BuildingDef Definition
	{
		get
		{
			return this.definition;
		}
	}

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x0600232B RID: 9003 RVA: 0x000A4847 File Offset: 0x000A2A47
	// (set) Token: 0x0600232C RID: 9004 RVA: 0x000A484F File Offset: 0x000A2A4F
	public string WgoId { get; private set; }

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x0600232D RID: 9005 RVA: 0x000A4858 File Offset: 0x000A2A58
	public string IconId
	{
		get
		{
			if (this.definition == null)
			{
				return "i_b_remove";
			}
			return this.definition.BuildResultIcon;
		}
	}

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x0600232E RID: 9006 RVA: 0x000A4873 File Offset: 0x000A2A73
	public List<NeedItemData> NeedItems
	{
		get
		{
			BuildingDef buildingDef = this.definition;
			if (buildingDef == null)
			{
				return null;
			}
			return buildingDef.needItems;
		}
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x000A4886 File Offset: 0x000A2A86
	public static BuildData GetDataForBuild(BuildingDef buildingDef)
	{
		return new BuildData
		{
			buildingMode = buildingDef.buildingMode,
			definition = buildingDef,
			WgoId = buildingDef.wgoId
		};
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000A48AC File Offset: 0x000A2AAC
	public static BuildData GetDataForRemove()
	{
		return new BuildData
		{
			buildingMode = BuildingDef.BuildingMode.Remove
		};
	}

	// Token: 0x04001F9B RID: 8091
	private const string REMOVE_BUILD_DATA_POINTER_ICON_ID = "i_b_remove";

	// Token: 0x04001F9C RID: 8092
	private BuildingDef.BuildingMode buildingMode;

	// Token: 0x04001F9D RID: 8093
	[CanBeNull]
	private BuildingDef definition;
}
