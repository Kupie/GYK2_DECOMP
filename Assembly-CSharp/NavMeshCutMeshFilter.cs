using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.ProBuilder;

// Token: 0x02000A8D RID: 2701
[DefaultExecutionOrder(1)]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(NavmeshCut))]
public class NavMeshCutMeshFilter : CustomNavMeshCutBakeSourceBase
{
	// Token: 0x06004970 RID: 18800 RVA: 0x0015AE88 File Offset: 0x00159088
	private void Start()
	{
		if (!this.navmeshCut)
		{
			base.TryGetComponent<NavmeshCut>(out this.navmeshCut);
		}
		if (!this.navmeshCut)
		{
			Debug.LogError("NavMeshCutMeshFilter is missing a required component: " + base.name);
			return;
		}
		NavMeshCutMeshFilter.NavMeshCutMeshFilterMode navMeshCutMeshFilterMode = this.mode;
		if (navMeshCutMeshFilterMode != NavMeshCutMeshFilter.NavMeshCutMeshFilterMode.MeshFilter)
		{
			if (navMeshCutMeshFilterMode == NavMeshCutMeshFilter.NavMeshCutMeshFilterMode.ProBuilderMesh)
			{
				if (!this.proBuilderMesh)
				{
					Debug.LogError("NavMeshCutMeshFilter is missing a required component: " + base.name);
					this.navmeshCut.enabled = false;
					return;
				}
				this.navmeshCut.mesh = this.proBuilderMesh.mesh;
			}
		}
		else
		{
			if (!this.meshFilter)
			{
				Debug.LogError("NavMeshCutMeshFilter is missing a required component: " + base.name);
				this.navmeshCut.enabled = false;
				return;
			}
			this.navmeshCut.mesh = this.meshFilter.mesh;
		}
		this.navmeshCut.type = NavmeshCut.MeshType.CustomMesh;
		this.navmeshCut.enabled = true;
		this.navmeshCut.ForceUpdate();
	}

	// Token: 0x04003945 RID: 14661
	[SerializeField]
	private NavMeshCutMeshFilter.NavMeshCutMeshFilterMode mode;

	// Token: 0x04003946 RID: 14662
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x04003947 RID: 14663
	[SerializeField]
	private ProBuilderMesh proBuilderMesh;

	// Token: 0x04003948 RID: 14664
	[SerializeField]
	private NavmeshCut navmeshCut;

	// Token: 0x02000A8E RID: 2702
	public enum NavMeshCutMeshFilterMode
	{
		// Token: 0x0400394A RID: 14666
		MeshFilter,
		// Token: 0x0400394B RID: 14667
		ProBuilderMesh
	}
}
