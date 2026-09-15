using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class IndoorArea : MonoBehaviour
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0003621E File Offset: 0x0003441E
	public string Id
	{
		get
		{
			if (!string.IsNullOrEmpty(this.id))
			{
				return this.id;
			}
			return base.name;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x0003623A File Offset: 0x0003443A
	public IReadOnlyList<BoxCollider> Colliders
	{
		get
		{
			return this.colliders;
		}
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x00036244 File Offset: 0x00034444
	public IndoorAreaData ToData()
	{
		IndoorAreaData indoorAreaData = new IndoorAreaData
		{
			id = this.Id,
			bounds = new List<IndoorAreaBoundData>()
		};
		if (this.colliders == null)
		{
			return indoorAreaData;
		}
		for (int i = 0; i < this.colliders.Count; i++)
		{
			BoxCollider boxCollider = this.colliders[i];
			if (!(boxCollider == null))
			{
				indoorAreaData.bounds.Add(new IndoorAreaBoundData(boxCollider));
			}
		}
		return indoorAreaData;
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x000362B8 File Offset: 0x000344B8
	public bool ContainsXZ(Vector3 worldPos)
	{
		if (this.colliders == null)
		{
			return false;
		}
		for (int i = 0; i < this.colliders.Count; i++)
		{
			BoxCollider boxCollider = this.colliders[i];
			if (!(boxCollider == null) && new IndoorAreaBoundData(boxCollider).ContainsXZ(worldPos))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000C2F RID: 3119
	[SerializeField]
	private string id;

	// Token: 0x04000C30 RID: 3120
	[SerializeField]
	private List<BoxCollider> colliders = new List<BoxCollider>();
}
