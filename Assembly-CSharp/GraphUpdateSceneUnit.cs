using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000735 RID: 1845
public class GraphUpdateSceneUnit : MonoBehaviour
{
	// Token: 0x0600302F RID: 12335 RVA: 0x000E7548 File Offset: 0x000E5748
	public void Assign(SGuid holder)
	{
		this.holder = holder;
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x000E7551 File Offset: 0x000E5751
	public void Release()
	{
		if (this.graphUpdateApplied)
		{
			this.ApplyGraphUpdate(this.graphUpdateMask, !this.graphUpdateSetWalkability, this.graphUpdateUpdatePhysics, -this.graphUpdatePenaltyDelta);
			this.graphUpdateApplied = false;
		}
		this.holder = SGuid.Empty;
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x000E758F File Offset: 0x000E578F
	public void UpdateParameters(GraphMask graphMask, Vector3 center, WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData)
	{
		this.graphUpdateMask = graphMask;
		this.UpdateParameters(center, graphUpdateSceneBoxData);
	}

	// Token: 0x06003032 RID: 12338 RVA: 0x000E75A0 File Offset: 0x000E57A0
	public void UpdateParameters(Vector3 center, WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData)
	{
		if (graphUpdateSceneBoxData == null)
		{
			return;
		}
		this.EnsureComponents();
		if (this.graphUpdateBoxCollider == null || this.graphUpdateScene == null)
		{
			return;
		}
		this.graphUpdateSetWalkability = graphUpdateSceneBoxData.setWalkability;
		this.graphUpdateUpdatePhysics = graphUpdateSceneBoxData.updatePhysics;
		this.graphUpdatePenaltyDelta = graphUpdateSceneBoxData.penaltyDelta;
		this.graphUpdateApplied = true;
		base.transform.position = center + graphUpdateSceneBoxData.localCenter;
		this.graphUpdateBoxCollider.center = Vector3.zero;
		this.graphUpdateBoxCollider.size = new Vector3(Mathf.Max(graphUpdateSceneBoxData.size.x, 0.01f), Mathf.Max(graphUpdateSceneBoxData.size.y, 0.01f), Mathf.Max(graphUpdateSceneBoxData.size.z, 0.01f));
		this.ApplyGraphUpdate(this.graphUpdateMask, this.graphUpdateSetWalkability, this.graphUpdateUpdatePhysics, this.graphUpdatePenaltyDelta);
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x000E7694 File Offset: 0x000E5894
	private void EnsureComponents()
	{
		if (this.graphUpdateBoxCollider == null)
		{
			base.TryGetComponent<BoxCollider>(out this.graphUpdateBoxCollider);
			if (this.graphUpdateBoxCollider == null)
			{
				this.graphUpdateBoxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
		}
		if (this.graphUpdateScene == null)
		{
			base.TryGetComponent<GraphUpdateScene>(out this.graphUpdateScene);
			if (this.graphUpdateScene == null)
			{
				this.graphUpdateScene = base.gameObject.AddComponent<GraphUpdateScene>();
			}
		}
	}

	// Token: 0x06003034 RID: 12340 RVA: 0x000E7718 File Offset: 0x000E5918
	private void ApplyGraphUpdate(GraphMask graphMask, bool setWalkability, bool updatePhysics, int penaltyDelta = 0)
	{
		if (this.graphUpdateScene == null || AstarPath.active == null)
		{
			return;
		}
		this.graphUpdateScene.applyOnStart = true;
		this.graphUpdateScene.applyOnScan = true;
		this.graphUpdateScene.modifyWalkability = true;
		this.graphUpdateScene.setWalkability = setWalkability;
		this.graphUpdateScene.updatePhysics = updatePhysics;
		this.graphUpdateScene.updateErosion = true;
		this.graphUpdateScene.resetPenaltyOnPhysics = true;
		this.graphUpdateScene.penaltyDelta = penaltyDelta;
		GraphUpdateObject graphUpdate = this.graphUpdateScene.GetGraphUpdate();
		if (graphUpdate == null)
		{
			return;
		}
		graphUpdate.graphMask = graphMask;
		AstarPath.active.UpdateGraphs(graphUpdate);
	}

	// Token: 0x04002705 RID: 9989
	private const float MIN_BOX_AXIS = 0.01f;

	// Token: 0x04002706 RID: 9990
	private BoxCollider graphUpdateBoxCollider;

	// Token: 0x04002707 RID: 9991
	private GraphUpdateScene graphUpdateScene;

	// Token: 0x04002708 RID: 9992
	private GraphMask graphUpdateMask = GraphMask.everything;

	// Token: 0x04002709 RID: 9993
	private bool graphUpdateApplied;

	// Token: 0x0400270A RID: 9994
	private bool graphUpdateSetWalkability;

	// Token: 0x0400270B RID: 9995
	private bool graphUpdateUpdatePhysics;

	// Token: 0x0400270C RID: 9996
	[SerializeField]
	private int graphUpdatePenaltyDelta;

	// Token: 0x0400270D RID: 9997
	public SGuid holder = SGuid.Empty;
}
