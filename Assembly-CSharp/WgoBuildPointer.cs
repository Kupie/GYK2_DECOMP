using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000160 RID: 352
public class WgoBuildPointer : BuildPointerObject
{
	// Token: 0x17000152 RID: 338
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x0002A2A0 File Offset: 0x000284A0
	public Vector3 PreviewOffset
	{
		get
		{
			return this.offset;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600087B RID: 2171 RVA: 0x0002A2A8 File Offset: 0x000284A8
	// (set) Token: 0x0600087C RID: 2172 RVA: 0x0002A2B0 File Offset: 0x000284B0
	public bool DrawBuffAreas { get; set; } = true;

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x0600087D RID: 2173 RVA: 0x0002A2B9 File Offset: 0x000284B9
	public Wgo Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0002A2C4 File Offset: 0x000284C4
	public void SetTarget(Wgo target, GameScene gameScene, BuildingDef buildingDef, List<NeedItemData> itemNeeds = null, MultiInventory multiInventory = null)
	{
		this.target = target;
		this.target.transform.SetParent(base.transform);
		this.gameScene = gameScene;
		this.buildingDef = buildingDef;
		this.hasCustomBuildArea = !string.IsNullOrEmpty(buildingDef.customBuildAreaId);
		this.canTakeResources = delegate
		{
			if (multiInventory == null || itemNeeds == null || multiInventory.HasItemsById(itemNeeds, 1, null))
			{
				BuildingDef buildingDef2 = buildingDef;
				return buildingDef2 == null || !buildingDef2.HasLimits || buildingDef.limitMax > buildingDef.currentLimitExpression.EvaluateInt();
			}
			return false;
		};
		this.takeResourcesAction = delegate
		{
			if (multiInventory != null && itemNeeds != null)
			{
				multiInventory.RemoveItems(itemNeeds, 1, null);
			}
		};
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0002A35B File Offset: 0x0002855B
	public override bool HasRotation()
	{
		return this.target.CanBeRotated();
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0002A368 File Offset: 0x00028568
	public override void Rotate()
	{
		this.target.MainWgoPart.Rotate(false);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0002A37C File Offset: 0x0002857C
	public override bool TryDoBuildAction()
	{
		if (this.shownAsActive)
		{
			Action action = this.takeResourcesAction;
			if (action != null)
			{
				action();
			}
			WgoData wgoData = new WgoData(this.buildData.WgoId, this.target.Data.Position, this.gameScene.Id);
			if (this.target.CanBeRotated() && this.target.MainWgoPart.WgoPartData.rotationIndex != -1)
			{
				wgoData.MainWgoPartData.variationId = this.target.MainWgoPart.WgoPartData.variationId;
				wgoData.MainWgoPartData.rotationIndex = this.target.MainWgoPart.WgoPartData.rotationIndex;
			}
			Wgo wgo = this.gameScene.AddWgoData(wgoData, false);
			if (this.buildData.Definition != null)
			{
				foreach (LazyExpression lazyExpression in this.buildData.Definition.expressionAfterBuilding)
				{
					lazyExpression.EvaluateBool(wgoData);
				}
			}
			this.TrySetCustomRotation(wgo);
			if (this.buildingDef.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft && BuildController.Instance.CurrentFullCoverSoftHintArea != null)
			{
				Wgo componentInParent = BuildController.Instance.CurrentFullCoverSoftHintArea.GetComponentInParent<Wgo>();
				if (componentInParent != null)
				{
					componentInParent.Data.AddWorkbenchExtension(wgo.Data.UniqueId);
					wgo.Data.AddWorkbenchParent(componentInParent.Data.UniqueId);
				}
			}
			this.UpdateUnbuffableObjectsTint();
			this.UpdateSelectionStuff();
			return true;
		}
		return false;
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x0002A51C File Offset: 0x0002871C
	public override void SetupSelectionCells(BuildSelectionCell[] prefabCells, Transform parent)
	{
		this.overlapMask = 537526528;
		if (this.buildData.Definition.id != "graveyard_module_p")
		{
			this.overlapMask |= 65536;
		}
		this.target.transform.localPosition = Vector3.zero;
		Physics.SyncTransforms();
		Collider[] componentsInChildren = this.target.GetComponentsInChildren<Collider>();
		Bounds bounds = default(Bounds);
		bool flag = false;
		Collider[] array = new Collider[20];
		List<Collider> list = new List<Collider>();
		foreach (Collider collider in componentsInChildren)
		{
			int j = collider.gameObject.layer;
			PlacementBlockingArea placementBlockingArea;
			if ((j == 19 || j == 28) && !PlacementBlockingArea.TryGet(collider, out placementBlockingArea))
			{
				BuildArea buildArea;
				if (collider.TryGetComponent<BuildArea>(out buildArea))
				{
					if (buildArea.foprceShowAsBuffAreaForPointerPlacement)
					{
						list.Add(buildArea.Collider);
						goto IL_011F;
					}
					if (buildArea.ignoreForPointerPlacement)
					{
						goto IL_011F;
					}
				}
				if (collider.gameObject.layer != 28 || this.buildingDef.chooseCustomBuildAreaType != BuildingDef.BuildAreaChoosingType.FullCoverSoft)
				{
					Bounds bounds2 = collider.bounds;
					if (!flag)
					{
						flag = true;
						bounds = bounds2;
					}
					else
					{
						bounds.Encapsulate(bounds2);
					}
					this.buildColliders.Add(collider);
				}
			}
			IL_011F:;
		}
		Bounds bounds3 = bounds;
		foreach (Collider collider2 in list)
		{
			if (!this.buildColliders.Contains(collider2))
			{
				this.buildColliders.Add(collider2);
				bounds3.Encapsulate(collider2.bounds);
			}
		}
		Vector3 min = bounds.min;
		this.offset = VisualConsts.GetRoundedPosXZ(min, BuildConsts.BUILD_GRID_SIZE) - min;
		this.offset.y = 0f;
		this.target.transform.position += this.offset;
		if (!this.offset.magnitude.EqualsTo(0f, 1E-05f))
		{
			Physics.SyncTransforms();
		}
		Bounds bounds4 = new Bounds(bounds.center + this.offset, bounds.size);
		this.roundedBounds = VisualConsts.GetGreaterRoundedBoundsXZ(bounds4, BuildConsts.BUILD_GRID_SIZE);
		Bounds greaterRoundedBoundsXZ = VisualConsts.GetGreaterRoundedBoundsXZ(new Bounds(bounds3.center + this.offset, bounds3.size), BuildConsts.BUILD_GRID_SIZE);
		Vector2Int vector2Int = 2 * new Vector2Int(16, 15);
		int num = Mathf.CeilToInt(greaterRoundedBoundsXZ.size.x / 0.01f / (float)vector2Int.x);
		int num2 = Mathf.CeilToInt(greaterRoundedBoundsXZ.size.z / 0.01f / (float)vector2Int.y);
		this.cellsGrid = new BuildSelectionCell[num, num2];
		this.cellsGridMask = new byte[num, num2];
		Vector3 vector = new Vector3(BuildConsts.CELL_SIZE.x, 0f, BuildConsts.CELL_SIZE.y);
		Vector3 vector2 = new Vector3(greaterRoundedBoundsXZ.min.x, 0.01f, greaterRoundedBoundsXZ.min.z) + vector / 2f;
		BuildSelectionCell buildSelectionCell = prefabCells[0];
		BuildSelectionCell buildSelectionCell2 = prefabCells[1];
		for (int k = 0; k < num; k++)
		{
			for (int l = 0; l < num2; l++)
			{
				Vector3 vector3 = vector2 + Vector3.Scale(vector, new Vector3((float)k, 0f, (float)l));
				int num3 = Physics.OverlapBoxNonAlloc(vector3 + parent.transform.position, BuildConsts.CASTING_BOX_HALF_EXTENTS, array, Quaternion.identity, 268959744);
				Debug.DrawRay(vector3, Vector3.up, Color.blue, 10f);
				for (int m = 0; m < num3; m++)
				{
					Collider collider3 = array[m];
					if (this.buildColliders.Contains(collider3))
					{
						BuildSelectionCell buildSelectionCell3 = null;
						if (collider3.gameObject.layer == 19)
						{
							if (list.Contains(collider3))
							{
								buildSelectionCell3 = buildSelectionCell2.Copy(parent, true, "");
							}
							else
							{
								buildSelectionCell3 = buildSelectionCell.Copy(parent, true, "");
							}
						}
						else if (collider3.gameObject.layer == 28)
						{
							buildSelectionCell3 = buildSelectionCell2.Copy(parent, true, "");
						}
						if (buildSelectionCell3)
						{
							buildSelectionCell3.transform.localPosition = vector3;
							this.cells.Add(buildSelectionCell3);
							BuffCell buffCell = buildSelectionCell3 as BuffCell;
							if (buffCell != null)
							{
								this.buffCells.Add(buffCell);
							}
							byte b = ((buildSelectionCell3 is BuffCell) ? 2 : 1);
							if (this.cellsGridMask[k, l] == 0 || this.cellsGridMask[k, l] > b)
							{
								if (this.cellsGrid[k, l] != null)
								{
									this.cellsGrid[k, l].gameObject.SetActive(false);
								}
								this.cellsGrid[k, l] = buildSelectionCell3;
								this.cellsGridMask[k, l] = b;
							}
							else
							{
								buildSelectionCell3.gameObject.SetActive(false);
							}
						}
					}
				}
			}
		}
		foreach (ModuleSlotArea moduleSlotArea in this.target.GetComponentsInChildren<ModuleSlotArea>())
		{
			this.moduleSlotAreas.Add(moduleSlotArea);
		}
		ColorUtility.TryParseHtmlString(this.unDestroyableSelectionTintColorHex, out this.unDestroyableSelectionTintColor);
		this.UpdateUnbuffableObjectsTint();
		this.DoBuffDrawingLogic();
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x0002AAAC File Offset: 0x00028CAC
	public override void ApplySelectionCellsVisuals()
	{
		if (this.cellsGrid == null || this.cellsGridMask == null)
		{
			return;
		}
		int length = this.cellsGridMask.GetLength(0);
		int length2 = this.cellsGridMask.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				if (this.cellsGridMask[i, j] != 0)
				{
					BuildSelectionCell buildSelectionCell = this.cellsGrid[i, j];
					if (!(buildSelectionCell == null))
					{
						byte b = this.cellsGridMask[i, j];
						bool flag = i > 0 && this.cellsGridMask[i - 1, j] > 0 && this.cellsGridMask[i - 1, j] <= b;
						bool flag2 = i + 1 < length && this.cellsGridMask[i + 1, j] > 0 && this.cellsGridMask[i + 1, j] <= b;
						bool flag3 = j + 1 < length2 && this.cellsGridMask[i, j + 1] > 0 && this.cellsGridMask[i, j + 1] <= b;
						bool flag4 = j > 0 && this.cellsGridMask[i, j - 1] > 0 && this.cellsGridMask[i, j - 1] <= b;
						bool flag5 = i > 0 && j + 1 < length2 && this.cellsGridMask[i - 1, j + 1] > 0 && this.cellsGridMask[i - 1, j + 1] <= b;
						bool flag6 = i + 1 < length && j + 1 < length2 && this.cellsGridMask[i + 1, j + 1] > 0 && this.cellsGridMask[i + 1, j + 1] <= b;
						bool flag7 = i > 0 && j > 0 && this.cellsGridMask[i - 1, j - 1] > 0 && this.cellsGridMask[i - 1, j - 1] <= b;
						bool flag8 = i + 1 < length && j > 0 && this.cellsGridMask[i + 1, j - 1] > 0 && this.cellsGridMask[i + 1, j - 1] <= b;
						BuildSelectionCellVariation buildSelectionCellVariation;
						if (flag && flag3 && !flag5)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerInsideTopLeft;
						}
						else if (flag2 && flag3 && !flag6)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerInsideTopRight;
						}
						else if (flag && flag4 && !flag7)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerInsideBottomLeft;
						}
						else if (flag2 && flag4 && !flag8)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerInsideBottomRight;
						}
						else if (!flag && !flag3)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerTopLeft;
						}
						else if (!flag2 && !flag3)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerTopRight;
						}
						else if (!flag && !flag4)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerBottomLeft;
						}
						else if (!flag2 && !flag4)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.CornerBottomRight;
						}
						else if (!flag)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.EdgeLeft;
						}
						else if (!flag2)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.EdgeRight;
						}
						else if (!flag3)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.EdgeTop;
						}
						else if (!flag4)
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.EdgeBottom;
						}
						else
						{
							buildSelectionCellVariation = BuildSelectionCellVariation.Central;
						}
						buildSelectionCell.SetVariation(buildSelectionCellVariation);
					}
				}
			}
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x0002ADA4 File Offset: 0x00028FA4
	public override void UpdateSelectionCellsState()
	{
		bool flag = !this.fullCoverSoftQueryReusableForAvailability;
		if (flag)
		{
			this.InvalidateFullCoverSoftQueryCache();
		}
		this.fullCoverSoftQueryReusableForAvailability = false;
		Collider[] array = new Collider[20];
		Func<bool> func = this.canTakeResources;
		this.shownAsActive = func == null || func();
		bool flag2 = this.buildingDef.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft;
		List<string> list = null;
		if (flag2 && this.hasCustomBuildArea && GameBalance.Me != null)
		{
			GameBalance.Me.customBuildAreaIdToWgoIds.TryGetValue(this.buildingDef.customBuildAreaId, out list);
		}
		int num = 0;
		while (num < this.cells.Count && this.shownAsActive)
		{
			BuildSelectionCell buildSelectionCell = this.cells[num];
			if (!(buildSelectionCell is BuffCell))
			{
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				bool flag6 = false;
				int num2 = buildSelectionCell.OverlapBoxNonAlloc(array, this.overlapMask);
				bool flag7 = true;
				for (int i = 0; i < num2; i++)
				{
					Collider collider = array[i];
					if (!(collider == null))
					{
						if (collider.gameObject.layer == 29)
						{
							flag7 = false;
							break;
						}
						WorldZone worldZone;
						if (collider.TryGetComponent<WorldZone>(out worldZone))
						{
							if (!flag3)
							{
								flag3 = worldZone.Data.Definition.id == this.worldZoneId;
							}
						}
						else if (!this.buildColliders.Contains(collider))
						{
							PlacementBlockingArea placementBlockingArea;
							BuildArea buildArea;
							if (PlacementBlockingArea.TryGet(collider, out placementBlockingArea))
							{
								if (PlacementBlockingArea.IsBlockingFor(collider, this.buildingDef, this.target))
								{
									flag7 = false;
									break;
								}
							}
							else if (collider.TryGetComponent<BuildArea>(out buildArea))
							{
								if (!buildArea.foprceShowAsBuffAreaForPointerPlacement)
								{
									flag4 = true;
									bool flag8 = false;
									switch (this.buildingDef.chooseCustomBuildAreaType)
									{
									case BuildingDef.BuildAreaChoosingType.Soft:
									case BuildingDef.BuildAreaChoosingType.FullCoverWithCount:
										if (this.hasCustomBuildArea && !flag5 && buildArea.Id == this.buildingDef.customBuildAreaId)
										{
											flag5 = true;
											goto IL_0338;
										}
										goto IL_0338;
									case BuildingDef.BuildAreaChoosingType.Strict:
										flag6 = buildArea.Id == this.buildingDef.customBuildAreaId;
										if (flag6)
										{
											goto IL_0338;
										}
										flag8 = true;
										break;
									}
									if (flag8)
									{
										break;
									}
								}
							}
							else if (!flag2)
							{
								int layer = collider.gameObject.layer;
								if (layer == 8 || layer == 19 || layer == 16)
								{
									Wgo componentInParent = collider.GetComponentInParent<Wgo>();
									BuildingDef.BuildAreaChoosingType chooseCustomBuildAreaType = this.buildingDef.chooseCustomBuildAreaType;
									if (chooseCustomBuildAreaType > BuildingDef.BuildAreaChoosingType.FullCoverWithCount)
									{
										if (chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft)
										{
											if (flag2 && list != null && list.Count > 0 && (componentInParent == null || componentInParent.Data == null || componentInParent == this.target || componentInParent.Data.isTempObject || !list.Contains(componentInParent.Data.id)))
											{
												goto IL_0338;
											}
										}
									}
									else
									{
										if (componentInParent == this.target || (componentInParent != null && componentInParent.Data != null && componentInParent.Data.isTempObject))
										{
											goto IL_0338;
										}
										if (this.buildData.Definition != null)
										{
											Wgo componentInParent2 = collider.GetComponentInParent<Wgo>();
											if (componentInParent2 != null && this.buildData.Definition.ShouldIgnoreWgoGroupAsObstacle(componentInParent2.Data.Definition.wgoGroup))
											{
												goto IL_0338;
											}
										}
									}
									flag7 = false;
									break;
								}
							}
						}
					}
					IL_0338:;
				}
				if (!flag3)
				{
					flag7 = false;
				}
				switch (this.buildingDef.chooseCustomBuildAreaType)
				{
				case BuildingDef.BuildAreaChoosingType.Soft:
				case BuildingDef.BuildAreaChoosingType.FullCoverWithCount:
					if (this.hasCustomBuildArea && !flag5)
					{
						flag7 = false;
					}
					if (!this.hasCustomBuildArea && !flag4)
					{
						flag7 = false;
					}
					break;
				case BuildingDef.BuildAreaChoosingType.Strict:
					if (!flag6)
					{
						flag7 = false;
					}
					break;
				case BuildingDef.BuildAreaChoosingType.FullCoverSoft:
					if (!flag4)
					{
						flag7 = false;
					}
					break;
				}
				this.shownAsActive = this.shownAsActive && flag7;
				if (!this.shownAsActive)
				{
					break;
				}
			}
			num++;
		}
		if (this.shownAsActive && this.buildingDef.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft && this.hasCustomBuildArea)
		{
			this.shownAsActive &= this.IsFullCoverSoftPlacementAllowed();
		}
		if (this.buildingDef.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverWithCount)
		{
			int num3 = 0;
			int fullCoveringCount = this.buildingDef.fullCoveringCount;
			using (HashSet<PreSetModuleBuildView>.Enumerator enumerator = base.PreSetModuleBuildViews.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsFullyInsideIn(base.WorldRoundedBounds))
					{
						num3++;
					}
				}
			}
			this.shownAsActive &= fullCoveringCount == num3;
		}
		for (int j = 0; j < this.cells.Count; j++)
		{
			BuildSelectionCell buildSelectionCell2 = this.cells[j];
			if (!(buildSelectionCell2 is BuffCell))
			{
				buildSelectionCell2.IsAvailableForBuild = this.shownAsActive;
			}
		}
		foreach (ModuleSlotArea moduleSlotArea in this.moduleSlotAreas)
		{
			moduleSlotArea.ApplyVisibility(this.shownAsActive);
		}
		if (flag)
		{
			this.UpdateSelectionStuff();
		}
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x0002B2D0 File Offset: 0x000294D0
	private static bool FullyContainsXZ(Bounds container, Bounds inner, float eps = 0.001f)
	{
		return inner.min.x >= container.min.x - eps && inner.max.x <= container.max.x + eps && inner.min.z >= container.min.z - eps && inner.max.z <= container.max.z + eps;
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0002B352 File Offset: 0x00029552
	private void InvalidateFullCoverSoftQueryCache()
	{
		this.fullCoverSoftQueryCached = false;
		this.cachedHasCoveredFullCoverSoftArea = false;
		this.cachedFullCoverSoftArea = null;
		this.cachedFullCoverSoftSlotFree = false;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x0002B370 File Offset: 0x00029570
	private void EnsureFullCoverSoftQueryCache()
	{
		if (this.fullCoverSoftQueryCached)
		{
			return;
		}
		this.fullCoverSoftQueryCached = true;
		this.cachedHasCoveredFullCoverSoftArea = this.TryGetCoveredFullCoverSoftBuildArea(out this.cachedFullCoverSoftArea);
		this.cachedFullCoverSoftSlotFree = this.cachedHasCoveredFullCoverSoftArea && this.IsFullCoverSoftSlotFree(this.cachedFullCoverSoftArea);
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x0002B3BC File Offset: 0x000295BC
	private bool TryGetCoveredFullCoverSoftBuildArea(out BuildArea coveredArea)
	{
		coveredArea = null;
		if (this.buildingDef == null || this.buildingDef.chooseCustomBuildAreaType != BuildingDef.BuildAreaChoosingType.FullCoverSoft || string.IsNullOrEmpty(this.buildingDef.customBuildAreaId))
		{
			return false;
		}
		Bounds worldRoundedBounds = base.WorldRoundedBounds;
		int num = Physics.OverlapBoxNonAlloc(worldRoundedBounds.center, worldRoundedBounds.extents, this.fullCoverSoftAreaOverlap, Quaternion.identity, 524288);
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.fullCoverSoftAreaOverlap[i];
			BuildArea buildArea;
			if (!(collider == null) && collider.TryGetComponent<BuildArea>(out buildArea) && !(buildArea.Id != this.buildingDef.customBuildAreaId) && WgoBuildPointer.FullyContainsXZ(worldRoundedBounds, collider.bounds, 0.001f))
			{
				coveredArea = buildArea;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0002B47C File Offset: 0x0002967C
	private void TryAddFullCoverSoftSlotHost(HashSet<WGODef> eligibleDefs, HashSet<Wgo> overlappingHosts, List<Rect> outSelectionRects)
	{
		if (overlappingHosts == null)
		{
			return;
		}
		this.EnsureFullCoverSoftQueryCache();
		if (!this.cachedHasCoveredFullCoverSoftArea || !this.cachedFullCoverSoftSlotFree)
		{
			return;
		}
		Wgo componentInParent = this.cachedFullCoverSoftArea.GetComponentInParent<Wgo>();
		if (!this.IsEligibleWorldIconHost(componentInParent))
		{
			return;
		}
		if (eligibleDefs == null || componentInParent.Data == null || !eligibleDefs.Contains(componentInParent.Data.Definition))
		{
			return;
		}
		overlappingHosts.Add(componentInParent);
		Rect buildAreaRect = this.GetBuildAreaRect(componentInParent);
		if (buildAreaRect != Rect.zero && !outSelectionRects.Contains(buildAreaRect))
		{
			outSelectionRects.Add(buildAreaRect);
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0002B506 File Offset: 0x00029706
	private bool IsFullCoverSoftPlacementAllowed()
	{
		this.EnsureFullCoverSoftQueryCache();
		return this.cachedHasCoveredFullCoverSoftArea && this.cachedFullCoverSoftSlotFree;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0002B520 File Offset: 0x00029720
	private bool IsFullCoverSoftSlotFree(BuildArea coveredArea)
	{
		if (coveredArea == null)
		{
			return false;
		}
		Collider collider = ((coveredArea.Collider != null) ? coveredArea.Collider : coveredArea.GetComponent<Collider>());
		if (collider == null)
		{
			return true;
		}
		Wgo componentInParent = coveredArea.GetComponentInParent<Wgo>();
		Bounds bounds = collider.bounds;
		Vector3 vector = Vector3.Max(Vector3.zero, bounds.extents - VisualConsts.XYZ_STEP);
		int num = Physics.OverlapBoxNonAlloc(bounds.center, vector, this.fullCoverSoftOccupantsOverlap, Quaternion.identity, 590080);
		for (int i = 0; i < num; i++)
		{
			Collider collider2 = this.fullCoverSoftOccupantsOverlap[i];
			BuildArea buildArea;
			ModuleSlotArea moduleSlotArea;
			if (!(collider2 == null) && !collider2.TryGetComponent<BuildArea>(out buildArea) && !collider2.TryGetComponent<ModuleSlotArea>(out moduleSlotArea))
			{
				PlacementBlockingArea placementBlockingArea;
				if (PlacementBlockingArea.TryGet(collider2, out placementBlockingArea))
				{
					if (PlacementBlockingArea.IsBlockingFor(collider2, this.buildData.Definition, this.target))
					{
						return false;
					}
				}
				else
				{
					Wgo componentInParent2 = collider2.GetComponentInParent<Wgo>();
					if (componentInParent2 != null)
					{
						if (!(componentInParent2 == this.target) && !(componentInParent2 == componentInParent) && componentInParent2.Data != null && !componentInParent2.Data.isTempObject && (this.buildData.Definition == null || !this.buildData.Definition.ShouldIgnoreWgoGroupAsObstacle(componentInParent2.Data.Definition.wgoGroup)))
						{
							return false;
						}
					}
					else
					{
						int layer = collider2.gameObject.layer;
						if (layer == 8 || layer == 16)
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0002B6AF File Offset: 0x000298AF
	public override void UpdatePosition(Vector3 position)
	{
		this.target.Data.Position = position + this.offset;
		this.UpdateSelectionStuff();
		this.fullCoverSoftQueryReusableForAvailability = true;
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0002B6DC File Offset: 0x000298DC
	public override void ShowHints()
	{
		BuildingHUDData buildingHUDData = new BuildingHUDData(this.target.DockPoints, this.HasRotation());
		LazyUI.Get<BuildingHUD>().Draw(buildingHUDData);
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0002B70C File Offset: 0x0002990C
	public override void ClearSelectionCells()
	{
		this.buildColliders.Clear();
		this.buffCells.Clear();
		this.buffUsageRects.Clear();
		this.selectionRects.Clear();
		this.InvalidateFullCoverSoftQueryCache();
		this.fullCoverSoftQueryReusableForAvailability = false;
		base.ClearSelectionCells();
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0002B758 File Offset: 0x00029958
	private void UpdateSelectionStuff()
	{
		BuildManager instance = LazySingleton<BuildManager>.Instance;
		BuildLayout buildLayout;
		if (instance == null)
		{
			buildLayout = null;
		}
		else
		{
			BuildController buildController = instance.BuildController;
			buildLayout = ((buildController != null) ? buildController.BuildLayout : null);
		}
		BuildLayout buildLayout2 = buildLayout;
		if (buildLayout2 == null)
		{
			return;
		}
		this.buffUsageRects.Clear();
		this.selectionRects.Clear();
		this.overlappingHostsBuffer.Clear();
		this.InvalidateFullCoverSoftQueryCache();
		if (GameBalance.Me == null)
		{
			buildLayout2.UpdateSelection(this.selectionRects, this.buffUsageRects);
			WorkbenchAdditionWorldIconPresenter.Clear();
			this.ResetAdditionWorldIconPublishState();
			return;
		}
		ValueTuple<string, WGODef> placingWgo = this.GetPlacingWgo();
		string item = placingWgo.Item1;
		WGODef item2 = placingWgo.Item2;
		ValueTuple<WgoBuildPointer.BuildPointerRole, HashSet<WGODef>> pointerRoleAndEligibleDefs = this.GetPointerRoleAndEligibleDefs(item, item2);
		WgoBuildPointer.BuildPointerRole item3 = pointerRoleAndEligibleDefs.Item1;
		HashSet<WGODef> item4 = pointerRoleAndEligibleDefs.Item2;
		if (item3 == WgoBuildPointer.BuildPointerRole.ExtensionParent)
		{
			this.FillSelectionRectsForParentOverlappingExtensions(item2, this.overlappingHostsBuffer, this.selectionRects);
			buildLayout2.UpdateSelection(this.selectionRects, this.buffUsageRects);
			this.UpdateAdditionWorldIcons(item3, item, this.overlappingHostsBuffer);
			return;
		}
		if (item3 == WgoBuildPointer.BuildPointerRole.Extension)
		{
			this.TryAddFullCoverSoftSlotHost(item4, this.overlappingHostsBuffer, this.selectionRects);
		}
		if (this.buffCells.Count == 0)
		{
			buildLayout2.UpdateSelection(this.selectionRects, this.buffUsageRects);
			this.UpdateAdditionWorldIcons(item3, item, this.overlappingHostsBuffer);
			return;
		}
		if (item4 == null || item4.Count == 0)
		{
			buildLayout2.UpdateSelection(this.selectionRects, this.buffUsageRects);
			this.UpdateAdditionWorldIcons(item3, item, this.overlappingHostsBuffer);
			return;
		}
		this.FillRectsFromBuffCells(item4, this.overlappingHostsBuffer, this.buffUsageRects);
		this.AddBuildAreaRects(this.selectionRects, this.overlappingHostsBuffer);
		if (item3 == WgoBuildPointer.BuildPointerRole.Extension)
		{
			this.AddAlreadyConnectedExtensionParents(item, item4, this.overlappingHostsBuffer, this.selectionRects);
		}
		buildLayout2.UpdateSelection(this.selectionRects, this.buffUsageRects);
		this.UpdateAdditionWorldIcons(item3, item, this.overlappingHostsBuffer);
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0002B914 File Offset: 0x00029B14
	[return: TupleElementNames(new string[] { "placingWgoId", "placingDef" })]
	private ValueTuple<string, WGODef> GetPlacingWgo()
	{
		BuildingDef buildingDef = this.buildingDef;
		string text;
		if ((text = ((buildingDef != null) ? buildingDef.wgoId : null)) == null)
		{
			Wgo wgo = this.target;
			if (wgo == null)
			{
				text = null;
			}
			else
			{
				WgoData data = wgo.Data;
				text = ((data != null) ? data.id : null);
			}
		}
		string text2 = text;
		return this.ResolveExtensionLogicWgo(text2);
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0002B960 File Offset: 0x00029B60
	[return: TupleElementNames(new string[] { "placingWgoId", "placingDef" })]
	private ValueTuple<string, WGODef> ResolveExtensionLogicWgo(string wgoId)
	{
		if (string.IsNullOrEmpty(wgoId) || GameBalance.Me == null)
		{
			return new ValueTuple<string, WGODef>(wgoId, null);
		}
		if (this.buildingDef != null && !string.IsNullOrEmpty(this.buildingDef.customWgoPlacePreview))
		{
			string customWgoPlacePreview = this.buildingDef.customWgoPlacePreview;
			WGODef workbenchExtensionLogicDef = GameBalance.Me.GetWorkbenchExtensionLogicDef(customWgoPlacePreview);
			if (workbenchExtensionLogicDef != null && (GameBalance.Me.workbenchesWhichUseExtensions.Contains(workbenchExtensionLogicDef) || GameBalance.Me.IsWorkbenchExtensionId(customWgoPlacePreview)))
			{
				return new ValueTuple<string, WGODef>(customWgoPlacePreview, workbenchExtensionLogicDef);
			}
		}
		WGODef workbenchExtensionLogicDef2 = GameBalance.Me.GetWorkbenchExtensionLogicDef(wgoId);
		if (workbenchExtensionLogicDef2 == null)
		{
			return new ValueTuple<string, WGODef>(wgoId, null);
		}
		if (GameBalance.Me.workbenchesWhichUseExtensions.Contains(workbenchExtensionLogicDef2) || GameBalance.Me.IsWorkbenchExtensionId(workbenchExtensionLogicDef2.id))
		{
			return new ValueTuple<string, WGODef>(workbenchExtensionLogicDef2.id, workbenchExtensionLogicDef2);
		}
		return new ValueTuple<string, WGODef>(wgoId, workbenchExtensionLogicDef2);
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0002BA38 File Offset: 0x00029C38
	[return: TupleElementNames(new string[] { "role", "eligibleDefs" })]
	private ValueTuple<WgoBuildPointer.BuildPointerRole, HashSet<WGODef>> GetPointerRoleAndEligibleDefs(string placingWgoId, WGODef placingDef)
	{
		HashSet<WGODef> hashSet = GameBalance.Me.workbenchesWhichUseExtensions;
		bool flag = !string.IsNullOrEmpty(placingWgoId) && GameBalance.Me.IsWorkbenchExtensionId(placingWgoId);
		if (placingDef != null && hashSet != null && hashSet.Contains(placingDef))
		{
			return new ValueTuple<WgoBuildPointer.BuildPointerRole, HashSet<WGODef>>(WgoBuildPointer.BuildPointerRole.ExtensionParent, hashSet);
		}
		if (flag)
		{
			List<WGODef> list;
			if (GameBalance.Me.TryGetParentWorkbenchDefsForExtension(placingWgoId, out list))
			{
				hashSet = this.GetExtensionParentDefs(placingWgoId, list);
			}
			return new ValueTuple<WgoBuildPointer.BuildPointerRole, HashSet<WGODef>>(WgoBuildPointer.BuildPointerRole.Extension, hashSet);
		}
		return new ValueTuple<WgoBuildPointer.BuildPointerRole, HashSet<WGODef>>(WgoBuildPointer.BuildPointerRole.None, hashSet);
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0002BAAC File Offset: 0x00029CAC
	private HashSet<WGODef> GetExtensionParentDefs(string placingWgoId, List<WGODef> parents)
	{
		if (this.extensionParentDefsBufferForId == placingWgoId)
		{
			return this.extensionParentDefsBuffer;
		}
		this.extensionParentDefsBufferForId = placingWgoId;
		this.extensionParentDefsBuffer.Clear();
		if (parents != null)
		{
			for (int i = 0; i < parents.Count; i++)
			{
				if (parents[i] != null)
				{
					this.extensionParentDefsBuffer.Add(parents[i]);
				}
			}
		}
		return this.extensionParentDefsBuffer;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0002BB16 File Offset: 0x00029D16
	private HashSet<string> GetCachedAllowedExtensionIds(string parentWorkbenchId)
	{
		if (this.cachedAllowedExtensionIdsForParentId == parentWorkbenchId && this.cachedAllowedExtensionIds != null)
		{
			return this.cachedAllowedExtensionIds;
		}
		this.cachedAllowedExtensionIdsForParentId = parentWorkbenchId;
		this.cachedAllowedExtensionIds = GameBalance.Me.GetAllowedExtensionIdsForParentWorkbench(parentWorkbenchId);
		return this.cachedAllowedExtensionIds;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x0002BB54 File Offset: 0x00029D54
	private void FillSelectionRectsForParentOverlappingExtensions(WGODef parentDef, HashSet<Wgo> overlappingExtensions, List<Rect> outSelectionRects)
	{
		if (parentDef == null)
		{
			return;
		}
		HashSet<string> hashSet = this.GetCachedAllowedExtensionIds(parentDef.id);
		for (int i = 0; i < this.buildColliders.Count; i++)
		{
			Collider collider = this.buildColliders[i];
			if (!(collider == null) && collider.gameObject.layer == 19)
			{
				BoxCollider boxCollider = collider as BoxCollider;
				Vector3 vector;
				Vector3 vector2;
				Quaternion quaternion;
				if (boxCollider != null)
				{
					vector = boxCollider.transform.TransformPoint(boxCollider.center);
					vector2 = Vector3.Scale(boxCollider.size * 0.5f, boxCollider.transform.lossyScale);
					vector2.x = Mathf.Max(0f, vector2.x - 0.01f);
					vector2.z = Mathf.Max(0f, vector2.z - 0.0125f);
					quaternion = boxCollider.transform.rotation;
				}
				else
				{
					Bounds bounds = collider.bounds;
					vector = bounds.center;
					vector2 = bounds.extents;
					vector2.x = Mathf.Max(0f, vector2.x - 0.01f);
					vector2.z = Mathf.Max(0f, vector2.z - 0.0125f);
					quaternion = collider.transform.rotation;
				}
				int num = Physics.OverlapBoxNonAlloc(vector, vector2, this.buffAreaOverlapColliders, quaternion, 268435456, QueryTriggerInteraction.Collide);
				for (int j = 0; j < num; j++)
				{
					Collider collider2 = this.buffAreaOverlapColliders[j];
					if (!(collider2 == null))
					{
						Wgo componentInParent = collider2.GetComponentInParent<Wgo>();
						if (!(componentInParent == null) && hashSet.Contains(componentInParent.Data.id))
						{
							overlappingExtensions.Add(componentInParent);
							Rect buildAreaRect = this.GetBuildAreaRect(componentInParent);
							if (buildAreaRect != Rect.zero && !outSelectionRects.Contains(buildAreaRect))
							{
								outSelectionRects.Add(buildAreaRect);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0002BD48 File Offset: 0x00029F48
	private void FillRectsFromBuffCells(HashSet<WGODef> eligibleDefs, HashSet<Wgo> overlappingWorkbenches, List<Rect> outBuffUsageRects)
	{
		Vector2 cell_SIZE = BuildConsts.CELL_SIZE;
		for (int i = 0; i < this.buffCells.Count; i++)
		{
			BuffCell buffCell = this.buffCells[i];
			int num = buffCell.OverlapBoxNonAlloc(this.buffUsageOverlapColliders, 524288);
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				Collider collider = this.buffUsageOverlapColliders[j];
				if (!(collider == null) && !this.buildColliders.Contains(collider))
				{
					Wgo componentInParent = collider.GetComponentInParent<Wgo>();
					if (!(componentInParent == null) && !(componentInParent == this.target) && eligibleDefs.Contains(componentInParent.Data.Definition))
					{
						overlappingWorkbenches.Add(componentInParent);
						flag = true;
					}
				}
			}
			if (flag)
			{
				Vector3 position = buffCell.transform.position;
				outBuffUsageRects.Add(new Rect(position.x - cell_SIZE.x * 0.5f, position.z - cell_SIZE.y * 0.5f, cell_SIZE.x, cell_SIZE.y));
			}
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0002BE60 File Offset: 0x0002A060
	private void AddBuildAreaRects(List<Rect> outSelectionRects, HashSet<Wgo> workbenches)
	{
		foreach (Wgo wgo in workbenches)
		{
			Rect buildAreaRect = this.GetBuildAreaRect(wgo);
			if (buildAreaRect != Rect.zero)
			{
				outSelectionRects.Add(buildAreaRect);
			}
		}
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0002BEC4 File Offset: 0x0002A0C4
	private void AddAlreadyConnectedExtensionParents(string placingExtensionId, HashSet<WGODef> eligibleDefs, HashSet<Wgo> overlappingWorkbenches, List<Rect> outSelectionRects)
	{
		if (string.IsNullOrEmpty(placingExtensionId))
		{
			return;
		}
		foreach (Wgo wgo in LazySingleton<BuildManager>.Instance.WorldZone.Wgos)
		{
			if (!(wgo.Data.id != placingExtensionId))
			{
				foreach (Collider collider in wgo.GetComponentsInChildren<Collider>())
				{
					if (collider.gameObject.layer == 28)
					{
						Collider[] array = new Collider[10];
						int num = Physics.OverlapBoxNonAlloc(collider.bounds.center, WgoBuildPointer.GetBuffAreaOverlapHalfExtents(collider), array, collider.transform.rotation, 524288);
						for (int j = 0; j < num; j++)
						{
							Collider collider2 = array[j];
							if (!(collider2 == null))
							{
								Wgo componentInParent = collider2.GetComponentInParent<Wgo>();
								if (!(componentInParent == null) && eligibleDefs.Contains(componentInParent.Data.Definition) && !overlappingWorkbenches.Contains(componentInParent))
								{
									Rect buildAreaRect = this.GetBuildAreaRect(componentInParent);
									if (buildAreaRect != Rect.zero && !outSelectionRects.Contains(buildAreaRect))
									{
										outSelectionRects.Add(buildAreaRect);
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0002C038 File Offset: 0x0002A238
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != 29)
		{
			return;
		}
		this.UpdateSelectionCellsState();
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0002C038 File Offset: 0x0002A238
	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != 29)
		{
			return;
		}
		this.UpdateSelectionCellsState();
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0002C050 File Offset: 0x0002A250
	public override Vector3 GetCellsCenterLocal()
	{
		if (this.target != null)
		{
			CustomVisualCenterBuildPointer componentInChildren = this.target.GetComponentInChildren<CustomVisualCenterBuildPointer>(true);
			if (componentInChildren != null)
			{
				Vector3 vector = base.transform.InverseTransformPoint(componentInChildren.transform.position);
				vector.y = 0f;
				return vector;
			}
		}
		return base.GetCellsCenterLocal();
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0002C0AC File Offset: 0x0002A2AC
	public override void UpdateCollider()
	{
		if (this.triggerCollider == null)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			this.triggerCollider = boxCollider;
		}
		this.triggerCollider.center = base.GetObjectCenterLocal();
		this.triggerCollider.size = new Vector3(this.roundedBounds.size.x, 1f, this.roundedBounds.size.z);
		this.triggerCollider.isTrigger = true;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x0002C12E File Offset: 0x0002A32E
	public override void OnPointerDisable()
	{
		base.OnPointerDisable();
		this.ClearSelectionTint();
		WorkbenchAdditionWorldIconPresenter.Clear();
		this.ResetAdditionWorldIconPublishState();
		this.InvalidateFullCoverSoftQueryCache();
		this.fullCoverSoftQueryReusableForAvailability = false;
		this.moduleSlotAreas.Clear();
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0002C160 File Offset: 0x0002A360
	private void ClearSelectionTint()
	{
		foreach (Wgo wgo in this.wgosShownAsInactiveWhenExtensionIsPlacing)
		{
			wgo.SetSelectionTint(Color.white, 0f);
		}
		this.wgosShownAsInactiveWhenExtensionIsPlacing.Clear();
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0002C1C8 File Offset: 0x0002A3C8
	private void UpdateUnbuffableObjectsTint()
	{
		this.ClearSelectionTint();
		string item = this.GetPlacingWgo().Item1;
		List<WGODef> list;
		if (GameBalance.Me.TryGetParentWorkbenchDefsForExtension(item, out list))
		{
			foreach (Wgo wgo in LazySingleton<BuildManager>.Instance.WorldZone.Wgos)
			{
				if (!list.Contains(wgo.Data.Definition))
				{
					this.wgosShownAsInactiveWhenExtensionIsPlacing.Add(wgo);
				}
			}
		}
		foreach (Wgo wgo2 in this.wgosShownAsInactiveWhenExtensionIsPlacing)
		{
			wgo2.SetSelectionTint(this.unDestroyableSelectionTintColor, 0.4f);
		}
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0002C2AC File Offset: 0x0002A4AC
	private void UpdateAdditionWorldIcons(WgoBuildPointer.BuildPointerRole role, string placingWgoId, HashSet<Wgo> overlappingHosts)
	{
		if (role == WgoBuildPointer.BuildPointerRole.None)
		{
			WorkbenchAdditionWorldIconPresenter.Clear();
			this.ResetAdditionWorldIconPublishState();
			return;
		}
		BuildManager instance = LazySingleton<BuildManager>.Instance;
		WorldZone worldZone = ((instance != null) ? instance.WorldZone : null);
		if (worldZone == null)
		{
			WorkbenchAdditionWorldIconPresenter.Clear();
			this.ResetAdditionWorldIconPublishState();
			return;
		}
		if (overlappingHosts == null)
		{
			overlappingHosts = this.overlappingHostsBuffer;
		}
		bool flag = this.RefreshEligibleIconHosts(role, placingWgoId, worldZone);
		if (this.hasPublishedIconState && !flag && role == this.cachedIconRole && placingWgoId == this.cachedIconPlacingWgoId && overlappingHosts.SetEquals(this.lastIconOverlapHosts))
		{
			return;
		}
		this.cachedIconRole = role;
		this.cachedIconPlacingWgoId = placingWgoId;
		this.lastIconOverlapHosts.Clear();
		foreach (Wgo wgo in overlappingHosts)
		{
			this.lastIconOverlapHosts.Add(wgo);
		}
		this.hasPublishedIconState = true;
		this.iconStatesBuffer.Clear();
		bool flag2 = role == WgoBuildPointer.BuildPointerRole.Extension && GardenTabletWorldIconLogic.IsGardenTablet(placingWgoId);
		if (role == WgoBuildPointer.BuildPointerRole.Extension)
		{
			string additionWorldIconId = WgoBuildPointer.GetAdditionWorldIconId(placingWgoId);
			for (int i = 0; i < this.cachedEligibleIconHosts.Count; i++)
			{
				Wgo wgo2 = this.cachedEligibleIconHosts[i];
				if (this.IsEligibleWorldIconHost(wgo2))
				{
					this.iconStatesBuffer[wgo2] = new WorkbenchAdditionWorldIconPresenter.State(additionWorldIconId, overlappingHosts.Contains(wgo2), flag2, flag2 ? (-0.5f) : 0f);
				}
			}
		}
		else if (role == WgoBuildPointer.BuildPointerRole.ExtensionParent)
		{
			for (int j = 0; j < this.cachedEligibleIconHosts.Count; j++)
			{
				Wgo wgo3 = this.cachedEligibleIconHosts[j];
				if (this.IsEligibleWorldIconHost(wgo3))
				{
					this.iconStatesBuffer[wgo3] = new WorkbenchAdditionWorldIconPresenter.State(WgoBuildPointer.GetAdditionWorldIconId(wgo3.Data.id), overlappingHosts.Contains(wgo3), false, 0f);
				}
			}
		}
		WorkbenchAdditionWorldIconPresenter.Replace(this.iconStatesBuffer);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0002C498 File Offset: 0x0002A698
	private bool RefreshEligibleIconHosts(WgoBuildPointer.BuildPointerRole role, string placingWgoId, WorldZone worldZone)
	{
		int wgosVersion = worldZone.WgosVersion;
		bool flag = role == WgoBuildPointer.BuildPointerRole.Extension && GardenTabletWorldIconLogic.IsGardenTablet(placingWgoId);
		if (!flag && role == this.cachedEligibleHostsRole && placingWgoId == this.cachedEligibleHostsPlacingWgoId && wgosVersion == this.cachedEligibleHostsZoneWgosVersion)
		{
			return false;
		}
		this.cachedEligibleHostsRole = role;
		this.cachedEligibleHostsPlacingWgoId = placingWgoId;
		this.cachedEligibleHostsZoneWgosVersion = wgosVersion;
		this.cachedEligibleIconHosts.Clear();
		if (role == WgoBuildPointer.BuildPointerRole.Extension)
		{
			List<WGODef> list;
			if (!GameBalance.Me.TryGetParentWorkbenchDefsForExtension(placingWgoId, out list) || list == null || list.Count == 0)
			{
				return true;
			}
			HashSet<WGODef> extensionParentDefs = this.GetExtensionParentDefs(placingWgoId, list);
			using (List<Wgo>.Enumerator enumerator = worldZone.Wgos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Wgo wgo = enumerator.Current;
					if (this.IsEligibleWorldIconHost(wgo) && extensionParentDefs.Contains(wgo.Data.Definition) && (!flag || (GardenBedNavigation.IsGardenPlot(wgo.Data) && GardenTabletWorldIconLogic.DoesTabletAffectPlot(placingWgoId, wgo.Data))))
					{
						this.cachedEligibleIconHosts.Add(wgo);
					}
				}
				return true;
			}
		}
		if (role == WgoBuildPointer.BuildPointerRole.ExtensionParent)
		{
			HashSet<string> hashSet = this.GetCachedAllowedExtensionIds(placingWgoId);
			foreach (Wgo wgo2 in worldZone.Wgos)
			{
				if (this.IsEligibleWorldIconHost(wgo2) && hashSet.Contains(wgo2.Data.id))
				{
					this.cachedEligibleIconHosts.Add(wgo2);
				}
			}
		}
		return true;
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0002C630 File Offset: 0x0002A830
	private void ResetAdditionWorldIconPublishState()
	{
		this.hasPublishedIconState = false;
		this.cachedIconRole = WgoBuildPointer.BuildPointerRole.None;
		this.cachedIconPlacingWgoId = null;
		this.lastIconOverlapHosts.Clear();
		this.cachedEligibleIconHosts.Clear();
		this.cachedEligibleHostsRole = WgoBuildPointer.BuildPointerRole.None;
		this.cachedEligibleHostsPlacingWgoId = null;
		this.cachedEligibleHostsZoneWgosVersion = int.MinValue;
		this.iconStatesBuffer.Clear();
		this.extensionParentDefsBuffer.Clear();
		this.extensionParentDefsBufferForId = null;
		this.cachedAllowedExtensionIds = null;
		this.cachedAllowedExtensionIdsForParentId = null;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0002C6AC File Offset: 0x0002A8AC
	private bool IsEligibleWorldIconHost(Wgo wgo)
	{
		return wgo != null && wgo != this.target && wgo.Data != null && !wgo.Data.isTempObject && !wgo.IsDespawning;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0002C6E8 File Offset: 0x0002A8E8
	private static string GetAdditionWorldIconId(string additionWgoId)
	{
		BuildingDef buildingDef;
		if (!string.IsNullOrEmpty(additionWgoId) && GameBalance.Me.buildableWgos.TryGetValue(additionWgoId, out buildingDef))
		{
			return buildingDef.BuildResultIcon;
		}
		if (!string.IsNullOrEmpty(additionWgoId))
		{
			return "i_b_" + additionWgoId;
		}
		return "i_b_blueprint_placeholder";
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0002C734 File Offset: 0x0002A934
	private Rect GetBuildAreaRect(Wgo wgo)
	{
		bool flag = false;
		Bounds bounds = default(Bounds);
		foreach (Collider collider in wgo.GetComponentsInChildren<Collider>())
		{
			PlacementBlockingArea placementBlockingArea;
			if (collider.gameObject.layer == 19 && !PlacementBlockingArea.TryGet(collider, out placementBlockingArea))
			{
				if (!flag)
				{
					flag = true;
					bounds = collider.bounds;
				}
				else
				{
					bounds.Encapsulate(collider.bounds);
				}
			}
		}
		if (!flag)
		{
			return Rect.zero;
		}
		return new Rect(bounds.min.x, bounds.min.z, bounds.size.x, bounds.size.z);
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0002C7DC File Offset: 0x0002A9DC
	private static Vector3 GetBuffAreaOverlapHalfExtents(Collider buffAreaCollider)
	{
		Vector3 extents = buffAreaCollider.bounds.extents;
		extents.x = Mathf.Max(0f, extents.x - 0.01f);
		extents.z = Mathf.Max(0f, extents.z - 0.0125f);
		return extents;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0002C834 File Offset: 0x0002AA34
	private void DoBuffDrawingLogic()
	{
		BuildManager instance = LazySingleton<BuildManager>.Instance;
		object obj;
		if (instance == null)
		{
			obj = null;
		}
		else
		{
			BuildController buildController = instance.BuildController;
			obj = ((buildController != null) ? buildController.BuildLayout : null);
		}
		object obj2 = obj;
		BuildGrid3D buildGrid3D = ((obj2 != null) ? obj2.BuildGrid3D : null);
		if (buildGrid3D == null || GameBalance.Me == null || this.target == null)
		{
			return;
		}
		ValueTuple<string, WGODef> placingWgo = this.GetPlacingWgo();
		string item = placingWgo.Item1;
		WGODef item2 = placingWgo.Item2;
		if (item2 == null)
		{
			buildGrid3D.SetAllowedExtensionWgoIds(null);
			return;
		}
		if (GameBalance.Me.IsWorkbenchExtensionId(item))
		{
			buildGrid3D.SetAllowedExtensionWgoIds(new HashSet<string> { item });
			return;
		}
		if (GameBalance.Me.workbenchesWhichUseExtensions.Contains(item2))
		{
			HashSet<string> allowedExtensionIdsForParentWorkbench = GameBalance.Me.GetAllowedExtensionIdsForParentWorkbench(item);
			buildGrid3D.SetAllowedExtensionWgoIds(allowedExtensionIdsForParentWorkbench);
			return;
		}
		buildGrid3D.SetAllowedExtensionWgoIds(new HashSet<string>());
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0002C900 File Offset: 0x0002AB00
	private void TrySetCustomRotation(Wgo builtWgo)
	{
		Vector3 vector = base.WorldRoundedBounds.extents / 4f;
		vector.y = 0.5f;
		Collider[] array = new Collider[10];
		if (Physics.OverlapBoxNonAlloc(base.WorldRoundedBounds.center, vector, array, Quaternion.identity, 524288) <= 0)
		{
			return;
		}
		foreach (Collider collider in array)
		{
			BuildArea buildArea;
			if (!(collider == null) && collider.TryGetComponent<BuildArea>(out buildArea) && buildArea.HasRotationRequirement)
			{
				builtWgo.Data.MainWgoPartData.TryApplyState(builtWgo.Data.UniqueId, builtWgo.MainWgoPart.WgoPartData.variationId, buildArea.RotationRequirement);
			}
		}
	}

	// Token: 0x04000A3C RID: 2620
	protected Wgo target;

	// Token: 0x04000A3D RID: 2621
	private List<Collider> buildColliders = new List<Collider>();

	// Token: 0x04000A3E RID: 2622
	protected GameScene gameScene;

	// Token: 0x04000A3F RID: 2623
	private BuildingDef buildingDef;

	// Token: 0x04000A40 RID: 2624
	private Func<bool> canTakeResources;

	// Token: 0x04000A41 RID: 2625
	protected Action takeResourcesAction;

	// Token: 0x04000A42 RID: 2626
	private int overlapMask;

	// Token: 0x04000A43 RID: 2627
	private bool hasCustomBuildArea;

	// Token: 0x04000A44 RID: 2628
	private Vector3 offset;

	// Token: 0x04000A45 RID: 2629
	private BoxCollider triggerCollider;

	// Token: 0x04000A46 RID: 2630
	private readonly HashSet<Wgo> wgosShownAsInactiveWhenExtensionIsPlacing = new HashSet<Wgo>();

	// Token: 0x04000A47 RID: 2631
	private readonly List<BuffCell> buffCells = new List<BuffCell>();

	// Token: 0x04000A48 RID: 2632
	private readonly List<Rect> buffUsageRects = new List<Rect>();

	// Token: 0x04000A49 RID: 2633
	private readonly List<Rect> selectionRects = new List<Rect>();

	// Token: 0x04000A4A RID: 2634
	private readonly Collider[] buffUsageOverlapColliders = new Collider[10];

	// Token: 0x04000A4B RID: 2635
	private readonly Collider[] buffAreaOverlapColliders = new Collider[10];

	// Token: 0x04000A4C RID: 2636
	private Color unDestroyableSelectionTintColor = Color.gray;

	// Token: 0x04000A4D RID: 2637
	private string unDestroyableSelectionTintColorHex = "#444e50";

	// Token: 0x04000A4E RID: 2638
	private readonly List<ModuleSlotArea> moduleSlotAreas = new List<ModuleSlotArea>();

	// Token: 0x04000A4F RID: 2639
	private readonly Collider[] fullCoverSoftAreaOverlap = new Collider[32];

	// Token: 0x04000A50 RID: 2640
	private readonly Collider[] fullCoverSoftOccupantsOverlap = new Collider[64];

	// Token: 0x04000A51 RID: 2641
	private bool fullCoverSoftQueryCached;

	// Token: 0x04000A52 RID: 2642
	private bool cachedHasCoveredFullCoverSoftArea;

	// Token: 0x04000A53 RID: 2643
	private BuildArea cachedFullCoverSoftArea;

	// Token: 0x04000A54 RID: 2644
	private bool cachedFullCoverSoftSlotFree;

	// Token: 0x04000A55 RID: 2645
	private bool fullCoverSoftQueryReusableForAvailability;

	// Token: 0x04000A56 RID: 2646
	private readonly HashSet<Wgo> overlappingHostsBuffer = new HashSet<Wgo>();

	// Token: 0x04000A57 RID: 2647
	private readonly HashSet<Wgo> lastIconOverlapHosts = new HashSet<Wgo>();

	// Token: 0x04000A58 RID: 2648
	private readonly List<Wgo> cachedEligibleIconHosts = new List<Wgo>();

	// Token: 0x04000A59 RID: 2649
	private readonly Dictionary<Wgo, WorkbenchAdditionWorldIconPresenter.State> iconStatesBuffer = new Dictionary<Wgo, WorkbenchAdditionWorldIconPresenter.State>();

	// Token: 0x04000A5A RID: 2650
	private readonly HashSet<WGODef> extensionParentDefsBuffer = new HashSet<WGODef>();

	// Token: 0x04000A5B RID: 2651
	private string extensionParentDefsBufferForId;

	// Token: 0x04000A5C RID: 2652
	private HashSet<string> cachedAllowedExtensionIds;

	// Token: 0x04000A5D RID: 2653
	private string cachedAllowedExtensionIdsForParentId;

	// Token: 0x04000A5E RID: 2654
	private bool hasPublishedIconState;

	// Token: 0x04000A5F RID: 2655
	private WgoBuildPointer.BuildPointerRole cachedIconRole;

	// Token: 0x04000A60 RID: 2656
	private string cachedIconPlacingWgoId;

	// Token: 0x04000A61 RID: 2657
	private WgoBuildPointer.BuildPointerRole cachedEligibleHostsRole;

	// Token: 0x04000A62 RID: 2658
	private string cachedEligibleHostsPlacingWgoId;

	// Token: 0x04000A63 RID: 2659
	private int cachedEligibleHostsZoneWgosVersion = int.MinValue;

	// Token: 0x02000161 RID: 353
	private enum BuildPointerRole
	{
		// Token: 0x04000A66 RID: 2662
		None,
		// Token: 0x04000A67 RID: 2663
		Extension,
		// Token: 0x04000A68 RID: 2664
		ExtensionParent
	}
}
