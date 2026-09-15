using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002BC RID: 700
[Serializable]
public class SortedDecals
{
	// Token: 0x060011E7 RID: 4583 RVA: 0x00059720 File Offset: 0x00057920
	public GroundDecal AddDecal<T>(Vector3 position, Direction orientation, Pool decalPool, DecalLayerBand layerBand, GroundDecalsCollection ownerCollection, string customDeathEffectId = "") where T : GroundDecal
	{
		Vector3 vector = RaycastUtils.TrySnapToTheGround(position, 1f, 10f);
		vector += -Vector3.up * -0.005f;
		int num = Mathf.RoundToInt(vector.x / 1.76f);
		int num2 = Mathf.RoundToInt(vector.z / 1.76f);
		this.CollectOverlapping(num, num2, vector, layerBand, this.overlapBuffer);
		this.CullCoveredDecals(vector, this.overlapBuffer);
		int num3 = this.PickDepthLayer(layerBand, this.overlapBuffer);
		this.overlapBuffer.Clear();
		T orCreateObject = decalPool.GetOrCreateObject<T>();
		orCreateObject.SetPlacementMetadata(num, num2, num3, layerBand, ownerCollection, vector);
		orCreateObject.SpawnDecal(vector + VisualConsts.GetFightDecalLayerOffset(num3), orientation, customDeathEffectId);
		this.GetOrCreateCell(num, num2).Add(orCreateObject);
		this.spawnOrder.Add(orCreateObject);
		this.EnforceGlobalBudget();
		return orCreateObject;
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x00059820 File Offset: 0x00057A20
	public void RemoveDecal(GroundDecal decal, Pool decalPool)
	{
		this.Unregister(decal);
		GroundDecalsCollection ownerCollection = decal.OwnerCollection;
		decal.ClearPlacementMetadata();
		Pool pool = decalPool;
		if (ownerCollection != null)
		{
			ownerCollection.RemoveFromActive(decal);
			pool = ownerCollection.DecalPool ?? decalPool;
		}
		if (pool != null)
		{
			pool.ReleaseObject<GroundDecal>(decal);
		}
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x0005986C File Offset: 0x00057A6C
	private int PickDepthLayer(DecalLayerBand layerBand, List<GroundDecal> overlapping)
	{
		int layerMin = SortedDecals.GetLayerMin(layerBand);
		int slotCount = SortedDecals.GetSlotCount(layerBand);
		uint num = 0U;
		for (int i = 0; i < overlapping.Count; i++)
		{
			int num2 = overlapping[i].DepthLayer - layerMin;
			if (num2 >= 0 && num2 < slotCount)
			{
				num |= 1U << num2;
			}
		}
		int num3 = SortedDecals.FindFreeSlot(num, slotCount);
		if (num3 >= 0)
		{
			return layerMin + num3;
		}
		GroundDecal oldest = SortedDecals.GetOldest(overlapping);
		num3 = oldest.DepthLayer - layerMin;
		this.RemoveDecal(oldest, null);
		return layerMin + num3;
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x000598F4 File Offset: 0x00057AF4
	private static int FindFreeSlot(uint used, int slotCount)
	{
		int i = slotCount - 1;
		while (i >= 0)
		{
			if ((used & (1U << i)) != 0U)
			{
				if (i + 1 >= slotCount)
				{
					return SortedDecals.FindLowestFreeSlot(used, slotCount);
				}
				return i + 1;
			}
			else
			{
				i--;
			}
		}
		return 0;
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x0005992C File Offset: 0x00057B2C
	private static int FindLowestFreeSlot(uint used, int slotCount)
	{
		for (int i = 0; i < slotCount; i++)
		{
			if ((used & (1U << i)) == 0U)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x00059954 File Offset: 0x00057B54
	private void CollectOverlapping(int cellX, int cellZ, Vector3 groundPos, DecalLayerBand layerBand, List<GroundDecal> result)
	{
		result.Clear();
		for (int i = -1; i <= 1; i++)
		{
			Dictionary<int, List<GroundDecal>> dictionary;
			if (this.decalsXZ.TryGetValue(cellX + i, out dictionary))
			{
				for (int j = -1; j <= 1; j++)
				{
					List<GroundDecal> list;
					if (dictionary.TryGetValue(cellZ + j, out list))
					{
						for (int k = 0; k < list.Count; k++)
						{
							GroundDecal groundDecal = list[k];
							if (groundDecal.LayerBand == layerBand && SortedDecals.SqrDistanceXZ(groundDecal.GroundPosition, groundPos) < 3.0976f)
							{
								result.Add(groundDecal);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x000599E8 File Offset: 0x00057BE8
	private void CullCoveredDecals(Vector3 groundPos, List<GroundDecal> overlapping)
	{
		float num = Mathf.Clamp01(LazySingletonSO<GlobalResources>.Instance.fighting.decalOverlapCullFactor);
		if (num <= 0f)
		{
			return;
		}
		float num2 = 1.76f * num;
		float num3 = num2 * num2;
		for (int i = overlapping.Count - 1; i >= 0; i--)
		{
			GroundDecal groundDecal = overlapping[i];
			if (SortedDecals.SqrDistanceXZ(groundDecal.GroundPosition, groundPos) < num3)
			{
				overlapping.RemoveAt(i);
				this.RemoveDecal(groundDecal, null);
			}
		}
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x00059A58 File Offset: 0x00057C58
	private void EnforceGlobalBudget()
	{
		int maxActiveDecals = LazySingletonSO<GlobalResources>.Instance.fighting.maxActiveDecals;
		if (maxActiveDecals <= 0)
		{
			return;
		}
		while (this.spawnOrder.Count > maxActiveDecals)
		{
			this.RemoveDecal(this.spawnOrder[0], null);
		}
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x00059A9C File Offset: 0x00057C9C
	private List<GroundDecal> GetOrCreateCell(int cellX, int cellZ)
	{
		Dictionary<int, List<GroundDecal>> dictionary;
		if (!this.decalsXZ.TryGetValue(cellX, out dictionary))
		{
			dictionary = new Dictionary<int, List<GroundDecal>>();
			this.decalsXZ[cellX] = dictionary;
		}
		List<GroundDecal> list;
		if (!dictionary.TryGetValue(cellZ, out list))
		{
			list = new List<GroundDecal>();
			dictionary[cellZ] = list;
		}
		return list;
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x00059AE8 File Offset: 0x00057CE8
	private void Unregister(GroundDecal decal)
	{
		this.spawnOrder.Remove(decal);
		if (decal.DepthLayer == -1)
		{
			return;
		}
		Dictionary<int, List<GroundDecal>> dictionary;
		List<GroundDecal> list;
		if (!this.decalsXZ.TryGetValue(decal.CellX, out dictionary) || !dictionary.TryGetValue(decal.CellZ, out list))
		{
			return;
		}
		list.Remove(decal);
		if (list.Count > 0)
		{
			return;
		}
		dictionary.Remove(decal.CellZ);
		if (dictionary.Count == 0)
		{
			this.decalsXZ.Remove(decal.CellX);
		}
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x00059B6C File Offset: 0x00057D6C
	private static GroundDecal GetOldest(List<GroundDecal> decals)
	{
		GroundDecal groundDecal = decals[0];
		for (int i = 1; i < decals.Count; i++)
		{
			if (decals[i].Lifetime > groundDecal.Lifetime)
			{
				groundDecal = decals[i];
			}
		}
		return groundDecal;
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x00059BB0 File Offset: 0x00057DB0
	private static float SqrDistanceXZ(Vector3 a, Vector3 b)
	{
		float num = a.x - b.x;
		float num2 = a.z - b.z;
		return num * num + num2 * num2;
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x00059BE0 File Offset: 0x00057DE0
	private static int GetLayerMin(DecalLayerBand layerBand)
	{
		int num;
		switch (layerBand)
		{
		case DecalLayerBand.Blood:
			num = 12;
			break;
		case DecalLayerBand.Gore:
			num = 70;
			break;
		case DecalLayerBand.Guts:
			num = 40;
			break;
		default:
			throw new ArgumentException("Invalid layer band");
		}
		return num;
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x00059C1C File Offset: 0x00057E1C
	private static int GetSlotCount(DecalLayerBand layerBand)
	{
		int num;
		switch (layerBand)
		{
		case DecalLayerBand.Blood:
			num = 28;
			break;
		case DecalLayerBand.Gore:
			num = 30;
			break;
		case DecalLayerBand.Guts:
			num = 30;
			break;
		default:
			throw new ArgumentException("Invalid layer band");
		}
		return num;
	}

	// Token: 0x040013C6 RID: 5062
	private const float CELL_SIZE = 1.76f;

	// Token: 0x040013C7 RID: 5063
	private const float OVERLAP_DISTANCE = 1.76f;

	// Token: 0x040013C8 RID: 5064
	private const float OVERLAP_DISTANCE_SQR = 3.0976f;

	// Token: 0x040013C9 RID: 5065
	private const int BLOOD_SLOT_COUNT = 28;

	// Token: 0x040013CA RID: 5066
	private const int GORE_SLOT_COUNT = 30;

	// Token: 0x040013CB RID: 5067
	private const int GUTS_SLOT_COUNT = 30;

	// Token: 0x040013CC RID: 5068
	private readonly Dictionary<int, Dictionary<int, List<GroundDecal>>> decalsXZ = new Dictionary<int, Dictionary<int, List<GroundDecal>>>();

	// Token: 0x040013CD RID: 5069
	private readonly List<GroundDecal> spawnOrder = new List<GroundDecal>();

	// Token: 0x040013CE RID: 5070
	private readonly List<GroundDecal> overlapBuffer = new List<GroundDecal>();
}
