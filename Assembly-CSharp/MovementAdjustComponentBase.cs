using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000362 RID: 866
public class MovementAdjustComponentBase : MonoBehaviour
{
	// Token: 0x0600170D RID: 5901 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Init(IMovable movable)
	{
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void DeInit()
	{
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void SetAdjustmentActive(bool active)
	{
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x0006D88C File Offset: 0x0006BA8C
	public void UpdatePos(Vector3 newPos)
	{
		RecastGraph sceneRecastGraph = MainGame.PlayerController.SceneRecastGraph;
		if (sceneRecastGraph != null)
		{
			NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
			walkable.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
			NNInfo nearest = sceneRecastGraph.GetNearest(newPos, walkable);
			if ((nearest.position.XZ() - newPos.XZ()).magnitude < 0.01f)
			{
				base.transform.position = new Vector3(newPos.x, nearest.position.y, newPos.z);
				return;
			}
		}
		base.transform.position = RaycastUtils.TrySnapToTheGround(newPos, 1f, 10f);
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x00002318 File Offset: 0x00000518
	protected virtual void UpdatePosIfMoving(Vector3 newPos)
	{
	}

	// Token: 0x040016FE RID: 5886
	private const float HIT_POINT_TO_CAST = 1f;

	// Token: 0x040016FF RID: 5887
	private const float HIT_MAX_DISTANCE = 10f;
}
