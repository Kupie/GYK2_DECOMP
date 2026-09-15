using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000734 RID: 1844
public class GraphCutUnit : MonoBehaviour
{
	// Token: 0x06003025 RID: 12325 RVA: 0x000E72E3 File Offset: 0x000E54E3
	public void Assign(SGuid holder)
	{
		this.holder = holder;
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x000E72EC File Offset: 0x000E54EC
	public void Release()
	{
		this.holder = SGuid.Empty;
		this.ClearCustomMesh();
		if (this.navmeshCut != null)
		{
			this.navmeshCut.enabled = false;
		}
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x000E7319 File Offset: 0x000E5519
	public void UpdateParameters(GraphMask graphMask, Vector3 center, Vector2 size)
	{
		this.navmeshCut.graphMask = graphMask;
		this.UpdateParameters(center, size);
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000E7330 File Offset: 0x000E5530
	public void UpdateParameters(Vector3 center, Vector2 size)
	{
		this.navmeshCut.type = NavmeshCut.MeshType.Box;
		this.navmeshCut.mesh = null;
		this.navmeshCut.transform.position = center;
		this.navmeshCut.rectangleSize = size;
		this.navmeshCut.height = 0.1f;
		this.navmeshCut.enabled = true;
		this.navmeshCut.ForceUpdate();
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000E7399 File Offset: 0x000E5599
	public void UpdateParameters(GraphMask graphMask, Vector3 center, float radius)
	{
		this.navmeshCut.graphMask = graphMask;
		this.UpdateParameters(center, radius);
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x000E73B0 File Offset: 0x000E55B0
	public void UpdateParameters(Vector3 center, float radius)
	{
		this.navmeshCut.type = NavmeshCut.MeshType.Sphere;
		this.navmeshCut.mesh = null;
		this.navmeshCut.transform.position = center;
		this.navmeshCut.circleRadius = radius;
		this.navmeshCut.height = 0.1f;
		this.navmeshCut.enabled = true;
		this.navmeshCut.ForceUpdate();
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x000E7419 File Offset: 0x000E5619
	public void UpdateParameters(GraphMask graphMask, Vector3 center, WgoPartBakedData.PlannerMeshData plannerMeshData)
	{
		this.navmeshCut.graphMask = graphMask;
		this.UpdateParameters(center, plannerMeshData);
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x000E7430 File Offset: 0x000E5630
	public void UpdateParameters(Vector3 center, WgoPartBakedData.PlannerMeshData plannerMeshData)
	{
		Mesh mesh;
		if (!WgoPartBakedData.TryCreateMesh(plannerMeshData, string.Format("PlannerCut_{0}_{1}", this.holder, (plannerMeshData != null) ? new int?(plannerMeshData.hash) : null), out mesh))
		{
			return;
		}
		this.ClearCustomMesh();
		this.customCutMesh = mesh;
		this.navmeshCut.type = NavmeshCut.MeshType.CustomMesh;
		this.navmeshCut.transform.position = center;
		this.navmeshCut.center = Vector3.zero;
		this.navmeshCut.meshScale = 1f;
		this.navmeshCut.height = 0.1f;
		this.navmeshCut.mesh = this.customCutMesh;
		this.navmeshCut.enabled = true;
		this.navmeshCut.ForceUpdate();
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x000E74F8 File Offset: 0x000E56F8
	private void ClearCustomMesh()
	{
		if (this.navmeshCut != null)
		{
			this.navmeshCut.mesh = null;
		}
		if (this.customCutMesh == null)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(this.customCutMesh);
		this.customCutMesh = null;
	}

	// Token: 0x04002701 RID: 9985
	private const float Y_SIZE = 0.1f;

	// Token: 0x04002702 RID: 9986
	private Mesh customCutMesh;

	// Token: 0x04002703 RID: 9987
	public NavmeshCut navmeshCut;

	// Token: 0x04002704 RID: 9988
	public SGuid holder = SGuid.Empty;
}
