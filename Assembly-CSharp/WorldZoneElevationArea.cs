using System;
using UnityEngine;

// Token: 0x020001C8 RID: 456
[RequireComponent(typeof(BoxCollider))]
public class WorldZoneElevationArea : MonoBehaviour
{
	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0003B3C9 File Offset: 0x000395C9
	public bool ShowGrid
	{
		get
		{
			return this.showGrid;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0003B3D1 File Offset: 0x000395D1
	public bool ShowGroundFootprint
	{
		get
		{
			return this.showGroundFootprint;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0003B3D9 File Offset: 0x000395D9
	public BoxCollider FootprintCollider
	{
		get
		{
			return this.footprintCollider;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0003B3E4 File Offset: 0x000395E4
	public float ElevationY
	{
		get
		{
			if (!(this.footprintCollider != null))
			{
				return base.transform.position.y;
			}
			return this.footprintCollider.bounds.center.y;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0003B428 File Offset: 0x00039628
	public float GroundPlaneY
	{
		get
		{
			WorldZone componentInParent = base.GetComponentInParent<WorldZone>();
			if (!(componentInParent != null))
			{
				return 0f;
			}
			return componentInParent.transform.position.y;
		}
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0003B45C File Offset: 0x0003965C
	public Rect GetXZRect()
	{
		if (this.footprintCollider == null)
		{
			return default(Rect);
		}
		Bounds bounds = this.footprintCollider.bounds;
		float num = (bounds.center.y - this.GroundPlaneY) * 0.75f;
		return new Rect(bounds.min.x, bounds.min.z + num, bounds.size.x, bounds.size.z);
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0003B4E0 File Offset: 0x000396E0
	public bool ContainsXZ(Vector2 xz)
	{
		return this.GetXZRect().Contains(xz);
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0003B4FC File Offset: 0x000396FC
	public WorldZoneElevationAreaBakedData ToBakedData()
	{
		return new WorldZoneElevationAreaBakedData
		{
			xzRect = this.GetXZRect(),
			elevationY = this.ElevationY
		};
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0003B52C File Offset: 0x0003972C
	private void Awake()
	{
		if (this.footprintCollider == null)
		{
			base.TryGetComponent<BoxCollider>(out this.footprintCollider);
		}
		if (this.footprintCollider != null)
		{
			this.footprintCollider.isTrigger = true;
			Vector3 size = this.footprintCollider.size;
			if (size.y > 0.02f)
			{
				this.footprintCollider.size = new Vector3(size.x, 0.01f, size.z);
			}
		}
	}

	// Token: 0x04000CC0 RID: 3264
	[SerializeField]
	private BoxCollider footprintCollider;

	// Token: 0x04000CC1 RID: 3265
	[SerializeField]
	[Tooltip("Point whose projected build-grid phase the footprint aligns to (camera perspective). Optional.")]
	private Transform elevationReference;

	// Token: 0x04000CC2 RID: 3266
	[SerializeField]
	private bool showGrid = true;

	// Token: 0x04000CC3 RID: 3267
	[SerializeField]
	private bool showGroundFootprint;
}
