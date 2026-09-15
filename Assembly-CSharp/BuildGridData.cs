using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class BuildGridData : MonoBehaviour
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00025EDE File Offset: 0x000240DE
	public BuildCellData[,] GridData
	{
		get
		{
			return this.gridData;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00025EE6 File Offset: 0x000240E6
	public BuildCellSelectionData[,] SelectionGridData
	{
		get
		{
			return this.selectionGridData;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00025EEE File Offset: 0x000240EE
	public BuildCellBuffUsageData[,] BuffUsageGridData
	{
		get
		{
			return this.buffUsageGridData;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00025EF6 File Offset: 0x000240F6
	public BuildCellData.BuildMode BuildMode
	{
		get
		{
			return this.buildMode;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00025EFE File Offset: 0x000240FE
	public BuildingDef CurrentBuildingDef
	{
		get
		{
			return this.currentBuildingDef;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060007C5 RID: 1989 RVA: 0x00025F06 File Offset: 0x00024106
	public bool DrawExtensions
	{
		get
		{
			return this.drawExtensions;
		}
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x00025F10 File Offset: 0x00024110
	public void FormGridData(Vector3 buildPos, string worldZoneId, Rect worldZoneRect, BuildingDef buildingDef = null, bool useExtensions = true)
	{
		this.currentBuildingDef = buildingDef;
		int num = 200;
		BuildCellData[,] array = new BuildCellData[num, num];
		this.selectionGridData = new BuildCellSelectionData[num, num];
		this.buffUsageGridData = new BuildCellBuffUsageData[num, num];
		Vector3 vector = new Vector3(BuildConsts.CELL_SIZE.x, 0f, BuildConsts.CELL_SIZE.y);
		this.drawExtensions = useExtensions;
		for (int i = -100; i < 100; i++)
		{
			for (int j = -100; j < 100; j++)
			{
				Vector3 vector2 = buildPos + Vector3.Scale(new Vector3((float)i, 0f, (float)j), vector) + vector / 2f;
				if (worldZoneRect.Contains(new Vector2(vector2.x, vector2.z)))
				{
					array[i + 100, j + 100] = BuildCellData.GetData(vector2, worldZoneId, buildingDef);
				}
			}
		}
		this.gridData = array;
		if (buildingDef != null)
		{
			BuildCellData.BuildMode buildMode;
			if (buildingDef.buildingMode == BuildingDef.BuildingMode.Remove)
			{
				buildMode = BuildCellData.BuildMode.Remove;
			}
			else
			{
				buildMode = BuildCellData.BuildMode.Place;
			}
			this.buildMode = buildMode;
		}
		else
		{
			this.buildMode = BuildCellData.BuildMode.Remove;
		}
		this.UpdateModuleSlotAreas();
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00026038 File Offset: 0x00024238
	public void FormSelectionGridData(Rect selectionRect)
	{
		if (this.gridData == null)
		{
			this.selectionGridData = null;
			return;
		}
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		this.selectionGridData = new BuildCellSelectionData[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				Vector3 coords = this.gridData[i, j].Coords;
				int num = (selectionRect.Contains(new Vector2(coords.x, coords.z)) ? 1 : 0);
				this.selectionGridData[i, j] = new BuildCellSelectionData(coords, num);
			}
		}
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x000260E0 File Offset: 0x000242E0
	public void FormSelectionGridData(List<Rect> selectionRects)
	{
		if (this.gridData == null)
		{
			this.selectionGridData = null;
			return;
		}
		if (selectionRects == null)
		{
			this.ClearSelectionGridData();
			return;
		}
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		this.selectionGridData = new BuildCellSelectionData[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				Vector3 coords = this.gridData[i, j].Coords;
				Vector2 vector = new Vector2(coords.x, coords.z);
				int num = 0;
				foreach (Rect rect in selectionRects)
				{
					if (rect.Contains(vector))
					{
						num = 1;
						break;
					}
				}
				this.selectionGridData[i, j] = new BuildCellSelectionData(coords, num);
			}
		}
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x000261E0 File Offset: 0x000243E0
	public void FormBuffUsageGridData(List<Rect> buffUsageRects)
	{
		if (this.gridData == null)
		{
			this.buffUsageGridData = null;
			return;
		}
		if (buffUsageRects == null)
		{
			this.ClearBuffUsageGridData();
			return;
		}
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		this.buffUsageGridData = new BuildCellBuffUsageData[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				Vector3 coords = this.gridData[i, j].Coords;
				Vector2 vector = new Vector2(coords.x, coords.z);
				int num = 0;
				foreach (Rect rect in buffUsageRects)
				{
					if (rect.Contains(vector))
					{
						num = 2;
						break;
					}
				}
				this.buffUsageGridData[i, j] = new BuildCellBuffUsageData(coords, num);
			}
		}
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000262E0 File Offset: 0x000244E0
	public void FormBuffUsageGridData(Rect buffUsageRect)
	{
		if (this.gridData == null)
		{
			this.buffUsageGridData = null;
			return;
		}
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		this.buffUsageGridData = new BuildCellBuffUsageData[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				Vector3 coords = this.gridData[i, j].Coords;
				int num = (buffUsageRect.Contains(new Vector2(coords.x, coords.z)) ? 2 : 0);
				this.buffUsageGridData[i, j] = new BuildCellBuffUsageData(coords, num);
			}
		}
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x00026388 File Offset: 0x00024588
	public void UpdateData()
	{
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				this.gridData[i, j].UpdateData();
			}
		}
		this.UpdateModuleSlotAreas();
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000263DF File Offset: 0x000245DF
	public void EraseData()
	{
		this.gridData = new BuildCellData[0, 0];
		this.selectionGridData = new BuildCellSelectionData[0, 0];
		this.buffUsageGridData = new BuildCellBuffUsageData[0, 0];
		this.busySlotAreas.Clear();
		this.freeSlotAreas.Clear();
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00026420 File Offset: 0x00024620
	private void UpdateModuleSlotAreas()
	{
		this.busySlotAreas.Clear();
		this.freeSlotAreas.Clear();
		if (this.gridData == null)
		{
			return;
		}
		BuildingDef buildingDef = this.currentBuildingDef;
		string text = ((buildingDef != null) ? buildingDef.customBuildAreaId : null);
		HashSet<ModuleSlotArea> hashSet = new HashSet<ModuleSlotArea>();
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				BuildCellData buildCellData = this.gridData[i, j];
				ModuleSlotArea slotArea = buildCellData.SlotArea;
				if (!(slotArea == null))
				{
					hashSet.Add(slotArea);
					if ((buildCellData.State & 4) != 0)
					{
						this.busySlotAreas.Add(slotArea);
					}
				}
			}
		}
		foreach (ModuleSlotArea moduleSlotArea in hashSet)
		{
			bool flag = !this.busySlotAreas.Contains(moduleSlotArea);
			if (moduleSlotArea.BuildArea != null && !string.IsNullOrEmpty(text))
			{
				flag &= moduleSlotArea.BuildArea.Id == text;
			}
			if (flag)
			{
				this.freeSlotAreas.Add(moduleSlotArea);
			}
			moduleSlotArea.ApplyVisibility(flag);
		}
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00026580 File Offset: 0x00024780
	private void ClearSelectionGridData()
	{
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		this.selectionGridData = new BuildCellSelectionData[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				Vector3 coords = this.gridData[i, j].Coords;
				this.selectionGridData[i, j] = new BuildCellSelectionData(coords, 0);
			}
		}
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x000265F8 File Offset: 0x000247F8
	private void ClearBuffUsageGridData()
	{
		int length = this.gridData.GetLength(0);
		int length2 = this.gridData.GetLength(1);
		this.buffUsageGridData = new BuildCellBuffUsageData[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				Vector3 coords = this.gridData[i, j].Coords;
				this.buffUsageGridData[i, j] = new BuildCellBuffUsageData(coords, 0);
			}
		}
	}

	// Token: 0x040009B5 RID: 2485
	private const int HALF_RANGE_SCAN_GRIDS_COUNT = 100;

	// Token: 0x040009B6 RID: 2486
	private BuildCellData[,] gridData;

	// Token: 0x040009B7 RID: 2487
	private BuildCellSelectionData[,] selectionGridData;

	// Token: 0x040009B8 RID: 2488
	private BuildCellBuffUsageData[,] buffUsageGridData;

	// Token: 0x040009B9 RID: 2489
	private BuildCellData.BuildMode buildMode;

	// Token: 0x040009BA RID: 2490
	private BuildingDef currentBuildingDef;

	// Token: 0x040009BB RID: 2491
	private bool drawExtensions;

	// Token: 0x040009BC RID: 2492
	private HashSet<ModuleSlotArea> busySlotAreas = new HashSet<ModuleSlotArea>();

	// Token: 0x040009BD RID: 2493
	private HashSet<ModuleSlotArea> freeSlotAreas = new HashSet<ModuleSlotArea>();
}
