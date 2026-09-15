using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class BuildLayout : MonoBehaviour
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x060007DD RID: 2013 RVA: 0x00026C33 File Offset: 0x00024E33
	public BuildGrid3D BuildGrid3D
	{
		get
		{
			return this.buildGrid3D;
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00026C3C File Offset: 0x00024E3C
	public void EnableBuildingMode(Vector3 position, string worldZoneId, Rect worldZoneXZRect, BuildingDef buildingDef = null, bool useExtensions = false, List<BuildElevationArea> elevationAreas = null)
	{
		base.transform.position = position + Vector3.up * 0.001f;
		this.worldZoneId = worldZoneId;
		this.buildGridData.FormGridData(position, worldZoneId, worldZoneXZRect, buildingDef, useExtensions);
		this.buildGrid3D.SetElevationAreas(elevationAreas);
		this.buildGrid3D.Draw(this.buildGridData.GridData, this.buildGridData.SelectionGridData, this.buildGridData.BuffUsageGridData, this.buildGridData.BuildMode, this.buildGridData.DrawExtensions);
		base.gameObject.SetActive(true);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00026CE0 File Offset: 0x00024EE0
	public void UpdateBuildingMode()
	{
		this.buildGridData.UpdateData();
		this.buildGrid3D.Draw(this.buildGridData.GridData, this.buildGridData.SelectionGridData, this.buildGridData.BuffUsageGridData, this.buildGridData.BuildMode, this.buildGridData.DrawExtensions);
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00026D3A File Offset: 0x00024F3A
	public void UpdateSelection(List<Rect> selectionRects)
	{
		if (this.buildGridData.GridData == null)
		{
			return;
		}
		this.buildGridData.FormSelectionGridData(selectionRects);
		this.buildGrid3D.UpdateSelection(this.buildGridData.SelectionGridData, this.buildGridData.BuffUsageGridData);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00026D78 File Offset: 0x00024F78
	public void UpdateSelection(List<Rect> selectionRects, List<Rect> buffUsageRects)
	{
		if (this.buildGridData.GridData == null)
		{
			return;
		}
		this.buildGridData.FormSelectionGridData(selectionRects);
		this.buildGridData.FormBuffUsageGridData(buffUsageRects);
		this.buildGrid3D.UpdateSelection(this.buildGridData.SelectionGridData, this.buildGridData.BuffUsageGridData);
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x00026DCC File Offset: 0x00024FCC
	public void DisableBuildingMode()
	{
		base.gameObject.SetActive(false);
		this.buildGrid3D.Clear();
		this.buildGridData.EraseData();
	}

	// Token: 0x040009D7 RID: 2519
	private const float Y_OFFSET = 0.001f;

	// Token: 0x040009D8 RID: 2520
	[SerializeField]
	private BuildGridData buildGridData;

	// Token: 0x040009D9 RID: 2521
	[SerializeField]
	private BuildGrid3D buildGrid3D;

	// Token: 0x040009DA RID: 2522
	private string worldZoneId;
}
