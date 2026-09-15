using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000A8C RID: 2700
[DefaultExecutionOrder(1)]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(NavmeshCut))]
[ExecuteAlways]
public class NavMeshCutBoxCustom : CustomNavMeshCutBakeSourceBase
{
	// Token: 0x0600496D RID: 18797 RVA: 0x0015ADA8 File Offset: 0x00158FA8
	private new void Awake()
	{
		base.Awake();
		if (this.navmeshCut)
		{
			this.navmeshCut.enabled = !this.dontUseCut;
		}
	}

	// Token: 0x0600496E RID: 18798 RVA: 0x0015ADD4 File Offset: 0x00158FD4
	private void UpdateNavMeshCut()
	{
		if (this.navmeshCut == null)
		{
			return;
		}
		this.navmeshCut.center = this.boxCollider.center.XZ();
		this.navmeshCut.rectangleSize.x = this.boxCollider.size.x;
		this.navmeshCut.rectangleSize.y = this.boxCollider.size.z;
		this.navmeshCut.height = this.boxCollider.size.y;
		this.navmeshCut.radiusExpansionMode = this.radiusExpansionMode;
	}

	// Token: 0x04003941 RID: 14657
	[SerializeField]
	private BoxCollider boxCollider;

	// Token: 0x04003942 RID: 14658
	[SerializeField]
	private NavmeshCut navmeshCut;

	// Token: 0x04003943 RID: 14659
	public bool dontUseCut;

	// Token: 0x04003944 RID: 14660
	public NavmeshCut.RadiusExpansionMode radiusExpansionMode = NavmeshCut.RadiusExpansionMode.ExpandByAgentRadius;
}
