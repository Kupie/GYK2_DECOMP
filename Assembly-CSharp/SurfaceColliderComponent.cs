using System;
using UnityEngine;

// Token: 0x02000134 RID: 308
[RequireComponent(typeof(Collider))]
public class SurfaceColliderComponent : MonoBehaviour, IOrderedSurface
{
	// Token: 0x17000122 RID: 290
	// (get) Token: 0x0600075B RID: 1883 RVA: 0x00023066 File Offset: 0x00021266
	// (set) Token: 0x0600075C RID: 1884 RVA: 0x0002306E File Offset: 0x0002126E
	public int Depth
	{
		get
		{
			return this.depth;
		}
		set
		{
			this.depth = value;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x0600075D RID: 1885 RVA: 0x00023077 File Offset: 0x00021277
	// (set) Token: 0x0600075E RID: 1886 RVA: 0x0002307F File Offset: 0x0002127F
	public SurfaceType SurfaceType
	{
		get
		{
			return this.surfaceType;
		}
		set
		{
			this.surfaceType = value;
		}
	}

	// Token: 0x04000946 RID: 2374
	[SerializeField]
	private int depth;

	// Token: 0x04000947 RID: 2375
	[SerializeField]
	private SurfaceType surfaceType;
}
