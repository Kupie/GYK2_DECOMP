using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class EnemySpawnZone : MonoBehaviour
{
	// Token: 0x06001223 RID: 4643 RVA: 0x0005A140 File Offset: 0x00058340
	public static EnemySpawnZone Create()
	{
		return new GameObject().AddComponent<EnemySpawnZone>();
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x0005A14C File Offset: 0x0005834C
	public Vector3 GetRandomPosFromZone(out List<PathfindingPenalty> penalties)
	{
		penalties = (this.setWalkPenaltyTag ? this.pathfindingPenalties : null);
		if (this.colliders.Count == 0)
		{
			return base.transform.position;
		}
		Collider random = this.colliders.GetRandom<BoxCollider>();
		return this.GetInsidePoint(random, 32);
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x0005A19A File Offset: 0x0005839A
	public void ResetActiveState()
	{
		base.gameObject.SetActive(this.initialActiveState);
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x0005A1B0 File Offset: 0x000583B0
	private Vector3 GetInsidePoint(Collider collider, int maxAttempts = 32)
	{
		BoxCollider boxCollider = collider as BoxCollider;
		if (boxCollider != null)
		{
			Vector3 vector = new Vector3(global::UnityEngine.Random.Range(-boxCollider.size.x * 0.5f, boxCollider.size.x * 0.5f), global::UnityEngine.Random.Range(-boxCollider.size.y * 0.5f, boxCollider.size.y * 0.5f), global::UnityEngine.Random.Range(-boxCollider.size.z * 0.5f, boxCollider.size.z * 0.5f));
			Vector3 vector2 = boxCollider.transform.TransformPoint(boxCollider.center + vector);
			if (this.useYFromGO)
			{
				vector2.y = base.transform.position.y;
			}
			return vector2;
		}
		SphereCollider sphereCollider = collider as SphereCollider;
		if (sphereCollider != null)
		{
			Vector3 vector3 = sphereCollider.transform.TransformPoint(sphereCollider.center);
			Vector3 lossyScale = sphereCollider.transform.lossyScale;
			float num = sphereCollider.radius * Mathf.Max(new float[] { lossyScale.x, lossyScale.y, lossyScale.z });
			Vector3 vector4 = vector3 + global::UnityEngine.Random.insideUnitSphere * num;
			if (this.useYFromGO)
			{
				vector4.y = base.transform.position.y;
			}
			return vector4;
		}
		Bounds bounds = collider.bounds;
		for (int i = 0; i < maxAttempts; i++)
		{
			Vector3 vector5 = new Vector3(global::UnityEngine.Random.Range(bounds.min.x, bounds.max.x), global::UnityEngine.Random.Range(bounds.min.y, bounds.max.y), global::UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
			if ((collider.ClosestPoint(vector5) - vector5).magnitude.EqualsTo(0f, 1E-05f))
			{
				if (this.useYFromGO)
				{
					vector5.y = base.transform.position.y;
				}
				return vector5;
			}
		}
		Vector3 center = collider.bounds.center;
		if (this.useYFromGO)
		{
			center.y = base.transform.position.y;
		}
		return center;
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x0005A411 File Offset: 0x00058611
	private void Awake()
	{
		this.initialActiveState = base.gameObject.activeSelf;
	}

	// Token: 0x040013ED RID: 5101
	public string id;

	// Token: 0x040013EE RID: 5102
	public bool useYFromGO = true;

	// Token: 0x040013EF RID: 5103
	public List<BoxCollider> colliders = new List<BoxCollider>();

	// Token: 0x040013F0 RID: 5104
	private bool initialActiveState = true;

	// Token: 0x040013F1 RID: 5105
	[Header("Penalty")]
	public bool setWalkPenaltyTag;

	// Token: 0x040013F2 RID: 5106
	public List<PathfindingPenalty> pathfindingPenalties = new List<PathfindingPenalty>();

	// Token: 0x040013F3 RID: 5107
	[SerializeField]
	[Space]
	private bool hasCustomEnemyDataSet;

	// Token: 0x040013F4 RID: 5108
	public List<EnemyData> customEnemyData = new List<EnemyData>();
}
