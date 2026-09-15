using System;
using UnityEngine;

// Token: 0x02000AFE RID: 2814
public static class RaycastUtils
{
	// Token: 0x06004B04 RID: 19204 RVA: 0x00161EBC File Offset: 0x001600BC
	public static Vector3 TrySnapToTheGround(Vector3 posToSnapOn, float hitPointCast, float hitMaxDistance)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(posToSnapOn + Vector3.up * hitPointCast, Vector3.down), out raycastHit, hitMaxDistance, 6144))
		{
			posToSnapOn = new Vector3(posToSnapOn.x, raycastHit.point.y, posToSnapOn.z);
		}
		return posToSnapOn;
	}

	// Token: 0x04003C8B RID: 15499
	private const int HIT_LAYER_MASK = 6144;
}
