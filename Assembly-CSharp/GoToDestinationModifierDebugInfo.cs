using System;
using UnityEngine;

// Token: 0x020002E4 RID: 740
[Serializable]
public struct GoToDestinationModifierDebugInfo
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06001368 RID: 4968 RVA: 0x0005E8E0 File Offset: 0x0005CAE0
	private bool IsCombatEntityModifier
	{
		get
		{
			return this.ModifierType == "CombatEntityDestinationModifier";
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06001369 RID: 4969 RVA: 0x0005E8F2 File Offset: 0x0005CAF2
	private bool IsControlPointModifier
	{
		get
		{
			return this.ModifierType == "ControlPointDestinationModifier";
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x0600136A RID: 4970 RVA: 0x0005E904 File Offset: 0x0005CB04
	private bool IsSpearModifier
	{
		get
		{
			return this.ModifierType == "SpearDestinationModifier";
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x0600136B RID: 4971 RVA: 0x0005E916 File Offset: 0x0005CB16
	private bool HasTargetWgoView
	{
		get
		{
			return this.TargetWgo != null;
		}
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x0600136C RID: 4972 RVA: 0x0005E924 File Offset: 0x0005CB24
	private bool HasActiveDockPoint
	{
		get
		{
			return this.ActiveDockPoint != null;
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x0600136D RID: 4973 RVA: 0x0005E92F File Offset: 0x0005CB2F
	private bool HasCustomDockPoint
	{
		get
		{
			return this.CustomDockPoint != null;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x0600136E RID: 4974 RVA: 0x0005E93A File Offset: 0x0005CB3A
	private bool HasActiveDockPointView
	{
		get
		{
			return this.ActiveDockPointView != null;
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x0600136F RID: 4975 RVA: 0x0005E948 File Offset: 0x0005CB48
	private bool HasCustomDockPointView
	{
		get
		{
			return this.CustomDockPointView != null;
		}
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x0005E958 File Offset: 0x0005CB58
	public static Wgo ResolveTargetWgoView(ICombatEntity targetEntity, WgoData wgoData = null)
	{
		Wgo wgo = targetEntity as Wgo;
		if (wgo != null)
		{
			return wgo;
		}
		if (wgoData != null)
		{
			return GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		}
		if (targetEntity != null)
		{
			return GameScene.GetWgoViewGlobal(targetEntity.CombatEntityUID);
		}
		return null;
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x0005E990 File Offset: 0x0005CB90
	public static DockPoint ResolveDockPointView(Wgo targetWgo, DockPointData dockPointData)
	{
		if (targetWgo == null || dockPointData == null)
		{
			return null;
		}
		WgoPart mainWgoPart = targetWgo.MainWgoPart;
		if (mainWgoPart == null)
		{
			return null;
		}
		foreach (DockPoint dockPoint in mainWgoPart.DockPoints)
		{
			if (mainWgoPart.GetDockPointData(dockPoint) == dockPointData)
			{
				return dockPoint;
			}
		}
		return null;
	}

	// Token: 0x040014A2 RID: 5282
	public string ModifierType;

	// Token: 0x040014A3 RID: 5283
	public bool IsValid;

	// Token: 0x040014A4 RID: 5284
	public bool IsStucked;

	// Token: 0x040014A5 RID: 5285
	public bool ShouldAnchorOnArrival;

	// Token: 0x040014A6 RID: 5286
	public Vector3 CurrentTargetPosition;

	// Token: 0x040014A7 RID: 5287
	public string TargetWgoId;

	// Token: 0x040014A8 RID: 5288
	public string TargetUniqueId;

	// Token: 0x040014A9 RID: 5289
	public Wgo TargetWgo;

	// Token: 0x040014AA RID: 5290
	public DockPointData ActiveDockPoint;

	// Token: 0x040014AB RID: 5291
	public DockPoint ActiveDockPointView;

	// Token: 0x040014AC RID: 5292
	public DockPointData CustomDockPoint;

	// Token: 0x040014AD RID: 5293
	public DockPoint CustomDockPointView;

	// Token: 0x040014AE RID: 5294
	public bool WasDockPointValid;

	// Token: 0x040014AF RID: 5295
	public string CapturePointName;

	// Token: 0x040014B0 RID: 5296
	public Vector3 ReservedSlotPosition;

	// Token: 0x040014B1 RID: 5297
	public bool SpearPositionSelected;

	// Token: 0x040014B2 RID: 5298
	public Vector3 SpearAttackOffsetLocal;

	// Token: 0x040014B3 RID: 5299
	public Vector3 SpearTargetPositionWhenSelected;
}
