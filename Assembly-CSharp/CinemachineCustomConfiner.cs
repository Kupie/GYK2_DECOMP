using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class CinemachineCustomConfiner : CinemachineExtension
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000912 RID: 2322 RVA: 0x0002E6CC File Offset: 0x0002C8CC
	public bool IsValid
	{
		get
		{
			return this.boundingCollider != null && this.boundingCollider.enabled && this.boundingCollider.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x17000169 RID: 361
	// (set) Token: 0x06000913 RID: 2323 RVA: 0x0002E6FB File Offset: 0x0002C8FB
	public Collider BoundingCollider
	{
		set
		{
			this.boundingCollider = value;
		}
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0002E704 File Offset: 0x0002C904
	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (this.IsValid && stage == this.applyOnStage)
		{
			Vector3 vector = this.ConfineScreenEdges(ref state);
			state.PositionCorrection += vector;
		}
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0002E744 File Offset: 0x0002C944
	private Vector3 ConfineScreenEdges(ref CameraState state)
	{
		Quaternion correctedOrientation = state.CorrectedOrientation;
		float orthographicSize = state.Lens.OrthographicSize;
		float num = orthographicSize * state.Lens.Aspect;
		Vector3 vector = correctedOrientation * Vector3.right * num;
		Vector3 vector2 = correctedOrientation * Vector3.up * orthographicSize;
		Vector3 vector3 = Vector3.zero;
		Vector3 correctedPosition = state.CorrectedPosition;
		Vector3 vector4 = correctedOrientation * Vector3.forward;
		Bounds lowFaceBoundsFromBoxCollider = this.GetLowFaceBoundsFromBoxCollider();
		bool flag = false;
		bool flag2 = false;
		Vector3 posOnYBoundsPlane = this.GetPosOnYBoundsPlane(correctedPosition, vector4, lowFaceBoundsFromBoxCollider);
		if (lowFaceBoundsFromBoxCollider.size.x <= num * 2f)
		{
			flag = true;
			vector3 += Vector3.right * (lowFaceBoundsFromBoxCollider.center.x - posOnYBoundsPlane.x);
		}
		if (lowFaceBoundsFromBoxCollider.size.z / 1.25f <= orthographicSize * 2f)
		{
			flag2 = true;
			vector3 += Vector3.forward * (lowFaceBoundsFromBoxCollider.center.z - posOnYBoundsPlane.z);
		}
		Vector3 vector5 = correctedPosition + vector2 + vector;
		Vector3 vector6 = correctedPosition - vector2 - vector;
		Vector3 vector7 = this.Clamp(vector5, vector4, lowFaceBoundsFromBoxCollider);
		Vector3 vector8 = this.Clamp(vector6, vector4, lowFaceBoundsFromBoxCollider);
		Vector3 vector9 = vector7 - vector5;
		Vector3 vector10 = vector8 - vector6;
		float num2 = (flag ? 0f : 1f);
		float num3 = (flag2 ? 0f : 1f);
		if (vector9.magnitude > 0.0001f)
		{
			vector3 += Vector3.Scale(vector9, new Vector3(num2, 1f, num3));
		}
		if (vector10.magnitude > 0.0001f)
		{
			vector3 += Vector3.Scale(vector10, new Vector3(num2, 1f, num3));
		}
		return vector3;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0002E91C File Offset: 0x0002CB1C
	private Vector3 Clamp(Vector3 linePos, Vector3 lineDir, Bounds yBounds)
	{
		float num = (yBounds.center.y - linePos.y) / lineDir.y;
		Vector3 vector = new Vector3(linePos.x + num * lineDir.x, yBounds.center.y, linePos.z + num * lineDir.z);
		if (!yBounds.Contains(vector))
		{
			return yBounds.ClosestPoint(vector) - num * lineDir;
		}
		return linePos;
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0002E998 File Offset: 0x0002CB98
	private Vector3 GetPosOnYBoundsPlane(Vector3 linePos, Vector3 lineDir, Bounds yBounds)
	{
		float num = (yBounds.center.y - linePos.y) / lineDir.y;
		return new Vector3(linePos.x + num * lineDir.x, yBounds.center.y, linePos.z + num * lineDir.z);
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
	private Bounds GetLowFaceBoundsFromBoxCollider()
	{
		Bounds bounds = default(Bounds);
		Bounds bounds2 = this.boundingCollider.bounds;
		bounds.center = new Vector3(bounds2.center.x, bounds2.center.y - bounds2.size.y / 2f, bounds2.center.z);
		bounds.size = new Vector3(bounds2.size.x, 0.01f, bounds2.size.z);
		return bounds;
	}

	// Token: 0x04000AC8 RID: 2760
	[SerializeField]
	private CinemachineCore.Stage applyOnStage;

	// Token: 0x04000AC9 RID: 2761
	[SerializeField]
	private Collider boundingCollider;
}
