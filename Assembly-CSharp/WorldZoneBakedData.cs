using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class WorldZoneBakedData : ScriptableObject
{
	// Token: 0x06000B67 RID: 2919 RVA: 0x00038C94 File Offset: 0x00036E94
	public void SetFrom(WorldZone worldZone)
	{
		if (worldZone == null)
		{
			return;
		}
		this.id = worldZone.Id;
		this.contentPartName = WorldZoneBakedData.ResolveContentPartName(worldZone.transform);
		this.pos = worldZone.transform.position;
		this.worldZoneType = worldZone.WorldZoneType;
		this.navigationGraph = worldZone.NavigationGraph;
		this.additionalMovementGraphs = worldZone.AdditionalMovementGraphs;
		this.processingPriority = worldZone.ProcessingPriority;
		if (worldZone.ZoneCollider != null)
		{
			Vector3 vector = worldZone.ZoneCollider.transform.TransformPoint(worldZone.ZoneCollider.center);
			Vector3 size = worldZone.ZoneCollider.size;
			this.wholeZoneRect = new Rect(new Vector2(vector.x - size.x / 2f, vector.z - size.z / 2f), new Vector2(size.x, size.z));
		}
		this.elevationAreas.Clear();
		foreach (WorldZoneElevationArea worldZoneElevationArea in worldZone.GetComponentsInChildren<WorldZoneElevationArea>(true))
		{
			if (!(worldZoneElevationArea == null))
			{
				this.elevationAreas.Add(worldZoneElevationArea.ToBakedData());
			}
		}
		this.navigationHoles.Clear();
		IReadOnlyList<Collider> navigationHoleColliders = worldZone.NavigationHoleColliders;
		if (navigationHoleColliders != null)
		{
			for (int j = 0; j < navigationHoleColliders.Count; j++)
			{
				Collider collider = navigationHoleColliders[j];
				if (!(collider == null))
				{
					BoxCollider boxCollider = collider as BoxCollider;
					if (boxCollider != null)
					{
						this.navigationHoles.Add(WorldZoneNavigationHoleBakedData.FromBoxCollider(boxCollider));
					}
					else
					{
						Debug.LogWarning(string.Concat(new string[] { "WorldZone [", worldZone.Id, "] navigation hole [", collider.name, "] is not a BoxCollider and was skipped during bake." }), collider);
					}
				}
			}
		}
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00038E70 File Offset: 0x00037070
	private static string ResolveContentPartName(Transform worldZoneTransform)
	{
		SceneWgoContentPart componentInParent = worldZoneTransform.GetComponentInParent<SceneWgoContentPart>(true);
		if (componentInParent == null)
		{
			return string.Empty;
		}
		string name = componentInParent.name;
		if (!name.EndsWith("Data"))
		{
			return name;
		}
		return name.Substring(0, name.Length - 4);
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00038EBC File Offset: 0x000370BC
	public static bool TryGetPrebuiltWgoParams(IReadOnlyList<WorldZoneBakedData> bakedZones, SGuid wgoUniqueId, out WorldZonePrebuiltWgoParams prebuiltParams)
	{
		prebuiltParams = null;
		if (bakedZones == null || SGuid.IsNullOrEmpty(wgoUniqueId))
		{
			return false;
		}
		for (int i = 0; i < bakedZones.Count; i++)
		{
			WorldZoneBakedData worldZoneBakedData = bakedZones[i];
			if (((worldZoneBakedData != null) ? worldZoneBakedData.prebuiltWgoParams : null) != null)
			{
				for (int j = 0; j < worldZoneBakedData.prebuiltWgoParams.Count; j++)
				{
					WorldZonePrebuiltWgoParams worldZonePrebuiltWgoParams = worldZoneBakedData.prebuiltWgoParams[j];
					if (worldZonePrebuiltWgoParams != null && !(worldZonePrebuiltWgoParams.wgoUniqueId != wgoUniqueId))
					{
						prebuiltParams = worldZonePrebuiltWgoParams;
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x04000C90 RID: 3216
	public string id;

	// Token: 0x04000C91 RID: 3217
	public string contentPartName;

	// Token: 0x04000C92 RID: 3218
	public Vector3 pos;

	// Token: 0x04000C93 RID: 3219
	public WorldZoneData.WorldZoneType worldZoneType;

	// Token: 0x04000C94 RID: 3220
	public LazyConsts.Navigation.Graph navigationGraph = LazyConsts.Navigation.Graph.None;

	// Token: 0x04000C95 RID: 3221
	public List<LazyConsts.Navigation.Graph> additionalMovementGraphs = new List<LazyConsts.Navigation.Graph>();

	// Token: 0x04000C96 RID: 3222
	public Rect wholeZoneRect;

	// Token: 0x04000C97 RID: 3223
	public List<WorldZonePrebuiltWgoParams> prebuiltWgoParams = new List<WorldZonePrebuiltWgoParams>();

	// Token: 0x04000C98 RID: 3224
	public List<WorldZoneElevationAreaBakedData> elevationAreas = new List<WorldZoneElevationAreaBakedData>();

	// Token: 0x04000C99 RID: 3225
	public List<WorldZoneNavigationHoleBakedData> navigationHoles = new List<WorldZoneNavigationHoleBakedData>();

	// Token: 0x04000C9A RID: 3226
	public int processingPriority;
}
