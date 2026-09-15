using System;
using Unity.Mathematics;

// Token: 0x020006EF RID: 1775
public struct BurstablePlane
{
	// Token: 0x06002EDE RID: 11998 RVA: 0x000E0351 File Offset: 0x000DE551
	public bool GetSide(float3 point)
	{
		return math.dot(this.normal, point) + this.distance > 0f;
	}

	// Token: 0x040025D7 RID: 9687
	public float3 normal;

	// Token: 0x040025D8 RID: 9688
	public float distance;
}
