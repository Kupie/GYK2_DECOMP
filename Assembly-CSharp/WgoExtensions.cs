using System;
using UnityEngine;

// Token: 0x02000ACF RID: 2767
public static class WgoExtensions
{
	// Token: 0x06004AB0 RID: 19120 RVA: 0x00160890 File Offset: 0x0015EA90
	public static bool TryGetNearestBuilderWorldZone(this Wgo builderWgo, out WorldZone worldZone)
	{
		worldZone = null;
		if (!builderWgo)
		{
			return false;
		}
		int num = Physics.OverlapBoxNonAlloc(builderWgo.transform.position, new Vector3(10f, 10f, 10f), WgoExtensions.overlapColliders, Quaternion.identity, 131072);
		if (num == 0)
		{
			return false;
		}
		float num2 = float.PositiveInfinity;
		WorldZone worldZone2 = null;
		Vector3 position = builderWgo.transform.position;
		for (int i = 0; i < num; i++)
		{
			Collider collider = WgoExtensions.overlapColliders[i];
			WorldZone worldZone3;
			if (collider && collider.TryGetComponent<WorldZone>(out worldZone3) && WgoExtensions.IsBuilderForWorldZone(builderWgo, worldZone3))
			{
				if (collider.bounds.Contains(position))
				{
					worldZone = worldZone3;
					return true;
				}
				float num3 = collider.bounds.SqrDistance(position);
				if (num2 > num3)
				{
					num2 = num3;
					worldZone2 = worldZone3;
				}
			}
		}
		if (!worldZone2)
		{
			return false;
		}
		worldZone = worldZone2;
		return true;
	}

	// Token: 0x06004AB1 RID: 19121 RVA: 0x00160974 File Offset: 0x0015EB74
	private static bool IsBuilderForWorldZone(Wgo builderWgo, WorldZone worldZone)
	{
		if (!builderWgo || worldZone == null)
		{
			return false;
		}
		WorldZoneDef dataOrNull = GameBalance.Me.GetDataOrNull<WorldZoneDef>(worldZone.Id);
		return dataOrNull != null && !string.IsNullOrEmpty(dataOrNull.builderId) && dataOrNull.builderId == builderWgo.Id;
	}

	// Token: 0x04003A79 RID: 14969
	private const float COLLIDER_BOX_SEARCH_HALF_SIZE = 10f;

	// Token: 0x04003A7A RID: 14970
	private static readonly Collider[] overlapColliders = new Collider[30];
}
