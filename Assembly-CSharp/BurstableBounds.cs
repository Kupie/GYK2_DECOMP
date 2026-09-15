using System;
using Unity.Mathematics;

// Token: 0x020006ED RID: 1773
[Serializable]
public struct BurstableBounds
{
	// Token: 0x17000745 RID: 1861
	// (get) Token: 0x06002ED7 RID: 11991 RVA: 0x000E022A File Offset: 0x000DE42A
	public float3 Min
	{
		get
		{
			return this.center - this.Extents;
		}
	}

	// Token: 0x17000746 RID: 1862
	// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x000E023D File Offset: 0x000DE43D
	public float3 Max
	{
		get
		{
			return this.center + this.Extents;
		}
	}

	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x000E0250 File Offset: 0x000DE450
	public float3 Extents
	{
		get
		{
			return this.size * 0.5f;
		}
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x000E0264 File Offset: 0x000DE464
	public bool Intersects(BurstableBounds bounds)
	{
		return (double)this.Min.x <= (double)bounds.Max.x && this.Max.x >= bounds.Min.x && this.Min.y <= bounds.Max.y && this.Max.y >= bounds.Min.y && this.Min.z <= bounds.Max.z && this.Max.z >= bounds.Min.z;
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x000E0311 File Offset: 0x000DE511
	public BurstableBounds(float3 center, float3 size)
	{
		this.center = center;
		this.size = size;
	}

	// Token: 0x040025D3 RID: 9683
	public float3 center;

	// Token: 0x040025D4 RID: 9684
	public float3 size;
}
