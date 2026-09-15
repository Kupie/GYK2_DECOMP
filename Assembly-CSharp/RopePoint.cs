using System;
using UnityEngine;

// Token: 0x02000547 RID: 1351
[ExecuteInEditMode]
public class RopePoint : MonoBehaviour
{
	// Token: 0x060022A7 RID: 8871 RVA: 0x000A2963 File Offset: 0x000A0B63
	private void Awake()
	{
		this.parentRope = base.GetComponentInParent<RopeRenderer>();
		if (this.parentRope == null)
		{
			Debug.LogWarning("RopePoint '" + base.gameObject.name + "' is not a child of a RopeRenderer!");
		}
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x000A299E File Offset: 0x000A0B9E
	private void Start()
	{
		this.UpdateCachedPosition();
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000A29A6 File Offset: 0x000A0BA6
	private void Update()
	{
		if (base.transform.hasChanged)
		{
			this.positionDirty = true;
			base.transform.hasChanged = false;
		}
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x000A29C8 File Offset: 0x000A0BC8
	public Vector3 GetWorldPosition()
	{
		if (this.positionDirty)
		{
			this.UpdateCachedPosition();
		}
		return this.cachedWorldPosition;
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000A29DE File Offset: 0x000A0BDE
	public Vector3 GetLocalOffset()
	{
		return this.localOffset;
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x000A29E6 File Offset: 0x000A0BE6
	public void SetLocalOffset(Vector3 offset)
	{
		this.localOffset = offset;
		this.positionDirty = true;
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000A29F6 File Offset: 0x000A0BF6
	public bool IsStartPoint()
	{
		return this.isStartPoint;
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x000A29FE File Offset: 0x000A0BFE
	public void SetAsStartPoint(bool isStart)
	{
		this.isStartPoint = isStart;
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000A2A07 File Offset: 0x000A0C07
	private void UpdateCachedPosition()
	{
		this.cachedWorldPosition = base.transform.position + base.transform.TransformDirection(this.localOffset);
		this.positionDirty = false;
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000A2A37 File Offset: 0x000A0C37
	public void SetGizmoSettings(bool show, Color color, float size)
	{
		this.showGizmo = show;
		this.gizmoColor = color;
		this.gizmoSize = size;
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x000A2A50 File Offset: 0x000A0C50
	private void OnDrawGizmos()
	{
		if (!this.showGizmo)
		{
			return;
		}
		Gizmos.color = this.gizmoColor;
		Gizmos.DrawSphere(base.transform.position + base.transform.TransformDirection(this.localOffset), this.gizmoSize);
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000A2AA0 File Offset: 0x000A0CA0
	private void OnDrawGizmosSelected()
	{
		if (!this.showGizmo)
		{
			return;
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(base.transform.position + base.transform.TransformDirection(this.localOffset), this.gizmoSize * 1.5f);
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000A2AF2 File Offset: 0x000A0CF2
	private void OnValidate()
	{
		this.positionDirty = true;
		if (this.isStartPoint)
		{
			this.gizmoColor = Color.green;
			return;
		}
		this.gizmoColor = Color.red;
	}

	// Token: 0x04001F53 RID: 8019
	[Header("Point Settings")]
	[SerializeField]
	private bool isStartPoint = true;

	// Token: 0x04001F54 RID: 8020
	[SerializeField]
	private Vector3 localOffset = Vector3.zero;

	// Token: 0x04001F55 RID: 8021
	[Header("Gizmo Settings")]
	[SerializeField]
	private bool showGizmo = true;

	// Token: 0x04001F56 RID: 8022
	[SerializeField]
	private Color gizmoColor = Color.red;

	// Token: 0x04001F57 RID: 8023
	[SerializeField]
	private float gizmoSize = 0.1f;

	// Token: 0x04001F58 RID: 8024
	private RopeRenderer parentRope;

	// Token: 0x04001F59 RID: 8025
	private Vector3 cachedWorldPosition;

	// Token: 0x04001F5A RID: 8026
	private bool positionDirty = true;
}
