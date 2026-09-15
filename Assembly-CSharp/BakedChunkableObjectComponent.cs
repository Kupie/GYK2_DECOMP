using System;
using UnityEngine;

// Token: 0x020006E4 RID: 1764
public abstract class BakedChunkableObjectComponent : MonoBehaviour
{
	// Token: 0x06002EA3 RID: 11939
	public abstract BakedChunkableObjectComponentData GetData();

	// Token: 0x06002EA4 RID: 11940
	public abstract void SetData(BakedChunkableObjectComponentData data);

	// Token: 0x06002EA5 RID: 11941 RVA: 0x000DF4C0 File Offset: 0x000DD6C0
	public virtual void BakeData()
	{
		BakedChunkableObjectComponentData data = this.GetData();
		ChunkBoundsPair chunkBoundsPair = ChunkSizeCalculator.CalculateChunkBounds(base.gameObject);
		data.lossyScale = base.transform.lossyScale;
		data.rotation = base.transform.rotation;
		data.worldPos = base.transform.position;
		data.chunkBounds = new BurstableChunkBoundsPair(new BurstableBounds(chunkBoundsPair.withShadows.center + base.transform.position - base.transform.position, chunkBoundsPair.withShadows.size), new BurstableBounds(chunkBoundsPair.withoutShadows.center + base.transform.position - base.transform.position, chunkBoundsPair.withoutShadows.size));
		GDPoint componentInParent = base.GetComponentInParent<GDPoint>(true);
		data.parentGdPointId = ((componentInParent != null) ? componentInParent.Id : null);
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x000DF5CA File Offset: 0x000DD7CA
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		this.GetData().DrawChunkGizmos();
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x000DF5E4 File Offset: 0x000DD7E4
	public virtual void ApplyData()
	{
		BakedChunkableObjectComponentData data = this.GetData();
		base.transform.rotation = data.rotation;
		base.transform.position = data.worldPos;
		base.transform.localScale = data.lossyScale;
	}
}
