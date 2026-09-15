using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000622 RID: 1570
[ExecuteAlways]
public class DockPoint : MonoBehaviour
{
	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x060029CC RID: 10700 RVA: 0x000C52F3 File Offset: 0x000C34F3
	public Wgo Owner
	{
		get
		{
			WgoPart wgoPart = this.owner;
			if (wgoPart == null)
			{
				return null;
			}
			return wgoPart.Wgo;
		}
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x060029CD RID: 10701 RVA: 0x000C5306 File Offset: 0x000C3506
	public bool IsForZombie
	{
		get
		{
			return this.isForZombie;
		}
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x060029CE RID: 10702 RVA: 0x000C530E File Offset: 0x000C350E
	public bool HideInFighting
	{
		get
		{
			return this.hideInFighting;
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x060029CF RID: 10703 RVA: 0x000C5316 File Offset: 0x000C3516
	public DockPointTag DockPointTag
	{
		get
		{
			return this.dockPointTag;
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x060029D0 RID: 10704 RVA: 0x000C531E File Offset: 0x000C351E
	public bool DisableTargetingForCaretaker
	{
		get
		{
			return this.disableTargetingForCaretaker;
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x060029D1 RID: 10705 RVA: 0x000C5326 File Offset: 0x000C3526
	public bool DontUseForWorkerPlacement
	{
		get
		{
			return this.dontUseForWorkerPlacement;
		}
	}

	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000C5330 File Offset: 0x000C3530
	public Direction Direction
	{
		get
		{
			Direction direction = this.direction;
			if (base.transform.lossyScale.x < 0f && (direction == Direction.Left || direction == Direction.Right))
			{
				direction = direction.OppositeDir();
			}
			if (this.applyRotationToDirection)
			{
				direction = (Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f) * direction.ConvertToVector3()).ConvertFromVector3();
			}
			return direction;
		}
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x000C53AB File Offset: 0x000C35AB
	public void Init(WgoPart owner)
	{
		this.owner = owner;
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x000C53B4 File Offset: 0x000C35B4
	public bool IsReachable(float playerRadius)
	{
		foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(base.transform.position, playerRadius, 0))
		{
			if (!collider2D.isTrigger)
			{
				Wgo wgo = collider2D.GetComponent<Wgo>() ?? collider2D.GetComponentInParent<Wgo>();
				if (wgo == null || wgo.Data.UniqueId != this.owner.Wgo.Data.UniqueId)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x000C5437 File Offset: 0x000C3637
	public bool IsReachable(RecastGraph recastGraph)
	{
		return recastGraph.IsPointOnNavmesh(base.transform.position.XZ());
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x000C5454 File Offset: 0x000C3654
	public bool IsDockReached(Vector2 otherObjPosition)
	{
		return (base.gameObject.transform.position - otherObjPosition).magnitude <= 0.001f;
	}

	// Token: 0x040022D6 RID: 8918
	[SerializeField]
	public Direction direction;

	// Token: 0x040022D7 RID: 8919
	[SerializeField]
	private bool isForZombie;

	// Token: 0x040022D8 RID: 8920
	[SerializeField]
	private bool hideInFighting;

	// Token: 0x040022D9 RID: 8921
	[SerializeField]
	private DockPointTag dockPointTag;

	// Token: 0x040022DA RID: 8922
	[SerializeField]
	private bool applyRotationToDirection;

	// Token: 0x040022DB RID: 8923
	[SerializeField]
	private bool disableTargetingForCaretaker;

	// Token: 0x040022DC RID: 8924
	[SerializeField]
	private bool dontUseForWorkerPlacement;

	// Token: 0x040022DD RID: 8925
	[SerializeField]
	private WgoPart owner;
}
