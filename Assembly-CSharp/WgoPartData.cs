using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020005CD RID: 1485
[Serializable]
public class WgoPartData
{
	// Token: 0x14000088 RID: 136
	// (add) Token: 0x060026ED RID: 9965 RVA: 0x000B6D94 File Offset: 0x000B4F94
	// (remove) Token: 0x060026EE RID: 9966 RVA: 0x000B6DCC File Offset: 0x000B4FCC
	public event Action<string, int> OnStateChange;

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x060026EF RID: 9967 RVA: 0x000B6E01 File Offset: 0x000B5001
	// (set) Token: 0x060026F0 RID: 9968 RVA: 0x000B6E09 File Offset: 0x000B5009
	public List<WgoPartStateData> AvailableVariations { get; set; }

	// Token: 0x1700064C RID: 1612
	// (get) Token: 0x060026F1 RID: 9969 RVA: 0x000B6E12 File Offset: 0x000B5012
	public WgoPartBakedData BakedData
	{
		get
		{
			return LazySingletonSerializedSO<WgoPartBakedDataCollection>.Instance.Get(this.id);
		}
	}

	// Token: 0x1700064D RID: 1613
	// (get) Token: 0x060026F2 RID: 9970 RVA: 0x000B6E24 File Offset: 0x000B5024
	public int DockPointsCount
	{
		get
		{
			return this.dockPointDataList.Count;
		}
	}

	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x060026F3 RID: 9971 RVA: 0x000B6E31 File Offset: 0x000B5031
	public List<DockPointData> DockPointDataList
	{
		get
		{
			return this.dockPointDataList;
		}
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x000B6E39 File Offset: 0x000B5039
	private WgoPartData()
	{
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x000B6E48 File Offset: 0x000B5048
	public WgoPartData(string id)
	{
		this.id = id;
		this.CreateDockPointData();
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x000B6E64 File Offset: 0x000B5064
	public WgoPartData(string id, string variationId, int rotationIndex)
	{
		this.id = id;
		this.variationId = variationId;
		this.rotationIndex = rotationIndex;
		this.CreateDockPointData();
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x000B6E8E File Offset: 0x000B508E
	public WgoPartData(WgoPartData other)
	{
		this.id = other.id;
		this.variationId = other.variationId;
		this.rotationIndex = other.rotationIndex;
		this.CreateDockPointData();
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x000B6EC8 File Offset: 0x000B50C8
	public void PrepareForGame()
	{
		Dictionary<int, List<DockPointData.Baked>> bakedDockPointsByHash = this.GetBakedDockPointsByHash();
		if (this.dockPointDataHashes == null || this.dockPointDataList == null || this.dockPointDataHashes.Count != this.dockPointDataList.Count || !this.DoesDockPointStructureMatch(bakedDockPointsByHash))
		{
			Dictionary<int, List<SGuid>> dictionary = this.CaptureDockPointOccupationByHash();
			this.CreateDockPointData();
			this.RestoreDockPointOccupation(dictionary);
		}
		else
		{
			this.BindBakedDataByHash(bakedDockPointsByHash);
		}
		this.RebuildDockPointDict();
	}

	// Token: 0x060026F9 RID: 9977 RVA: 0x000B6F38 File Offset: 0x000B5138
	private Dictionary<int, List<SGuid>> CaptureDockPointOccupationByHash()
	{
		Dictionary<int, List<SGuid>> dictionary = new Dictionary<int, List<SGuid>>();
		if (this.dockPointDataHashes == null || this.dockPointDataList == null)
		{
			return dictionary;
		}
		int num = Math.Min(this.dockPointDataHashes.Count, this.dockPointDataList.Count);
		for (int i = 0; i < num; i++)
		{
			int num2 = this.dockPointDataHashes[i];
			List<SGuid> list;
			if (!dictionary.TryGetValue(num2, out list))
			{
				list = new List<SGuid>();
				dictionary[num2] = list;
			}
			DockPointData dockPointData = this.dockPointDataList[i];
			list.Add((dockPointData != null) ? dockPointData.OccupiedBy : SGuid.Empty);
		}
		return dictionary;
	}

	// Token: 0x060026FA RID: 9978 RVA: 0x000B6FD8 File Offset: 0x000B51D8
	private Dictionary<int, List<DockPointData.Baked>> GetBakedDockPointsByHash()
	{
		Dictionary<int, List<DockPointData.Baked>> dictionary = new Dictionary<int, List<DockPointData.Baked>>();
		WgoPartBakedData bakedData = this.BakedData;
		IReadOnlyList<WgoPartBakedData.WgoPartDockPointsBakedData> readOnlyList = ((bakedData != null) ? bakedData.PointsList : null);
		if (readOnlyList == null)
		{
			return dictionary;
		}
		foreach (WgoPartBakedData.WgoPartDockPointsBakedData wgoPartDockPointsBakedData in readOnlyList)
		{
			if (((wgoPartDockPointsBakedData != null) ? wgoPartDockPointsBakedData.dockPoints : null) != null)
			{
				List<DockPointData.Baked> list;
				if (!dictionary.TryGetValue(wgoPartDockPointsBakedData.hash, out list))
				{
					list = new List<DockPointData.Baked>();
					dictionary[wgoPartDockPointsBakedData.hash] = list;
				}
				for (int i = 0; i < wgoPartDockPointsBakedData.dockPoints.Count; i++)
				{
					if (wgoPartDockPointsBakedData.dockPoints[i] != null)
					{
						list.Add(wgoPartDockPointsBakedData.dockPoints[i]);
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000B70A8 File Offset: 0x000B52A8
	private bool DoesDockPointStructureMatch(Dictionary<int, List<DockPointData.Baked>> bakedByHash)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < this.dockPointDataHashes.Count; i++)
		{
			int num = this.dockPointDataHashes[i];
			int num2;
			dictionary[num] = (dictionary.TryGetValue(num, out num2) ? (num2 + 1) : 1);
		}
		if (dictionary.Count != bakedByHash.Count)
		{
			return false;
		}
		foreach (KeyValuePair<int, List<DockPointData.Baked>> keyValuePair in bakedByHash)
		{
			int num3;
			if (!dictionary.TryGetValue(keyValuePair.Key, out num3) || num3 != keyValuePair.Value.Count)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000B716C File Offset: 0x000B536C
	private void BindBakedDataByHash(Dictionary<int, List<DockPointData.Baked>> bakedByHash)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < this.dockPointDataHashes.Count; i++)
		{
			int num = this.dockPointDataHashes[i];
			int num2;
			if (!dictionary.TryGetValue(num, out num2))
			{
				num2 = 0;
			}
			dictionary[num] = num2 + 1;
			DockPointData dockPointData = this.dockPointDataList[i];
			List<DockPointData.Baked> list;
			if (dockPointData != null && dockPointData.BakedData == null && bakedByHash.TryGetValue(num, out list) && num2 < list.Count)
			{
				dockPointData.BakedData = list[num2];
			}
		}
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x000B71F8 File Offset: 0x000B53F8
	private void RestoreDockPointOccupation(Dictionary<int, List<SGuid>> occupationByHash)
	{
		if (occupationByHash == null || occupationByHash.Count == 0 || this.dockPointDataHashes == null || this.dockPointDataList == null)
		{
			return;
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		int num = Math.Min(this.dockPointDataHashes.Count, this.dockPointDataList.Count);
		for (int i = 0; i < num; i++)
		{
			int num2 = this.dockPointDataHashes[i];
			List<SGuid> list;
			if (occupationByHash.TryGetValue(num2, out list))
			{
				int num3;
				if (!dictionary.TryGetValue(num2, out num3))
				{
					num3 = 0;
				}
				dictionary[num2] = num3 + 1;
				DockPointData dockPointData = this.dockPointDataList[i];
				if (dockPointData != null && num3 < list.Count && list[num3] != SGuid.Empty)
				{
					dockPointData.Occupy(list[num3]);
				}
			}
		}
	}

	// Token: 0x060026FE RID: 9982 RVA: 0x000B72C4 File Offset: 0x000B54C4
	private void RebuildDockPointDict()
	{
		this.dockPointDataDict = new Dictionary<int, List<DockPointData>>();
		if (this.dockPointDataHashes == null || this.dockPointDataList == null)
		{
			return;
		}
		int num = Math.Min(this.dockPointDataHashes.Count, this.dockPointDataList.Count);
		for (int i = 0; i < num; i++)
		{
			DockPointData dockPointData = this.dockPointDataList[i];
			if (dockPointData != null)
			{
				List<DockPointData> list;
				if (!this.dockPointDataDict.TryGetValue(this.dockPointDataHashes[i], out list))
				{
					list = new List<DockPointData>();
					this.dockPointDataDict.Add(this.dockPointDataHashes[i], list);
				}
				list.Add(dockPointData);
			}
		}
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x000B7365 File Offset: 0x000B5565
	public override string ToString()
	{
		return string.Format("[id={0}, rotation index={1}]", this.id, this.rotationIndex);
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x000B7384 File Offset: 0x000B5584
	public bool TryApplyState(SGuid parent, string variationId, int rotationIndex = -1)
	{
		if (this.AvailableVariations == null || this.AvailableVariations.Count == 0)
		{
			return false;
		}
		int num;
		if (rotationIndex == -1)
		{
			num = this.AvailableVariations.FindIndex((WgoPartStateData x) => x.id == variationId);
		}
		else
		{
			num = this.AvailableVariations.FindIndex((WgoPartStateData x) => x.id == variationId && x.rotationIndex == rotationIndex);
		}
		if (num == -1)
		{
			if (rotationIndex == -1)
			{
				Debug.LogError(string.Concat(new string[] { "Can not find variation [id:", variationId, "] for part [", this.id, "]" }));
			}
			else
			{
				Debug.LogError(string.Format("Can not find variation [id:{0}, rotationIndex:[{1}]] for part [{2}]", variationId, rotationIndex, this.id));
			}
			return false;
		}
		this.variationId = this.AvailableVariations[num].id;
		this.rotationIndex = this.AvailableVariations[num].rotationIndex;
		this.TryDisableDockPoints(parent);
		Action<string, int> onStateChange = this.OnStateChange;
		if (onStateChange != null)
		{
			onStateChange(this.variationId, this.rotationIndex);
		}
		return true;
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x000B74BB File Offset: 0x000B56BB
	public int GetStateHash()
	{
		return WgoPartData.GetStateHash(this.variationId, this.rotationIndex);
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x000B74CE File Offset: 0x000B56CE
	public static int GetStateHash(string variationId, int rotationIndex)
	{
		return (17 * 31 + ((!string.IsNullOrEmpty(variationId)) ? variationId.GetHashCode() : 0)) * 31 + rotationIndex;
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x000B74EC File Offset: 0x000B56EC
	public void TryDisableDockPoints(SGuid parent)
	{
		List<DockPointData> list;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list))
		{
			MainGame instance = MainGame.Instance;
			WorldData worldData;
			if (instance == null)
			{
				worldData = null;
			}
			else
			{
				GameSave gameSave = instance.GameSave;
				worldData = ((gameSave != null) ? gameSave.worldData : null);
			}
			WorldData worldData2 = worldData;
			foreach (DockPointData dockPointData in list)
			{
				if (dockPointData.IsOccupied)
				{
					SGuid occupiedBy = dockPointData.OccupiedBy;
					WgoData wgoData = ((worldData2 != null) ? worldData2.GetWgoData(occupiedBy) : null);
					if (wgoData != null && wgoData.takenDockPointsParentSGuid == parent)
					{
						wgoData.UnOccupyDockPoint(dockPointData);
					}
					else
					{
						dockPointData.UnOccupy();
					}
					if (worldData2 != null)
					{
						worldData2.NotifyDockPointHasToBeDisabled(parent, occupiedBy);
					}
				}
			}
		}
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x000B75B8 File Offset: 0x000B57B8
	public void TryFreeDockPoint(SGuid parent, SGuid occupant)
	{
		List<DockPointData> list;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list))
		{
			foreach (DockPointData dockPointData in list)
			{
				if (dockPointData.OccupiedBy == occupant)
				{
					dockPointData.UnOccupy();
					MainGame.Instance.GameSave.worldData.NotifyDockPointFreed(parent, occupant);
				}
			}
		}
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x000B7640 File Offset: 0x000B5840
	[CanBeNull]
	public DockPointData GetNearestDockPoint(WgoData parent, Vector3 positionFrom, out Vector3 dockPointPosition, DockPointData.Availability availability = DockPointData.Availability.All, DockPointData.Filter filter = DockPointData.Filter.All, Func<DockPointData, Vector3, bool> additionalCheck = null)
	{
		dockPointPosition = default(Vector3);
		DockPointData dockPointData = null;
		float num = float.MaxValue;
		List<DockPointData> list;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list))
		{
			foreach (DockPointData dockPointData2 in list)
			{
				if (((dockPointData2 != null) ? dockPointData2.BakedData : null) != null)
				{
					if (availability != DockPointData.Availability.OnlyOccupied)
					{
						if (availability == DockPointData.Availability.OnlyNotOccupied && dockPointData2.IsOccupied)
						{
							continue;
						}
					}
					else if (!dockPointData2.IsOccupied)
					{
						continue;
					}
					if (filter != DockPointData.Filter.OnlyZombie)
					{
						if (filter == DockPointData.Filter.OnlyNotZombie)
						{
							if (dockPointData2.BakedData.IsForZombie)
							{
								continue;
							}
						}
					}
					else if (!dockPointData2.BakedData.IsForZombie)
					{
						continue;
					}
					float magnitude = (parent.GetDockPointDataWorldPosition(dockPointData2) - positionFrom).magnitude;
					if (magnitude < num && (additionalCheck == null || additionalCheck(dockPointData2, parent.Position)))
					{
						num = magnitude;
						dockPointData = dockPointData2;
					}
				}
			}
		}
		if (dockPointData != null)
		{
			dockPointPosition = parent.GetDockPointDataWorldPosition(dockPointData);
		}
		return dockPointData;
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x000B7760 File Offset: 0x000B5960
	[CanBeNull]
	public DockPointData GetNearestDockPoint(WgoData parent, Vector3 positionFrom, DockPointData.Availability availability = DockPointData.Availability.All, DockPointData.Filter filter = DockPointData.Filter.All, Func<DockPointData, Vector3, bool> additionalCheck = null)
	{
		Vector3 vector;
		return this.GetNearestDockPoint(parent, positionFrom, out vector, availability, filter, additionalCheck);
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x000B777C File Offset: 0x000B597C
	public DockPointData GetDockPointById(SGuid sGuid)
	{
		List<DockPointData> list;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list))
		{
			foreach (DockPointData dockPointData in list)
			{
				if (dockPointData.OccupiedBy == sGuid)
				{
					return dockPointData;
				}
			}
		}
		return null;
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x000B77F0 File Offset: 0x000B59F0
	[CanBeNull]
	public DockPointData GetOccupiedDockPointBy(SGuid occupant)
	{
		List<DockPointData> list;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list))
		{
			foreach (DockPointData dockPointData in list)
			{
				if (dockPointData.OccupiedBy == occupant)
				{
					return dockPointData;
				}
			}
		}
		return null;
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x000B7864 File Offset: 0x000B5A64
	public List<DockPointData> GetDockPoints(DockPointData.Availability availability = DockPointData.Availability.All, DockPointData.Filter filter = DockPointData.Filter.All)
	{
		List<DockPointData> list = new List<DockPointData>();
		List<DockPointData> list2;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list2))
		{
			foreach (DockPointData dockPointData in list2)
			{
				if (((dockPointData != null) ? dockPointData.BakedData : null) != null)
				{
					if (availability != DockPointData.Availability.OnlyOccupied)
					{
						if (availability == DockPointData.Availability.OnlyNotOccupied && dockPointData.IsOccupied)
						{
							continue;
						}
					}
					else if (!dockPointData.IsOccupied)
					{
						continue;
					}
					if (filter != DockPointData.Filter.OnlyZombie)
					{
						if (filter == DockPointData.Filter.OnlyNotZombie)
						{
							if (dockPointData.BakedData.IsForZombie)
							{
								continue;
							}
						}
					}
					else if (!dockPointData.BakedData.IsForZombie)
					{
						continue;
					}
					list.Add(dockPointData);
				}
			}
		}
		return list;
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x000B791C File Offset: 0x000B5B1C
	public int GetOccupiedDockPointIndex(SGuid occupant)
	{
		List<DockPointData> list;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].OccupiedBy == occupant)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x000B7968 File Offset: 0x000B5B68
	[CanBeNull]
	public DockPointData GetDockPointByIndex(int index)
	{
		List<DockPointData> list;
		if (this.dockPointDataDict.TryGetValue(this.GetStateHash(), out list) && list.Count - 1 >= index)
		{
			return list[index];
		}
		return null;
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x000B79A0 File Offset: 0x000B5BA0
	public bool HasAnyAvailableDockPoint(DockPointData.Filter filter = DockPointData.Filter.All)
	{
		using (List<DockPointData>.Enumerator enumerator = this.GetDockPoints(DockPointData.Availability.All, filter).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsOccupied)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x000B79FC File Offset: 0x000B5BFC
	public Rect GetCollisionBoundsRect(Vector3 objGlobalPos)
	{
		Rect rect2;
		Rect rect = (this.BakedData.VariationCollisionBoundsRectDict.TryGetValue(this.GetStateHash(), out rect2) ? rect2 : Rect.zero);
		return new Rect(rect.x + objGlobalPos.x, rect.y + objGlobalPos.z, rect.width, rect.height);
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x000B7A5C File Offset: 0x000B5C5C
	private void CreateDockPointData()
	{
		this.dockPointDataList = new List<DockPointData>();
		this.dockPointDataHashes = new List<int>();
		WgoPartBakedData bakedData = this.BakedData;
		IReadOnlyList<WgoPartBakedData.WgoPartDockPointsBakedData> readOnlyList = ((bakedData != null) ? bakedData.PointsList : null);
		if (readOnlyList == null)
		{
			return;
		}
		foreach (WgoPartBakedData.WgoPartDockPointsBakedData wgoPartDockPointsBakedData in readOnlyList)
		{
			if (((wgoPartDockPointsBakedData != null) ? wgoPartDockPointsBakedData.dockPoints : null) != null)
			{
				for (int i = 0; i < wgoPartDockPointsBakedData.dockPoints.Count; i++)
				{
					DockPointData.Baked baked = wgoPartDockPointsBakedData.dockPoints[i];
					if (baked != null)
					{
						this.dockPointDataList.Add(new DockPointData
						{
							BakedData = baked
						});
						this.dockPointDataHashes.Add(wgoPartDockPointsBakedData.hash);
					}
				}
			}
		}
	}

	// Token: 0x0400217A RID: 8570
	public string id;

	// Token: 0x0400217B RID: 8571
	public string variationId;

	// Token: 0x0400217C RID: 8572
	public int rotationIndex = -1;

	// Token: 0x0400217D RID: 8573
	[SerializeField]
	private List<DockPointData> dockPointDataList;

	// Token: 0x0400217E RID: 8574
	[SerializeField]
	private List<int> dockPointDataHashes;

	// Token: 0x0400217F RID: 8575
	private Dictionary<int, List<DockPointData>> dockPointDataDict;
}
