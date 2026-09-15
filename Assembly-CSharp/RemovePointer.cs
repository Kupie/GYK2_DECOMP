using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015E RID: 350
public class RemovePointer : BuildPointerObject, ICursorChanger
{
	// Token: 0x06000857 RID: 2135 RVA: 0x00002318 File Offset: 0x00000518
	public override void Rotate()
	{
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00029224 File Offset: 0x00027424
	public override bool TryDoBuildAction()
	{
		if (RemovePointer.currentRemovingSelection != null && RemovePointer.currentRemovingSelection.IsBuildRemovable())
		{
			bool flag = RemovePointer.currentRemovingSelection.DoBuildRemove();
			if (this.currentSelectedBuildingDef != null)
			{
				Wgo wgo = RemovePointer.currentRemovingSelection as Wgo;
				if (wgo != null)
				{
					foreach (LazyExpression lazyExpression in this.currentSelectedBuildingDef.expressionAfterBuilding)
					{
						lazyExpression.EvaluateBool(wgo.Data);
					}
				}
			}
			if (!flag)
			{
				Rect rect;
				if (RemovePointer.markedForRemovingObjectRects.TryGetValue(RemovePointer.currentRemovingSelection, out rect))
				{
					RemovePointer.markedForRemovingObjectRects.Remove(RemovePointer.currentRemovingSelection);
				}
				else
				{
					RemovePointer.markedForRemovingObjectRects.Add(RemovePointer.currentRemovingSelection, RemovePointer.currentSelectedObjectRect);
				}
			}
			else
			{
				this.UpdateUnDestroyableRemovables();
				RemovePointer.currentRemovingSelection = null;
				RemovePointer.currentSelectedObjectRect = Rect.zero;
			}
			this.UpdateSelectionRect();
			if (flag)
			{
				this.UpdateUnDestroyableSelectionTint();
			}
			return flag;
		}
		return false;
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00029320 File Offset: 0x00027520
	public override void SetupSelectionCells(BuildSelectionCell[] prefabCells, Transform parent)
	{
		LazyInput.OnInputChanged += this.UpdateGamepadCursorState;
		ColorUtility.TryParseHtmlString(this.selectionTintColorHex, out this.selectionTintColor);
		ColorUtility.TryParseHtmlString(this.unDestroyableSelectionTintColorHex, out this.unDestroyableSelectionTintColor);
		this.UpdateUnDestroyableRemovables();
		this.UpdateSelectionRect();
		this.UpdateUnDestroyableSelectionTint();
		this.SetupGamepadCursor();
		this.UpdateGamepadCursorState();
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00029380 File Offset: 0x00027580
	public void UpdateUnDestroyableRemovables()
	{
		foreach (IBuildRemovable buildRemovable in this.unDestroyableRemovables)
		{
			this.ApplySelectionTint(buildRemovable, 0f, Color.white);
		}
		this.unDestroyableRemovables.Clear();
		foreach (Wgo wgo in LazySingleton<BuildManager>.Instance.WorldZone.Wgos)
		{
			if (wgo.IsBuildRemovable() && wgo.Data.CraftComponent.IsDestroyingCraftActive)
			{
				RemovePointer.markedForRemovingObjectRects[wgo] = this.GetRectForWgo(wgo);
			}
			if (!wgo.IsBuildRemovable())
			{
				this.unDestroyableRemovables.Add(wgo);
			}
			BuildingDef buildingDef;
			GameBalance.Me.removableWgos.TryGetValue(wgo.Data.id, out buildingDef);
			if (buildingDef != null && buildingDef.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.Strict && RemovePointer.HasBuiltWgoOnAnyBuildArea(wgo))
			{
				this.unDestroyableRemovables.Add(wgo);
			}
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x000294B4 File Offset: 0x000276B4
	public override void UpdateSelectionCellsState()
	{
		base.UpdateSelectionCellsState();
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x000294BC File Offset: 0x000276BC
	public override Vector3 GetCellsCenterLocal()
	{
		return new Vector3(BuildConsts.CELL_SIZE.x, 0f, -BuildConsts.CELL_SIZE.y) / 2f;
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x000294E7 File Offset: 0x000276E7
	public override void UpdatePosition(Vector3 position)
	{
		this.position = position;
		this.doFixedUpdate = true;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x000294F7 File Offset: 0x000276F7
	private void LateUpdate()
	{
		this.UpdateGamepadCursorVisual();
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x000294FF File Offset: 0x000276FF
	private void FixedUpdate()
	{
		if (!this.doFixedUpdate)
		{
			return;
		}
		this.doFixedUpdate = false;
		this.UpdateHoverFromOverlapCenter(this.GetSnappedOverlapCenter());
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00029520 File Offset: 0x00027720
	private Vector3 GetSnappedOverlapCenter()
	{
		return this.position + new Vector3(BuildConsts.CELL_SIZE.x, 0f, -BuildConsts.CELL_SIZE.y) / 2f + Vector3.up * 0.01f;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00029578 File Offset: 0x00027778
	private void UpdateHoverFromOverlapCenter(Vector3 overlapCenter)
	{
		int num = 0;
		IBuildRemovable buildRemovable = null;
		Collider[] array = new Collider[20];
		int num2 = Physics.OverlapBoxNonAlloc(overlapCenter, Vector3.Scale(BuildConsts.CASTING_BOX_HALF_EXTENTS, new Vector3(0.1f, 1f, 0.1f)), array, Quaternion.identity, 655616);
		Bounds bounds = default(Bounds);
		bool flag = false;
		for (int i = 0; i < num2; i++)
		{
			Collider collider = array[i];
			if (!(collider == null))
			{
				int layer = collider.gameObject.layer;
				BuildArea buildArea;
				PlacementBlockingArea placementBlockingArea;
				if ((layer == 8 || layer == 19) && (!collider.TryGetComponent<BuildArea>(out buildArea) || (!buildArea.foprceShowAsBuffAreaForPointerPlacement && !buildArea.ignoreForPointerPlacement)) && !PlacementBlockingArea.TryGet(collider, out placementBlockingArea))
				{
					IBuildRemovable componentInParent = collider.GetComponentInParent<IBuildRemovable>();
					if (componentInParent == null || RemovePointer.IsFromCurrentWorldZone(componentInParent))
					{
						if (num < 2)
						{
							num = ((componentInParent != null) ? 1 : 0);
						}
						if (componentInParent != null && componentInParent.IsBuildRemovable() && !this.unDestroyableRemovables.Contains(componentInParent))
						{
							buildRemovable = componentInParent;
							num = 2;
							if (!flag)
							{
								bounds = collider.bounds;
								flag = true;
							}
							else
							{
								bounds.Encapsulate(collider.bounds);
							}
						}
					}
				}
			}
		}
		Rect rect = (flag ? new Rect(bounds.min.x, bounds.min.z, bounds.size.x, bounds.size.z) : Rect.zero);
		IBuildRemovable buildRemovable2 = ((num == 2) ? buildRemovable : null);
		bool flag2 = buildRemovable2 != RemovePointer.currentRemovingSelection || RemovePointer.currentSelectedObjectRect != rect || num != this.previousRemovableFoundState;
		RemovePointer.currentSelectedObjectRect = rect;
		RemovePointer.currentRemovingSelection = buildRemovable2;
		this.currentSelectedBuildingDef = null;
		Wgo wgo = RemovePointer.currentRemovingSelection as Wgo;
		if (wgo != null)
		{
			GameBalance.Me.removableWgos.TryGetValue(wgo.Data.id, out this.currentSelectedBuildingDef);
		}
		if (flag2)
		{
			this.UpdateSelectionRect();
		}
		if (num != this.previousRemovableFoundState)
		{
			this.ApplyCursorHoverState(num);
			this.previousRemovableFoundState = num;
		}
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00029774 File Offset: 0x00027974
	private void ApplyCursorHoverState(int removableFoundState)
	{
		if (!LazyInput.IsGamepadActive)
		{
			CursorController.RemoveCursorState(this);
			if (removableFoundState == 2)
			{
				CursorController.AddCursorState(CursorType.BuildingModeDestroy, this);
			}
		}
		else
		{
			CursorController.RemoveCursorState(this);
		}
		this.RefreshGamepadCursorSprite(removableFoundState == 2);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x000297A0 File Offset: 0x000279A0
	private void UpdateSelectionRect()
	{
		List<Rect> list = RemovePointer.markedForRemovingObjectRects.Values.ToList<Rect>();
		List<IBuildRemovable> list2 = RemovePointer.markedForRemovingObjectRects.Keys.ToList<IBuildRemovable>();
		if (RemovePointer.currentRemovingSelection != null && RemovePointer.currentRemovingSelection.IsBuildRemovable() && !RemovePointer.markedForRemovingObjectRects.ContainsKey(RemovePointer.currentRemovingSelection))
		{
			list.Add(RemovePointer.currentSelectedObjectRect);
		}
		LazySingleton<BuildManager>.Instance.BuildController.BuildLayout.UpdateSelection(list);
		this.UpdateSelectionTint(list2);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00029818 File Offset: 0x00027A18
	private void UpdateUnDestroyableSelectionTint()
	{
		foreach (IBuildRemovable buildRemovable in this.unDestroyableRemovables)
		{
			this.ApplySelectionTint(buildRemovable, 0.4f, this.unDestroyableSelectionTintColor);
		}
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00029878 File Offset: 0x00027A78
	public override void OnPointerDisable()
	{
		base.OnPointerDisable();
		CursorController.RemoveCursorState(this);
		this.ClearSelectionTint();
		RemovePointer.markedForRemovingObjectRects.Clear();
		this.currentSelectedBuildingDef = null;
		LazyInput.OnInputChanged -= this.UpdateGamepadCursorState;
		this.DestroyGamepadCursor();
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x000298B4 File Offset: 0x00027AB4
	private void OnDestroy()
	{
		LazyInput.OnInputChanged -= this.UpdateGamepadCursorState;
		this.DestroyGamepadCursor();
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x000298D0 File Offset: 0x00027AD0
	private void UpdateSelectionTint(List<IBuildRemovable> removables)
	{
		foreach (IBuildRemovable buildRemovable in this.highlightedRemovables)
		{
			this.ApplySelectionTint(buildRemovable, 0f, this.selectionTintColor);
		}
		this.highlightedRemovables.Clear();
		foreach (IBuildRemovable buildRemovable2 in removables)
		{
			if (RemovePointer.IsAliveRemovable(buildRemovable2))
			{
				this.ApplySelectionTint(buildRemovable2, this.selectionTintAmount, this.selectionTintColor);
				this.highlightedRemovables.Add(buildRemovable2);
			}
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00029998 File Offset: 0x00027B98
	private void ClearSelectionTint()
	{
		foreach (IBuildRemovable buildRemovable in this.highlightedRemovables)
		{
			this.ApplySelectionTint(buildRemovable, 0f, Color.white);
		}
		foreach (IBuildRemovable buildRemovable2 in this.unDestroyableRemovables)
		{
			this.ApplySelectionTint(buildRemovable2, 0f, Color.white);
		}
		this.highlightedRemovables.Clear();
		this.unDestroyableRemovables.Clear();
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00029A58 File Offset: 0x00027C58
	private static bool IsAliveRemovable(IBuildRemovable removable)
	{
		if (removable == null)
		{
			return false;
		}
		global::UnityEngine.Object @object = removable as global::UnityEngine.Object;
		return @object == null || !(@object == null);
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00029A80 File Offset: 0x00027C80
	private void ApplySelectionTint(IBuildRemovable removable, float amount, Color color)
	{
		if (!RemovePointer.IsAliveRemovable(removable))
		{
			return;
		}
		Wgo wgo;
		if (!RemovePointer.TryGetWgo(removable, out wgo))
		{
			return;
		}
		wgo.SetSelectionTint(color, amount);
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00029AAC File Offset: 0x00027CAC
	private static bool TryGetWgo(IBuildRemovable removable, out Wgo wgo)
	{
		Wgo wgo2 = removable as Wgo;
		if (wgo2 != null)
		{
			wgo = wgo2;
			return true;
		}
		Component component = removable as Component;
		if (component != null)
		{
			wgo = component.GetComponentInParent<Wgo>();
			return wgo != null;
		}
		wgo = null;
		return false;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00029AE8 File Offset: 0x00027CE8
	private Rect GetRectForWgo(Wgo wgo)
	{
		bool flag = false;
		Bounds bounds = default(Bounds);
		foreach (Collider collider in wgo.GetComponentsInChildren<Collider>())
		{
			if (collider.gameObject.layer == 19)
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

	// Token: 0x0600086D RID: 2157 RVA: 0x00029B84 File Offset: 0x00027D84
	private static bool HasBuiltWgoOnAnyBuildArea(Wgo currentWgo)
	{
		if (((currentWgo != null) ? currentWgo.Data : null) == null)
		{
			return false;
		}
		WorldZoneData worldZoneData;
		if ((worldZoneData = currentWgo.Data.WorldZoneData) == null)
		{
			WorldZone worldZone = LazySingleton<BuildManager>.Instance.WorldZone;
			worldZoneData = ((worldZone != null) ? worldZone.Data : null);
		}
		WorldZoneData worldZoneData2 = worldZoneData;
		if (worldZoneData2 == null)
		{
			return false;
		}
		HashSet<WgoData> hashSet = new HashSet<WgoData>();
		foreach (Collider collider in currentWgo.GetComponentsInChildren<Collider>())
		{
			PlacementBlockingArea placementBlockingArea;
			if (!(collider == null) && collider.gameObject.layer == 19 && !PlacementBlockingArea.TryGet(collider, out placementBlockingArea))
			{
				Bounds bounds = collider.bounds;
				Rect rect = new Rect(bounds.min.x, bounds.min.z, bounds.size.x, bounds.size.z);
				foreach (WgoData wgoData in worldZoneData2.GetWgoDataByRect(rect))
				{
					hashSet.Add(wgoData);
				}
			}
		}
		BuildingDef buildingDef;
		GameBalance.Me.buildableWgos.TryGetValue(currentWgo.Data.id, out buildingDef);
		using (HashSet<WgoData>.Enumerator enumerator2 = hashSet.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (Wgo.IsOccupyingWgoOnBuildArea(enumerator2.Current, currentWgo.Data.UniqueId, buildingDef, false))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00029D20 File Offset: 0x00027F20
	private static bool IsFromCurrentWorldZone(IBuildRemovable removable)
	{
		Wgo wgo;
		if (!RemovePointer.TryGetWgo(removable, out wgo))
		{
			return true;
		}
		WorldZone worldZone = LazySingleton<BuildManager>.Instance.WorldZone;
		WorldZoneData worldZoneData = ((worldZone != null) ? worldZone.Data : null);
		WgoData data = wgo.Data;
		WorldZoneData worldZoneData2 = ((data != null) ? data.WorldZoneData : null);
		return worldZoneData == null || worldZoneData2 == null || worldZoneData == worldZoneData2 || worldZoneData.id == worldZoneData2.id;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00029D84 File Offset: 0x00027F84
	private void SetupGamepadCursor()
	{
		if (!RemovePointer.TryCreateCursorSprite(CursorType.Default, out this.defaultCursorSprite, out this.defaultCursorPivot))
		{
			Debug.LogError("RemovePointer: default cursor texture is missing on CursorController. Assign it on Assets/Prefabs/Systems/CursorController.prefab.");
			return;
		}
		if (!RemovePointer.TryCreateCursorSprite(CursorType.BuildingModeDestroy, out this.destroyCursorSprite, out this.destroyCursorPivot))
		{
			this.destroyCursorSprite = this.defaultCursorSprite;
		}
		GameObject gameObject = new GameObject("RemovePointerGamepadCursor");
		this.gamepadCursorCanvas = gameObject.AddComponent<Canvas>();
		this.gamepadCursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
		this.gamepadCursorCanvas.pixelPerfect = true;
		this.gamepadCursorCanvas.sortingOrder = 701;
		gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
		GameObject gameObject2 = new GameObject("Cursor");
		gameObject2.transform.SetParent(gameObject.transform, false);
		this.gamepadCursorImage = gameObject2.AddComponent<Image>();
		this.gamepadCursorImage.raycastTarget = false;
		this.gamepadCursorImage.preserveAspect = true;
		this.gamepadCursorRect = this.gamepadCursorImage.rectTransform;
		this.gamepadCursorRect.anchorMin = Vector2.zero;
		this.gamepadCursorRect.anchorMax = Vector2.zero;
		this.gamepadCursorReady = true;
		this.RefreshGamepadCursorSprite(false);
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00029E9C File Offset: 0x0002809C
	private static bool TryCreateCursorSprite(CursorType type, out Sprite sprite, out Vector2 pivot)
	{
		sprite = null;
		pivot = new Vector2(0f, 1f);
		CursorConfiguration cursorConfiguration;
		if (!CursorController.TryGetCursorConfiguration(type, out cursorConfiguration))
		{
			return false;
		}
		Texture2D sprite2 = cursorConfiguration.sprite;
		if (sprite2 == null)
		{
			return false;
		}
		if (sprite2.width > 0 && sprite2.height > 0)
		{
			pivot = new Vector2(cursorConfiguration.hotSpot.x / (float)sprite2.width, 1f - cursorConfiguration.hotSpot.y / (float)sprite2.height);
		}
		sprite = Sprite.Create(sprite2, new Rect(0f, 0f, (float)sprite2.width, (float)sprite2.height), pivot, 100f, 0U, SpriteMeshType.FullRect);
		sprite.name = sprite2.name + "_RemovePointer";
		return true;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00029F74 File Offset: 0x00028174
	private void RefreshGamepadCursorSprite(bool hoverRemovable)
	{
		if (!this.gamepadCursorReady || this.gamepadCursorImage == null)
		{
			return;
		}
		Sprite sprite = ((hoverRemovable && this.destroyCursorSprite != null) ? this.destroyCursorSprite : this.defaultCursorSprite);
		Vector2 vector = (hoverRemovable ? this.destroyCursorPivot : this.defaultCursorPivot);
		if (sprite == null)
		{
			return;
		}
		this.gamepadCursorImage.sprite = sprite;
		this.gamepadCursorRect.pivot = vector;
		this.gamepadCursorImage.SetNativeSize();
		float softwareCursorScale = CursorController.GetSoftwareCursorScale();
		this.gamepadCursorRect.localScale = Vector3.one * softwareCursorScale;
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0002A014 File Offset: 0x00028214
	private void UpdateGamepadCursorVisual()
	{
		if (!this.gamepadCursorReady || this.gamepadCursorRect == null)
		{
			return;
		}
		bool isGamepadActive = LazyInput.IsGamepadActive;
		if (this.gamepadCursorCanvas != null && this.gamepadCursorCanvas.gameObject.activeSelf != isGamepadActive)
		{
			this.gamepadCursorCanvas.gameObject.SetActive(isGamepadActive);
		}
		if (!isGamepadActive)
		{
			return;
		}
		Vector3 cursorScreenPosition = RemovePointer.GetCursorScreenPosition();
		cursorScreenPosition.z = 0f;
		this.gamepadCursorRect.position = cursorScreenPosition;
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x0002A092 File Offset: 0x00028292
	private void UpdateGamepadCursorState()
	{
		if (LazyInput.IsGamepadActive)
		{
			CursorController.RemoveCursorState(this);
			this.RefreshGamepadCursorSprite(this.previousRemovableFoundState == 2);
		}
		else
		{
			this.ApplyCursorHoverState(this.previousRemovableFoundState);
		}
		this.UpdateGamepadCursorVisual();
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0002A0C4 File Offset: 0x000282C4
	private static Vector3 GetCursorScreenPosition()
	{
		if (BuildController.Instance != null)
		{
			return BuildController.Instance.LastCursorScreenPosition;
		}
		return Input.mousePosition;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0002A0E4 File Offset: 0x000282E4
	private void DestroyGamepadCursor()
	{
		if (this.gamepadCursorCanvas != null)
		{
			global::UnityEngine.Object.Destroy(this.gamepadCursorCanvas.gameObject);
		}
		this.gamepadCursorCanvas = null;
		this.gamepadCursorRect = null;
		this.gamepadCursorImage = null;
		this.gamepadCursorReady = false;
		if (this.defaultCursorSprite != null)
		{
			global::UnityEngine.Object.Destroy(this.defaultCursorSprite);
		}
		if (this.destroyCursorSprite != null && this.destroyCursorSprite != this.defaultCursorSprite)
		{
			global::UnityEngine.Object.Destroy(this.destroyCursorSprite);
		}
		this.defaultCursorSprite = null;
		this.destroyCursorSprite = null;
	}

	// Token: 0x04000A26 RID: 2598
	private static readonly Dictionary<IBuildRemovable, Rect> markedForRemovingObjectRects = new Dictionary<IBuildRemovable, Rect>();

	// Token: 0x04000A27 RID: 2599
	private static Rect currentSelectedObjectRect = Rect.zero;

	// Token: 0x04000A28 RID: 2600
	private static IBuildRemovable currentRemovingSelection = null;

	// Token: 0x04000A29 RID: 2601
	private float selectionTintAmount = 0.3f;

	// Token: 0x04000A2A RID: 2602
	private Color selectionTintColor = Color.red;

	// Token: 0x04000A2B RID: 2603
	private Color unDestroyableSelectionTintColor = Color.gray;

	// Token: 0x04000A2C RID: 2604
	private string selectionTintColorHex = "#ca2b00";

	// Token: 0x04000A2D RID: 2605
	private string unDestroyableSelectionTintColorHex = "#444e50";

	// Token: 0x04000A2E RID: 2606
	private readonly HashSet<IBuildRemovable> highlightedRemovables = new HashSet<IBuildRemovable>();

	// Token: 0x04000A2F RID: 2607
	private readonly HashSet<IBuildRemovable> unDestroyableRemovables = new HashSet<IBuildRemovable>();

	// Token: 0x04000A30 RID: 2608
	private Vector3 position;

	// Token: 0x04000A31 RID: 2609
	private bool doFixedUpdate;

	// Token: 0x04000A32 RID: 2610
	private int previousRemovableFoundState = -1;

	// Token: 0x04000A33 RID: 2611
	private BuildingDef currentSelectedBuildingDef;

	// Token: 0x04000A34 RID: 2612
	private Canvas gamepadCursorCanvas;

	// Token: 0x04000A35 RID: 2613
	private RectTransform gamepadCursorRect;

	// Token: 0x04000A36 RID: 2614
	private Image gamepadCursorImage;

	// Token: 0x04000A37 RID: 2615
	private Sprite defaultCursorSprite;

	// Token: 0x04000A38 RID: 2616
	private Sprite destroyCursorSprite;

	// Token: 0x04000A39 RID: 2617
	private Vector2 defaultCursorPivot;

	// Token: 0x04000A3A RID: 2618
	private Vector2 destroyCursorPivot;

	// Token: 0x04000A3B RID: 2619
	private bool gamepadCursorReady;
}
