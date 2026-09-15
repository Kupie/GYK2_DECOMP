using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000158 RID: 344
public abstract class BuildPointerObject : MonoBehaviour, IBuildPointerObject
{
	// Token: 0x1700014F RID: 335
	// (get) Token: 0x0600082F RID: 2095 RVA: 0x0002822C File Offset: 0x0002642C
	protected HashSet<PreSetModuleBuildView> PreSetModuleBuildViews
	{
		get
		{
			return LazySingleton<BuildManager>.Instance.BuildController.BuildLayout.BuildGrid3D.PreSetModuleBuildViews;
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000830 RID: 2096 RVA: 0x00028247 File Offset: 0x00026447
	protected Bounds WorldRoundedBounds
	{
		get
		{
			return new Bounds(this.roundedBounds.center + base.transform.position, this.roundedBounds.size);
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000831 RID: 2097 RVA: 0x00028274 File Offset: 0x00026474
	public BuildData BuildData
	{
		get
		{
			return this.buildData;
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0002827C File Offset: 0x0002647C
	public Bounds GetWorldRoundedBounds()
	{
		return this.WorldRoundedBounds;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00028284 File Offset: 0x00026484
	public void Init(BuildData buildData, string worldZoneId)
	{
		this.buildData = buildData;
		this.worldZoneId = worldZoneId;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool HasRotation()
	{
		return false;
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Rotate()
	{
	}

	// Token: 0x06000836 RID: 2102
	public abstract bool TryDoBuildAction();

	// Token: 0x06000837 RID: 2103
	public abstract void SetupSelectionCells(BuildSelectionCell[] prefabCells, Transform parent);

	// Token: 0x06000838 RID: 2104 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void ApplySelectionCellsVisuals()
	{
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00028297 File Offset: 0x00026497
	public virtual void UpdateSelectionCellsState()
	{
		this.UpdateCellStatus();
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void ShowHints()
	{
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void UpdatePosition(Vector3 position)
	{
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x000282A0 File Offset: 0x000264A0
	public virtual void ClearSelectionCells()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			global::UnityEngine.Object.Destroy(this.cells[i].gameObject);
		}
		this.cells.Clear();
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x000282E4 File Offset: 0x000264E4
	public Vector3 GetObjectCenterLocal()
	{
		Bounds bounds = default(Bounds);
		foreach (BuildSelectionCell buildSelectionCell in this.cells)
		{
			bounds.Encapsulate(buildSelectionCell.SpriteBounds);
		}
		return base.transform.InverseTransformDirection(bounds.center);
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00028358 File Offset: 0x00026558
	public virtual Vector3 GetCellsCenterLocal()
	{
		if (this.cells == null || this.cells.Count == 0)
		{
			return Vector3.zero;
		}
		Bounds bounds = new Bounds(this.cells[0].transform.localPosition, Vector3.zero);
		for (int i = 1; i < this.cells.Count; i++)
		{
			BuildSelectionCell buildSelectionCell = this.cells[i];
			if (!(buildSelectionCell == null))
			{
				bounds.Encapsulate(buildSelectionCell.transform.localPosition);
			}
		}
		return bounds.center;
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnPointerDisable()
	{
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x000283E8 File Offset: 0x000265E8
	public virtual void SetVisibleSelectionCells(bool isVisible)
	{
		if (!isVisible)
		{
			this.previouslyActiveCells.Clear();
		}
		foreach (BuildSelectionCell buildSelectionCell in this.cells)
		{
			if (!(buildSelectionCell is BuffCell))
			{
				if (!isVisible && buildSelectionCell.gameObject.activeSelf)
				{
					this.previouslyActiveCells.Add(buildSelectionCell);
				}
				if (!isVisible || this.previouslyActiveCells.Contains(buildSelectionCell))
				{
					buildSelectionCell.gameObject.SetActive(isVisible);
				}
			}
		}
		if (isVisible)
		{
			this.previouslyActiveCells.Clear();
		}
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00028494 File Offset: 0x00026694
	protected void UpdateCellStatus()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			this.cells[i].IsAvailableForBuild = this.shownAsActive;
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void UpdateCollider()
	{
	}

	// Token: 0x04000A1D RID: 2589
	protected const float CELL_Y_LOCAL_OFFSET = 0.01f;

	// Token: 0x04000A1E RID: 2590
	protected BuildData buildData;

	// Token: 0x04000A1F RID: 2591
	protected List<BuildSelectionCell> cells = new List<BuildSelectionCell>();

	// Token: 0x04000A20 RID: 2592
	protected BuildSelectionCell[,] cellsGrid;

	// Token: 0x04000A21 RID: 2593
	protected byte[,] cellsGridMask;

	// Token: 0x04000A22 RID: 2594
	protected bool shownAsActive;

	// Token: 0x04000A23 RID: 2595
	protected string worldZoneId;

	// Token: 0x04000A24 RID: 2596
	protected Bounds roundedBounds;

	// Token: 0x04000A25 RID: 2597
	private HashSet<BuildSelectionCell> previouslyActiveCells = new HashSet<BuildSelectionCell>();
}
