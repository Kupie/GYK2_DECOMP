using System;
using UnityEngine;

// Token: 0x02000691 RID: 1681
[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider))]
public class WgoPartGraphUpdateSceneBoxMarker : MonoBehaviour
{
	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x06002D22 RID: 11554 RVA: 0x000D758B File Offset: 0x000D578B
	public BoxCollider SourceBoxCollider
	{
		get
		{
			if (this.sourceBoxCollider == null)
			{
				base.TryGetComponent<BoxCollider>(out this.sourceBoxCollider);
			}
			return this.sourceBoxCollider;
		}
	}

	// Token: 0x06002D23 RID: 11555 RVA: 0x000D75B0 File Offset: 0x000D57B0
	public WgoPartBakedData.GraphUpdateSceneBoxData ToBakedData(Transform rootTransform, bool mirror)
	{
		BoxCollider boxCollider = this.SourceBoxCollider;
		if (boxCollider == null)
		{
			return null;
		}
		Bounds bounds = boxCollider.bounds;
		Vector3 vector = rootTransform.InverseTransformPoint(bounds.center);
		if (mirror)
		{
			vector.x = -vector.x;
		}
		WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData = new WgoPartBakedData.GraphUpdateSceneBoxData();
		graphUpdateSceneBoxData.localCenter = vector;
		graphUpdateSceneBoxData.size = bounds.size;
		graphUpdateSceneBoxData.setWalkability = this.setWalkability;
		graphUpdateSceneBoxData.updatePhysics = this.updatePhysics;
		graphUpdateSceneBoxData.SetPenaltyDelta(this.penaltyDelta);
		return graphUpdateSceneBoxData;
	}

	// Token: 0x06002D24 RID: 11556 RVA: 0x00027874 File Offset: 0x00025A74
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400240C RID: 9228
	[SerializeField]
	private BoxCollider sourceBoxCollider;

	// Token: 0x0400240D RID: 9229
	[SerializeField]
	private bool setWalkability = true;

	// Token: 0x0400240E RID: 9230
	[SerializeField]
	private bool updatePhysics;

	// Token: 0x0400240F RID: 9231
	[SerializeField]
	private int penaltyDelta;
}
