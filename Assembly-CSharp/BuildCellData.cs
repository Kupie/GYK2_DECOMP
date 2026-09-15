using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000146 RID: 326
public struct BuildCellData
{
	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0002668B File Offset: 0x0002488B
	public int State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00026693 File Offset: 0x00024893
	public Vector3 Coords
	{
		get
		{
			return this.coords;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0002669B File Offset: 0x0002489B
	public BuildArea BuildAreaWithCovering
	{
		get
		{
			return this.buildAreaWithCovering;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000266A3 File Offset: 0x000248A3
	public HashSet<SGuid> ExtensionList
	{
		get
		{
			return this.extensionList;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060007D5 RID: 2005 RVA: 0x000266AB File Offset: 0x000248AB
	public bool ContainsExtensionParent
	{
		get
		{
			return this.containsExtensionParent;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060007D6 RID: 2006 RVA: 0x000266B3 File Offset: 0x000248B3
	public ModuleSlotArea SlotArea
	{
		get
		{
			return this.slotArea;
		}
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x000266BB File Offset: 0x000248BB
	public void AddState(BuildCellData.CellState stateFlag)
	{
		this.state |= (int)stateFlag;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x000266CC File Offset: 0x000248CC
	public static BuildCellData GetData(Vector3 coords, string zoneId, BuildingDef buildingDef = null)
	{
		BuildCellData buildCellData = new BuildCellData
		{
			coords = coords,
			buildingDef = buildingDef,
			hasCustomBuildAreaId = !string.IsNullOrEmpty((buildingDef != null) ? buildingDef.customBuildAreaId : null),
			chooseBuildAreaType = ((buildingDef != null) ? buildingDef.chooseCustomBuildAreaType : BuildingDef.BuildAreaChoosingType.None)
		};
		buildCellData.UpdateData();
		return buildCellData;
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0002672C File Offset: 0x0002492C
	public void UpdateData()
	{
		this.containsExtensionParent = false;
		this.slotArea = null;
		if (this.extensionList == null)
		{
			this.extensionList = new HashSet<SGuid>();
		}
		this.extensionList.Clear();
		int num = Physics.OverlapBoxNonAlloc(this.coords, BuildConsts.CASTING_BOX_HALF_EXTENTS, BuildCellData.buildAreaColliders, Quaternion.identity, 269091072);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = true;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		for (int i = 0; i < num; i++)
		{
			Collider collider = BuildCellData.buildAreaColliders[i];
			if (!(collider == null))
			{
				WorldZone worldZone;
				if (collider.TryGetComponent<WorldZone>(out worldZone))
				{
					flag = true;
				}
				else if (collider.gameObject.layer != 19 || !(collider.gameObject.GetComponentInParent<BuildPointerObject>() != null))
				{
					PlacementBlockingArea placementBlockingArea;
					if (PlacementBlockingArea.TryGet(collider, out placementBlockingArea))
					{
						if (this.buildingDef != null && PlacementBlockingArea.IsBlockingFor(collider, this.buildingDef, null))
						{
							flag10 = true;
							flag5 = true;
						}
					}
					else
					{
						if (!flag7 && collider.gameObject.layer == 28)
						{
							flag7 = true;
						}
						BuildArea buildArea;
						if (collider.TryGetComponent<BuildArea>(out buildArea))
						{
							flag2 = true;
							if (this.slotArea == null)
							{
								this.slotArea = buildArea.GetComponent<ModuleSlotArea>();
							}
							Wgo componentInParent = collider.GetComponentInParent<Wgo>();
							if (componentInParent != null && componentInParent.GetComponentInParent<BuildPointerObject>() == null && componentInParent.IsBuildRemovable())
							{
								flag6 = true;
								if (this.buildingDef == null)
								{
									flag9 = true;
									flag5 = true;
								}
							}
							switch (this.chooseBuildAreaType)
							{
							case BuildingDef.BuildAreaChoosingType.Soft:
								if (this.hasCustomBuildAreaId)
								{
									string id = buildArea.Id;
									BuildingDef buildingDef = this.buildingDef;
									if (id == ((buildingDef != null) ? buildingDef.customBuildAreaId : null))
									{
										flag3 = true;
									}
								}
								break;
							case BuildingDef.BuildAreaChoosingType.Strict:
							{
								string id2 = buildArea.Id;
								BuildingDef buildingDef2 = this.buildingDef;
								if (!(id2 == ((buildingDef2 != null) ? buildingDef2.customBuildAreaId : null)))
								{
									flag4 = false;
									goto IL_03AE;
								}
								goto IL_03AE;
							}
							case BuildingDef.BuildAreaChoosingType.FullCoverWithCount:
							{
								string id3 = buildArea.Id;
								BuildingDef buildingDef3 = this.buildingDef;
								if (id3 == ((buildingDef3 != null) ? buildingDef3.customBuildAreaId : null))
								{
									flag3 = true;
									if (buildArea.fullCoveringMode)
									{
										this.buildAreaWithCovering = buildArea;
										goto IL_03AE;
									}
									goto IL_03AE;
								}
								else
								{
									flag4 = false;
								}
								break;
							}
							case BuildingDef.BuildAreaChoosingType.FullCoverSoft:
								if (!flag3)
								{
									string id4 = buildArea.Id;
									BuildingDef buildingDef4 = this.buildingDef;
									if (id4 == ((buildingDef4 != null) ? buildingDef4.customBuildAreaId : null))
									{
										flag3 = true;
										flag8 = true;
									}
								}
								break;
							}
							Wgo componentInParent2 = collider.GetComponentInParent<Wgo>();
							if (componentInParent2 != null && componentInParent2.GetComponentInParent<BuildPointerObject>() == null && GameBalance.Me.workbenchesWhichUseExtensions.Contains(componentInParent2.Data.Definition))
							{
								this.containsExtensionParent = true;
							}
						}
						else
						{
							Wgo componentInParent3 = collider.GetComponentInParent<Wgo>();
							if (componentInParent3 != null)
							{
								ModuleSlotArea moduleSlotArea;
								if (!(componentInParent3.GetComponentInParent<BuildPointerObject>() != null) && !collider.TryGetComponent<ModuleSlotArea>(out moduleSlotArea) && (this.buildingDef == null || !this.buildingDef.ShouldIgnoreWgoGroupAsObstacle(componentInParent3.Data.Definition.wgoGroup)))
								{
									if (collider.gameObject.layer == 28)
									{
										this.extensionList.Add(componentInParent3.Data.UniqueId);
									}
									else
									{
										if (this.chooseBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft)
										{
											Dictionary<string, List<string>> customBuildAreaIdToWgoIds = GameBalance.Me.customBuildAreaIdToWgoIds;
											BuildingDef buildingDef5 = this.buildingDef;
											List<string> list;
											if (customBuildAreaIdToWgoIds.TryGetValue((buildingDef5 != null) ? buildingDef5.customBuildAreaId : null, out list) && list.Contains(componentInParent3.Data.id))
											{
												flag8 = false;
											}
										}
										flag5 = true;
										if (!flag9)
										{
											flag6 = componentInParent3.IsBuildRemovable();
										}
									}
								}
							}
							else
							{
								int layer = collider.gameObject.layer;
								if (layer == 8 || layer == 19)
								{
									flag5 = true;
								}
							}
						}
					}
				}
			}
			IL_03AE:;
		}
		this.state = 0;
		this.state |= 1;
		if (!flag)
		{
			return;
		}
		if (flag7)
		{
			this.state |= 8;
		}
		if (this.containsExtensionParent && this.extensionList.Count > 0)
		{
			this.state |= 64;
		}
		switch (this.chooseBuildAreaType)
		{
		case BuildingDef.BuildAreaChoosingType.Soft:
		case BuildingDef.BuildAreaChoosingType.FullCoverWithCount:
			if (this.hasCustomBuildAreaId && !flag3)
			{
				return;
			}
			if (!this.hasCustomBuildAreaId && !flag2)
			{
				return;
			}
			break;
		case BuildingDef.BuildAreaChoosingType.Strict:
			if (!flag2)
			{
				return;
			}
			if (!flag4)
			{
				this.state |= 6;
				return;
			}
			break;
		case BuildingDef.BuildAreaChoosingType.FullCoverSoft:
			if (!flag3)
			{
				return;
			}
			break;
		}
		if (!flag2 && this.chooseBuildAreaType == BuildingDef.BuildAreaChoosingType.None)
		{
			return;
		}
		if (flag10)
		{
			this.state |= 6;
			return;
		}
		if (!flag5 || flag8)
		{
			this.state |= 2;
			return;
		}
		this.state |= 6;
		if (flag6)
		{
			this.state |= 16;
			return;
		}
		if (!flag9)
		{
			this.state |= 32;
		}
	}

	// Token: 0x040009BE RID: 2494
	private int state;

	// Token: 0x040009BF RID: 2495
	private Vector3 coords;

	// Token: 0x040009C0 RID: 2496
	[CanBeNull]
	private BuildingDef buildingDef;

	// Token: 0x040009C1 RID: 2497
	private BuildingDef.BuildAreaChoosingType chooseBuildAreaType;

	// Token: 0x040009C2 RID: 2498
	private BuildArea buildAreaWithCovering;

	// Token: 0x040009C3 RID: 2499
	private bool hasCustomBuildAreaId;

	// Token: 0x040009C4 RID: 2500
	private static Collider[] buildAreaColliders = new Collider[30];

	// Token: 0x040009C5 RID: 2501
	private HashSet<SGuid> extensionList;

	// Token: 0x040009C6 RID: 2502
	private bool containsExtensionParent;

	// Token: 0x040009C7 RID: 2503
	private ModuleSlotArea slotArea;

	// Token: 0x02000147 RID: 327
	public enum CellState
	{
		// Token: 0x040009C9 RID: 2505
		Empty = 1,
		// Token: 0x040009CA RID: 2506
		Available,
		// Token: 0x040009CB RID: 2507
		Busy = 4,
		// Token: 0x040009CC RID: 2508
		Buff = 8,
		// Token: 0x040009CD RID: 2509
		RemovableWgo = 16,
		// Token: 0x040009CE RID: 2510
		UnremovableWgo = 32,
		// Token: 0x040009CF RID: 2511
		BuffUsage = 64
	}

	// Token: 0x02000148 RID: 328
	public enum BuildMode
	{
		// Token: 0x040009D1 RID: 2513
		Place,
		// Token: 0x040009D2 RID: 2514
		Remove
	}
}
