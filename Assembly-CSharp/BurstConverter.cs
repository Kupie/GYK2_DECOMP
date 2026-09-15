using System;
using UnityEngine;

// Token: 0x02000A95 RID: 2709
public static class BurstConverter
{
	// Token: 0x0600498A RID: 18826 RVA: 0x0015B6EC File Offset: 0x001598EC
	public static BurstableBounds ConvertBoundsToBurstable(Bounds bounds)
	{
		return new BurstableBounds(bounds.center, bounds.size);
	}

	// Token: 0x0600498B RID: 18827 RVA: 0x0015B70C File Offset: 0x0015990C
	public static BurstablePlane[] ConvertToBurstablePlanes(Plane[] planes)
	{
		BurstablePlane[] array = new BurstablePlane[planes.Length];
		BurstConverter.ConvertToBurstablePlanes(planes, array);
		return array;
	}

	// Token: 0x0600498C RID: 18828 RVA: 0x0015B72C File Offset: 0x0015992C
	public static void ConvertToBurstablePlanes(Plane[] planes, BurstablePlane[] result)
	{
		for (int i = 0; i < planes.Length; i++)
		{
			result[i] = new BurstablePlane
			{
				normal = planes[i].normal,
				distance = planes[i].distance
			};
		}
	}
}
