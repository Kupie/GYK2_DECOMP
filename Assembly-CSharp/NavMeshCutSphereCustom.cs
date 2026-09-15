using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000A90 RID: 2704
[DefaultExecutionOrder(1)]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(NavmeshCut))]
[ExecuteAlways]
public class NavMeshCutSphereCustom : CustomNavMeshCutBakeSourceBase
{
	// Token: 0x0600497D RID: 18813 RVA: 0x0015B24A File Offset: 0x0015944A
	private new void Awake()
	{
		base.Awake();
		if (this.navmeshCut)
		{
			this.navmeshCut.enabled = !this.dontUseCut;
		}
	}

	// Token: 0x0600497E RID: 18814 RVA: 0x0015B274 File Offset: 0x00159474
	private void UpdateNavMeshCut()
	{
		if (this.navmeshCut == null)
		{
			return;
		}
		this.navmeshCut.type = NavmeshCut.MeshType.Circle;
		this.navmeshCut.center = this.sphereCollider.center.XZ();
		this.navmeshCut.circleRadius = this.sphereCollider.radius;
		this.navmeshCut.height = this.sphereCollider.radius * 2f;
	}

	// Token: 0x04003951 RID: 14673
	[SerializeField]
	private SphereCollider sphereCollider;

	// Token: 0x04003952 RID: 14674
	[SerializeField]
	private NavmeshCut navmeshCut;

	// Token: 0x04003953 RID: 14675
	public bool dontUseCut;
}
