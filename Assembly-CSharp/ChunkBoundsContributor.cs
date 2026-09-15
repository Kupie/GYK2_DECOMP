using System;
using UnityEngine;

// Token: 0x020006F4 RID: 1780
[DisallowMultipleComponent]
public class ChunkBoundsContributor : MonoBehaviour
{
	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06002F06 RID: 12038 RVA: 0x000E0AA6 File Offset: 0x000DECA6
	public bool UseSeparateBounds
	{
		get
		{
			return this.useSeparateBounds;
		}
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x000E0AAE File Offset: 0x000DECAE
	public bool TryGetBounds(out Bounds worldBounds)
	{
		if (this.useSeparateBounds)
		{
			worldBounds = default(Bounds);
			return false;
		}
		return this.TryGetWorldBounds(this.bounds, out worldBounds);
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x000E0ACE File Offset: 0x000DECCE
	public bool TryGetBoundsWithShadows(out Bounds worldBounds)
	{
		return this.TryGetWorldBounds(this.useSeparateBounds ? this.boundsWithShadows : this.bounds, out worldBounds);
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x000E0AED File Offset: 0x000DECED
	public bool TryGetBoundsWithoutShadows(out Bounds worldBounds)
	{
		return this.TryGetWorldBounds(this.useSeparateBounds ? this.boundsWithoutShadows : this.bounds, out worldBounds);
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x000E0B0C File Offset: 0x000DED0C
	private bool TryGetWorldBounds(Bounds localBounds, out Bounds worldBounds)
	{
		worldBounds = default(Bounds);
		if (localBounds.size.sqrMagnitude <= 0f)
		{
			return false;
		}
		worldBounds = this.TransformLocalBounds(localBounds);
		return true;
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x000E0B48 File Offset: 0x000DED48
	private Bounds TransformLocalBounds(Bounds localBounds)
	{
		Transform transform = base.transform;
		Vector3 vector = transform.TransformPoint(localBounds.center);
		Vector3 extents = localBounds.extents;
		Vector3 vector2 = transform.TransformVector(new Vector3(extents.x, 0f, 0f));
		Vector3 vector3 = transform.TransformVector(new Vector3(0f, extents.y, 0f));
		Vector3 vector4 = transform.TransformVector(new Vector3(0f, 0f, extents.z));
		Vector3 vector5 = new Vector3(Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x) + Mathf.Abs(vector4.x), Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y) + Mathf.Abs(vector4.y), Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z) + Mathf.Abs(vector4.z));
		return new Bounds(vector, vector5 * 2f);
	}

	// Token: 0x040025F2 RID: 9714
	public static readonly Color gizmoBoundsColor = new Color(1f, 0.5f, 0f, 0.85f);

	// Token: 0x040025F3 RID: 9715
	public static readonly Color gizmoWithShadowsColor = new Color(1f, 0.85f, 0f, 0.85f);

	// Token: 0x040025F4 RID: 9716
	[SerializeField]
	[Tooltip("When enabled, With Shadows and Without Shadows use separate bounds.")]
	private bool useSeparateBounds;

	// Token: 0x040025F5 RID: 9717
	[SerializeField]
	[Tooltip("Local bounds merged into both chunk bounds entries. Zero size = ignored.")]
	private Bounds bounds = new Bounds(Vector3.zero, Vector3.one);

	// Token: 0x040025F6 RID: 9718
	[SerializeField]
	[Tooltip("Local bounds for with-shadows chunk bounds only. Zero size = ignored.")]
	private Bounds boundsWithShadows = new Bounds(Vector3.zero, Vector3.one);

	// Token: 0x040025F7 RID: 9719
	[SerializeField]
	[Tooltip("Local bounds for without-shadows chunk bounds only. Zero size = ignored.")]
	private Bounds boundsWithoutShadows = new Bounds(Vector3.zero, Vector3.one);
}
