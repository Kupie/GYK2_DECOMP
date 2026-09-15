using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
public static class GardenBedNavigation
{
	// Token: 0x060025A9 RID: 9641 RVA: 0x000B06E0 File Offset: 0x000AE8E0
	public static bool IsGardenPlot(WgoData wgoData)
	{
		if (wgoData == null || wgoData.isTempObject || wgoData.Definition == null)
		{
			return false;
		}
		string wgoGroup = wgoData.Definition.wgoGroup;
		return wgoGroup == "garden_bed" || wgoGroup == "vineyard_objects";
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000B0728 File Offset: 0x000AE928
	public static void TryRebuild(WgoData wgoData)
	{
		if (!GardenBedNavigation.IsGardenPlot(wgoData))
		{
			return;
		}
		GardenBedNavigation.RebuildApproachPoints(wgoData);
		GardenBedNavigation.MarkRebuilt(wgoData);
		GardenBedNavigation.RefreshAround(wgoData, null);
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x000B0746 File Offset: 0x000AE946
	public static void TryRebuildOnViewRespawn(WgoData wgoData)
	{
		if (!GardenBedNavigation.IsGardenPlot(wgoData))
		{
			return;
		}
		if (GardenBedNavigation.WasRebuiltThisSession(wgoData))
		{
			return;
		}
		GardenBedNavigation.TryRebuild(wgoData);
	}

	// Token: 0x060025AC RID: 9644 RVA: 0x000B0760 File Offset: 0x000AE960
	public static void UnlinkApproachPoints(WgoData wgoData)
	{
		GardenBedNavigation.ClearRebuilt(wgoData);
		if (((wgoData != null) ? wgoData.gdPointsData : null) == null)
		{
			return;
		}
		foreach (GDPointData gdpointData in wgoData.gdPointsData)
		{
			if (GardenBedNavigation.IsApproachPoint(gdpointData))
			{
				GardenBedNavigation.UnlinkFromNeighbors(gdpointData);
			}
		}
	}

	// Token: 0x060025AD RID: 9645 RVA: 0x000B07D0 File Offset: 0x000AE9D0
	public static void RefreshAround(WgoData origin, WgoData ignore)
	{
		WorldZoneData worldZoneData = ((origin != null) ? origin.WorldZoneData : null);
		if (worldZoneData == null)
		{
			return;
		}
		Rect rect = GardenBedNavigation.Expand(GardenBedNavigation.GetWorldOccupancyRect(origin), 2f);
		GardenBedNavigation.ApplyOccupancy(worldZoneData, rect, ignore);
		MainGame instance = MainGame.Instance;
		if (instance == null)
		{
			return;
		}
		GraphHelper graphHelper = instance.GraphHelper;
		if (graphHelper == null)
		{
			return;
		}
		graphHelper.QueueRescanGDPointGraph();
	}

	// Token: 0x060025AE RID: 9646 RVA: 0x000B0820 File Offset: 0x000AEA20
	public static bool TryGetOpenApproach(WgoData plot, Vector3 fromPosition, out Vector3 position, out Direction direction)
	{
		position = default(Vector3);
		direction = Direction.Down;
		if (!GardenBedNavigation.IsGardenPlot(plot))
		{
			return false;
		}
		GDPointData gdpointData = null;
		float num = float.MaxValue;
		if (plot.gdPointsData != null)
		{
			foreach (GDPointData gdpointData2 in plot.gdPointsData)
			{
				if (GardenBedNavigation.IsApproachPoint(gdpointData2) && gdpointData2.Enabled && GardenBedNavigation.HasEnabledLink(gdpointData2))
				{
					float sqrMagnitude = (gdpointData2.Position - fromPosition).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						gdpointData = gdpointData2;
					}
				}
			}
		}
		if (gdpointData != null)
		{
			position = gdpointData.Position;
			direction = gdpointData.Direction;
			return true;
		}
		if (plot.MainWgoPartData == null)
		{
			return false;
		}
		DockPointData dockPointData = null;
		foreach (DockPointData dockPointData2 in plot.MainWgoPartData.GetDockPoints(DockPointData.Availability.All, DockPointData.Filter.All))
		{
			if (dockPointData2.BakedData != null && !dockPointData2.BakedData.DontUseForWorkerPlacement)
			{
				Vector3 dockPointDataWorldPosition = plot.GetDockPointDataWorldPosition(dockPointData2);
				if (!GardenBedNavigation.IsInsideAnyOtherPlot(dockPointDataWorldPosition, plot, null))
				{
					float sqrMagnitude2 = (dockPointDataWorldPosition - fromPosition).sqrMagnitude;
					if (sqrMagnitude2 < num)
					{
						num = sqrMagnitude2;
						dockPointData = dockPointData2;
						position = dockPointDataWorldPosition;
						direction = dockPointData2.Direction;
					}
				}
			}
		}
		return dockPointData != null;
	}

	// Token: 0x060025AF RID: 9647 RVA: 0x000B0998 File Offset: 0x000AEB98
	private static void RebuildApproachPoints(WgoData wgoData)
	{
		GardenBedNavigation.UnlinkApproachPoints(wgoData);
		List<GDPointData> list = new List<GDPointData>();
		if (wgoData.gdPointsData != null)
		{
			foreach (GDPointData gdpointData in wgoData.gdPointsData)
			{
				if (!GardenBedNavigation.IsApproachPoint(gdpointData))
				{
					list.Add(gdpointData);
				}
			}
		}
		if (wgoData.MainWgoPartData != null)
		{
			List<DockPointData> dockPoints = wgoData.MainWgoPartData.GetDockPoints(DockPointData.Availability.All, DockPointData.Filter.All);
			int num = -536870912 + wgoData.UniqueId.GetHashCode();
			for (int i = 0; i < dockPoints.Count; i++)
			{
				DockPointData dockPointData = dockPoints[i];
				if (dockPointData.BakedData != null && !dockPointData.BakedData.DontUseForWorkerPlacement)
				{
					Vector3 dockPointDataWorldPosition = wgoData.GetDockPointDataWorldPosition(dockPointData);
					GDPointData gdpointData2 = new GDPointData(string.Format("{0}_approach_{1}", wgoData.UniqueId.Id, i), "garden_bed_approach", num + i, dockPointDataWorldPosition, dockPointData.Direction, wgoData.WorldId, true, true);
					list.Add(gdpointData2);
				}
			}
		}
		wgoData.RewriteGdPointsData(list);
	}

	// Token: 0x060025B0 RID: 9648 RVA: 0x000B0AC4 File Offset: 0x000AECC4
	private static void ApplyOccupancy(WorldZoneData zone, Rect searchRect, WgoData ignore)
	{
		MainGame instance = MainGame.Instance;
		GdPointsData gdPointsData;
		if (instance == null)
		{
			gdPointsData = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			if (gameSave == null)
			{
				gdPointsData = null;
			}
			else
			{
				WorldData worldData = gameSave.worldData;
				gdPointsData = ((worldData != null) ? worldData.gdPointsData : null);
			}
		}
		GdPointsData gdPointsData2 = gdPointsData;
		if (gdPointsData2 == null)
		{
			return;
		}
		List<WgoData> list = GardenBedNavigation.CollectPlots(zone, searchRect, ignore);
		List<GDPointData> points = gdPointsData2.Points;
		foreach (WgoData wgoData in list)
		{
			if (wgoData.gdPointsData != null)
			{
				foreach (GDPointData gdpointData in wgoData.gdPointsData)
				{
					if (GardenBedNavigation.IsApproachPoint(gdpointData))
					{
						GardenBedNavigation.UnlinkFromNeighbors(gdpointData);
						bool flag = !GardenBedNavigation.IsInsideAnyOtherPlot(gdpointData.Position, wgoData, ignore);
						gdpointData.SetEnabledStateSilent(flag);
						if (flag)
						{
							GardenBedNavigation.ConnectToNearestWalkway(gdpointData, points, list);
						}
					}
				}
			}
		}
		foreach (GDPointData gdpointData2 in points)
		{
			if (!GardenBedNavigation.IsApproachPoint(gdpointData2) && gdpointData2.IsWaypoint)
			{
				Vector2 vector = new Vector2(gdpointData2.Position.x, gdpointData2.Position.z);
				if (searchRect.Contains(vector))
				{
					bool flag2 = GardenBedNavigation.IsInsideAnyPlot(gdpointData2.Position, ignore, list);
					gdpointData2.SetEnabledStateSilent(!flag2);
				}
			}
		}
		foreach (GDPointData gdpointData3 in points)
		{
			if (GardenBedNavigation.IsApproachPoint(gdpointData3))
			{
				GardenBedNavigation.PruneDisabledLinks(gdpointData3, gdPointsData2);
				if (!GardenBedNavigation.HasEnabledLink(gdpointData3))
				{
					gdpointData3.SetEnabledStateSilent(false);
				}
			}
		}
	}

	// Token: 0x060025B1 RID: 9649 RVA: 0x000B0CB8 File Offset: 0x000AEEB8
	private static void ConnectToNearestWalkway(GDPointData approach, List<GDPointData> allPoints, List<WgoData> plots)
	{
		GDPointData gdpointData = null;
		float num = 1.5f;
		foreach (GDPointData gdpointData2 in allPoints)
		{
			if (gdpointData2 != null && gdpointData2 != approach && !GardenBedNavigation.IsApproachPoint(gdpointData2) && gdpointData2.IsWaypoint && gdpointData2.Enabled && !GardenBedNavigation.IsInsideAnyPlot(gdpointData2.Position, null, plots))
			{
				float num2 = Vector3.Distance(approach.Position, gdpointData2.Position);
				if (num2 < num)
				{
					num = num2;
					gdpointData = gdpointData2;
				}
			}
		}
		if (gdpointData == null)
		{
			return;
		}
		approach.AddNextNodeInstanceId(gdpointData.InstanceId);
		gdpointData.AddNextNodeInstanceId(approach.InstanceId);
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x000B0D70 File Offset: 0x000AEF70
	private static void UnlinkFromNeighbors(GDPointData point)
	{
		MainGame instance = MainGame.Instance;
		GdPointsData gdPointsData;
		if (instance == null)
		{
			gdPointsData = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			if (gameSave == null)
			{
				gdPointsData = null;
			}
			else
			{
				WorldData worldData = gameSave.worldData;
				gdPointsData = ((worldData != null) ? worldData.gdPointsData : null);
			}
		}
		GdPointsData gdPointsData2 = gdPointsData;
		if (gdPointsData2 == null)
		{
			return;
		}
		if (point.NextNodeInstanceIds == null || point.NextNodeInstanceIds.Count == 0)
		{
			return;
		}
		foreach (int num in new List<int>(point.NextNodeInstanceIds))
		{
			GDPointData gdpointDataByInstanceId = gdPointsData2.GetGDPointDataByInstanceId(num);
			if (gdpointDataByInstanceId != null)
			{
				gdpointDataByInstanceId.RemoveNextNodeInstanceId(point.InstanceId);
			}
		}
		point.SetNextNodeInstanceIds(new List<int>());
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x000B0E28 File Offset: 0x000AF028
	private static bool HasEnabledLink(GDPointData point)
	{
		if (point.NextPointData == null)
		{
			return false;
		}
		foreach (GDPointData gdpointData in point.NextPointData)
		{
			if (gdpointData != null && gdpointData.Enabled)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060025B4 RID: 9652 RVA: 0x000B0E90 File Offset: 0x000AF090
	private static void PruneDisabledLinks(GDPointData point, GdPointsData gdPointsData)
	{
		if (point.NextNodeInstanceIds == null || point.NextNodeInstanceIds.Count == 0)
		{
			return;
		}
		foreach (int num in new List<int>(point.NextNodeInstanceIds))
		{
			GDPointData gdpointDataByInstanceId = gdPointsData.GetGDPointDataByInstanceId(num);
			if (gdpointDataByInstanceId == null || !gdpointDataByInstanceId.Enabled)
			{
				point.RemoveNextNodeInstanceId(num);
				if (gdpointDataByInstanceId != null)
				{
					gdpointDataByInstanceId.RemoveNextNodeInstanceId(point.InstanceId);
				}
			}
		}
	}

	// Token: 0x060025B5 RID: 9653 RVA: 0x000B0F20 File Offset: 0x000AF120
	private static List<WgoData> CollectPlots(WorldZoneData zone, Rect searchRect, WgoData ignore)
	{
		List<WgoData> list = new List<WgoData>();
		foreach (SGuid sguid in zone.wgoDataList)
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(sguid);
			if (wgoData != null && wgoData != ignore && !wgoData.isRemovingFromData && GardenBedNavigation.IsGardenPlot(wgoData))
			{
				Rect worldOccupancyRect = GardenBedNavigation.GetWorldOccupancyRect(wgoData);
				if (searchRect.Overlaps(worldOccupancyRect, true))
				{
					list.Add(wgoData);
				}
			}
		}
		return list;
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x000B0FB4 File Offset: 0x000AF1B4
	private static bool IsInsideAnyOtherPlot(Vector3 worldPos, WgoData owner, WgoData ignore)
	{
		WorldZoneData worldZoneData = ((owner != null) ? owner.WorldZoneData : null);
		if (worldZoneData == null)
		{
			return false;
		}
		foreach (SGuid sguid in worldZoneData.wgoDataList)
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(sguid);
			if (wgoData != null && wgoData != owner && wgoData != ignore && !wgoData.isRemovingFromData && GardenBedNavigation.IsGardenPlot(wgoData) && GardenBedNavigation.ContainsXZ(GardenBedNavigation.GetWorldOccupancyRect(wgoData), worldPos))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x000B1050 File Offset: 0x000AF250
	private static bool IsInsideAnyPlot(Vector3 worldPos, WgoData ignore, List<WgoData> plotsOverride = null)
	{
		if (plotsOverride != null)
		{
			foreach (WgoData wgoData in plotsOverride)
			{
				if (wgoData != ignore && GardenBedNavigation.ContainsXZ(GardenBedNavigation.GetWorldOccupancyRect(wgoData), worldPos))
				{
					return true;
				}
			}
			return false;
		}
		WorldZoneData worldZoneData = ((ignore != null) ? ignore.WorldZoneData : null);
		if (worldZoneData == null)
		{
			return false;
		}
		foreach (SGuid sguid in worldZoneData.wgoDataList)
		{
			WgoData wgoData2 = MainGame.WorldData.GetWgoData(sguid);
			if (wgoData2 != null && wgoData2 != ignore && !wgoData2.isRemovingFromData && GardenBedNavigation.IsGardenPlot(wgoData2) && GardenBedNavigation.ContainsXZ(GardenBedNavigation.GetWorldOccupancyRect(wgoData2), worldPos))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x000B1144 File Offset: 0x000AF344
	public static Rect GetWorldOccupancyRect(WgoData wgoData)
	{
		if (((wgoData != null) ? wgoData.MainWgoPartData : null) != null)
		{
			Rect collisionBoundsRect = wgoData.MainWgoPartData.GetCollisionBoundsRect(wgoData.Position);
			if (collisionBoundsRect.width > 0.01f && collisionBoundsRect.height > 0.01f)
			{
				return collisionBoundsRect;
			}
		}
		if (wgoData != null && wgoData.HasSerializedBounds)
		{
			Bounds bounds = wgoData.SerializedBounds.GetBounds();
			Vector3 vector = wgoData.Position + bounds.center;
			return new Rect(vector.x - bounds.extents.x, vector.z - bounds.extents.z, bounds.size.x, bounds.size.z);
		}
		Vector3 vector2 = ((wgoData != null) ? wgoData.Position : Vector3.zero);
		return new Rect(vector2.x - 0.6f, vector2.z - 0.75f, 1.2f, 1.5f);
	}

	// Token: 0x060025B9 RID: 9657 RVA: 0x000B123C File Offset: 0x000AF43C
	private static void MarkRebuilt(WgoData wgoData)
	{
		string text;
		if (wgoData == null)
		{
			text = null;
		}
		else
		{
			SGuid uniqueId = wgoData.UniqueId;
			text = ((uniqueId != null) ? uniqueId.Id : null);
		}
		string text2 = text;
		if (!string.IsNullOrEmpty(text2))
		{
			GardenBedNavigation.rebuiltThisSession.Add(text2);
		}
	}

	// Token: 0x060025BA RID: 9658 RVA: 0x000B1278 File Offset: 0x000AF478
	private static void ClearRebuilt(WgoData wgoData)
	{
		string text;
		if (wgoData == null)
		{
			text = null;
		}
		else
		{
			SGuid uniqueId = wgoData.UniqueId;
			text = ((uniqueId != null) ? uniqueId.Id : null);
		}
		string text2 = text;
		if (!string.IsNullOrEmpty(text2))
		{
			GardenBedNavigation.rebuiltThisSession.Remove(text2);
		}
	}

	// Token: 0x060025BB RID: 9659 RVA: 0x000B12B4 File Offset: 0x000AF4B4
	private static bool WasRebuiltThisSession(WgoData wgoData)
	{
		string text;
		if (wgoData == null)
		{
			text = null;
		}
		else
		{
			SGuid uniqueId = wgoData.UniqueId;
			text = ((uniqueId != null) ? uniqueId.Id : null);
		}
		string text2 = text;
		return !string.IsNullOrEmpty(text2) && GardenBedNavigation.rebuiltThisSession.Contains(text2);
	}

	// Token: 0x060025BC RID: 9660 RVA: 0x000B12EF File Offset: 0x000AF4EF
	private static bool IsApproachPoint(GDPointData point)
	{
		return point != null && point.CustomTag == "garden_bed_approach";
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x000B1306 File Offset: 0x000AF506
	private static bool ContainsXZ(Rect rect, Vector3 worldPos)
	{
		return rect.Contains(new Vector2(worldPos.x, worldPos.z));
	}

	// Token: 0x060025BE RID: 9662 RVA: 0x000B1320 File Offset: 0x000AF520
	private static Rect Expand(Rect rect, float padding)
	{
		return new Rect(rect.xMin - padding, rect.yMin - padding, rect.width + padding * 2f, rect.height + padding * 2f);
	}

	// Token: 0x040020E5 RID: 8421
	public const string ApproachCustomTag = "garden_bed_approach";

	// Token: 0x040020E6 RID: 8422
	private const float NeighborSearchPadding = 2f;

	// Token: 0x040020E7 RID: 8423
	private const float WalkwayConnectMaxDistance = 1.5f;

	// Token: 0x040020E8 RID: 8424
	private const float FallbackOccupancyHalfWidth = 0.6f;

	// Token: 0x040020E9 RID: 8425
	private const float FallbackOccupancyHalfDepth = 0.75f;

	// Token: 0x040020EA RID: 8426
	private static readonly HashSet<string> rebuiltThisSession = new HashSet<string>();
}
