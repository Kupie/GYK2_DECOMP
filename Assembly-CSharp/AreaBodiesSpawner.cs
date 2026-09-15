using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

// Token: 0x020007CC RID: 1996
public class AreaBodiesSpawner : MonoBehaviour
{
	// Token: 0x06003349 RID: 13129 RVA: 0x000F6B7C File Offset: 0x000F4D7C
	public void SpawnBodies()
	{
		if (this.spawnedBodies.Count > 0)
		{
			this.ClearBodies();
		}
		if (this.col == null || this.bodyPrefab == null)
		{
			Debug.LogWarning("Collider or Body Prefab not set.", this);
			return;
		}
		Bounds bounds = this.col.bounds;
		for (int i = 0; i < this.bodiesCount; i++)
		{
			int num = 100;
			Vector3 center;
			bool flag;
			do
			{
				center = new Vector3(global::UnityEngine.Random.Range(bounds.min.x, bounds.max.x), global::UnityEngine.Random.Range(bounds.min.y, bounds.max.y), global::UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
				flag = this.col.ClosestPoint(center) == center;
			}
			while (!flag && --num > 0);
			if (!flag)
			{
				center = bounds.center;
				Debug.LogWarning("Could not find a point inside collider '" + this.col.name + "' after 100 attempts. Using bounds center as fallback.", this);
			}
			center.y = this.baseY + global::UnityEngine.Random.Range(-this.yRange, 0f);
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.bodyPrefab, center, Quaternion.identity);
			gameObject.transform.SetParent(this.bodiesParent.transform, true);
			gameObject.gameObject.SetActive(true);
			this.spawnedBodies.Add(gameObject);
			Rigidbody rigidbody;
			if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
			{
				rigidbody = gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.useGravity = false;
			for (int j = 0; j < this.preSetBodies.Count; j++)
			{
				Rigidbody rigidbody2 = this.preSetBodies[j];
				rigidbody2.transform.position = this.presetBodiesPositions[j];
				this.spawnedBodies.Add(rigidbody2.gameObject);
			}
		}
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x000F6D6C File Offset: 0x000F4F6C
	private bool IsPointInsideCollider(Vector3 point, Collider targetCollider)
	{
		bool flag = true;
		MeshCollider meshCollider = targetCollider as MeshCollider;
		if (meshCollider != null)
		{
			flag = meshCollider.convex;
		}
		if (flag)
		{
			return targetCollider.ClosestPoint(point) == point;
		}
		bool queriesHitBackfaces = Physics.queriesHitBackfaces;
		Physics.queriesHitBackfaces = true;
		int num = 0;
		foreach (RaycastHit raycastHit in Physics.RaycastAll(new Ray(point, Vector3.up), float.PositiveInfinity))
		{
			if (raycastHit.collider == targetCollider)
			{
				num++;
			}
		}
		Physics.queriesHitBackfaces = queriesHitBackfaces;
		return num % 2 == 1;
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x000F6E00 File Offset: 0x000F5000
	public void SpawnBodies_NoOverlap()
	{
		if (this.spawnedBodies.Count > 0)
		{
			this.ClearBodies();
		}
		if (this.col == null || this.bodyPrefab == null)
		{
			Debug.LogWarning("Collider or Body Prefab not set.", this);
			return;
		}
		Vector3 vector = this.GetPrefabHalfExtentsWorld();
		vector += Vector3.one * Mathf.Max(0f, this.noOverlapPadding);
		Bounds bounds = this.col.bounds;
		List<Bounds> list = new List<Bounds>();
		for (int i = 0; i < this.preSetBodies.Count; i++)
		{
			Rigidbody rigidbody = this.preSetBodies[i];
			if (rigidbody)
			{
				if (i < this.presetBodiesPositions.Count)
				{
					rigidbody.transform.position = this.presetBodiesPositions[i];
				}
				Bounds bounds2 = AreaBodiesSpawner.ComputeWorldBoundsFromColliders(rigidbody.gameObject);
				bounds2.Expand(this.noOverlapPadding * 2f);
				list.Add(bounds2);
				if (!this.spawnedBodies.Contains(rigidbody.gameObject))
				{
					this.spawnedBodies.Add(rigidbody.gameObject);
				}
			}
		}
		float num = Mathf.Max(vector.x * 2f, vector.z * 2f);
		int num2 = Mathf.CeilToInt((bounds.max.x - bounds.min.x) / num);
		int num3 = Mathf.CeilToInt((bounds.max.z - bounds.min.z) / num);
		bool[,] array = new bool[num2, num3];
		foreach (Bounds bounds3 in list)
		{
			int num4 = Mathf.FloorToInt((bounds3.min.x - bounds.min.x) / num);
			int num5 = Mathf.CeilToInt((bounds3.max.x - bounds.min.x) / num);
			int num6 = Mathf.FloorToInt((bounds3.min.z - bounds.min.z) / num);
			int num7 = Mathf.CeilToInt((bounds3.max.z - bounds.min.z) / num);
			for (int j = Mathf.Max(0, num4); j < Mathf.Min(num2, num5); j++)
			{
				for (int k = Mathf.Max(0, num6); k < Mathf.Min(num3, num7); k++)
				{
					array[j, k] = true;
				}
			}
		}
		List<Vector2Int> list2 = new List<Vector2Int>();
		int num8 = num2 * num3;
		for (int l = 0; l < num2; l++)
		{
			for (int m = 0; m < num3; m++)
			{
				if (!array[l, m])
				{
					float num9 = bounds.min.x + (float)l * num;
					float num10 = bounds.min.x + (float)(l + 1) * num;
					float num11 = bounds.min.z + (float)m * num;
					float num12 = bounds.min.z + (float)(m + 1) * num;
					Vector3[] array2 = new Vector3[]
					{
						new Vector3(num9 + num * 0.5f, this.baseY, num11 + num * 0.5f),
						new Vector3(num9, this.baseY, num11),
						new Vector3(num10, this.baseY, num11),
						new Vector3(num9, this.baseY, num12),
						new Vector3(num10, this.baseY, num12)
					};
					bool flag = false;
					foreach (Vector3 vector2 in array2)
					{
						if (this.IsPointInsideCollider(vector2, this.col))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list2.Add(new Vector2Int(l, m));
					}
				}
			}
		}
		int num13 = 0;
		for (int num14 = 0; num14 < num2; num14++)
		{
			for (int num15 = 0; num15 < num3; num15++)
			{
				if (array[num14, num15])
				{
					num13++;
				}
			}
		}
		Debug.Log(string.Format("Grid: {0}x{1} ({2} total), occupied by presets: {3}, cells intersecting collider: {4}", new object[] { num2, num3, num8, num13, list2.Count }));
		int num16 = 0;
		while (num16 < this.bodiesCount && list2.Count > 0)
		{
			bool flag2 = false;
			for (int num17 = 0; num17 < Mathf.Min(this.noOverlapMaxAttemptsPerBody, list2.Count); num17++)
			{
				int num18 = global::UnityEngine.Random.Range(0, list2.Count);
				Vector2Int vector2Int = list2[num18];
				float num19 = bounds.min.x + ((float)vector2Int.x + 0.5f) * num;
				float num20 = bounds.min.z + ((float)vector2Int.y + 0.5f) * num;
				Vector3 vector3 = new Vector3(num19, this.baseY + global::UnityEngine.Random.Range(-this.yRange, 0f), num20);
				if (!this.IsPointInsideCollider(vector3, this.col))
				{
					list2.RemoveAt(num18);
				}
				else
				{
					Bounds bounds4 = new Bounds(vector3, vector * 2f);
					bounds4.Expand(this.noOverlapPadding * 2f);
					bool flag3 = false;
					for (int num21 = 0; num21 < list.Count; num21++)
					{
						if (list[num21].Intersects(bounds4))
						{
							flag3 = true;
							break;
						}
					}
					if (!flag3)
					{
						GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.bodyPrefab, vector3, Quaternion.identity);
						gameObject.transform.SetParent(this.bodiesParent.transform, true);
						gameObject.gameObject.SetActive(true);
						this.spawnedBodies.Add(gameObject);
						if (!gameObject.GetComponent<Rigidbody>())
						{
							gameObject.AddComponent<Rigidbody>().useGravity = false;
						}
						Bounds bounds5 = AreaBodiesSpawner.ComputeWorldBoundsFromColliders(gameObject);
						bounds5.Expand(this.noOverlapPadding * 2f);
						list.Add(bounds5);
						int num22 = Mathf.FloorToInt((bounds5.min.x - bounds.min.x) / num);
						int num23 = Mathf.CeilToInt((bounds5.max.x - bounds.min.x) / num);
						int num24 = Mathf.FloorToInt((bounds5.min.z - bounds.min.z) / num);
						int num25 = Mathf.CeilToInt((bounds5.max.z - bounds.min.z) / num);
						for (int num26 = Mathf.Max(0, num22); num26 < Mathf.Min(num2, num23); num26++)
						{
							for (int num27 = Mathf.Max(0, num24); num27 < Mathf.Min(num3, num25); num27++)
							{
								array[num26, num27] = true;
							}
						}
						list2.RemoveAt(num18);
						flag2 = true;
						break;
					}
					list2.RemoveAt(num18);
				}
			}
			if (!flag2)
			{
				Debug.LogWarning(string.Format("Could not place body {0}/{1} without overlap after {2} attempts", num16 + 1, this.bodiesCount, this.noOverlapMaxAttemptsPerBody), this);
			}
			num16++;
		}
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x000F7598 File Offset: 0x000F5798
	public void SpawnBodies_Poisson()
	{
		if (this.spawnedBodies.Count > 0)
		{
			this.ClearBodies();
		}
		if (this.col == null || this.bodyPrefab == null)
		{
			Debug.LogWarning("Collider or Body Prefab not set.", this);
			return;
		}
		Vector3 vector = this.GetPrefabHalfExtentsWorld();
		vector += Vector3.one * Mathf.Max(0f, this.noOverlapPadding);
		Bounds bounds = this.col.bounds;
		List<Bounds> list = new List<Bounds>();
		for (int i = 0; i < this.preSetBodies.Count; i++)
		{
			Rigidbody rigidbody = this.preSetBodies[i];
			if (rigidbody)
			{
				if (i < this.presetBodiesPositions.Count)
				{
					rigidbody.transform.position = this.presetBodiesPositions[i];
				}
				Bounds bounds2 = AreaBodiesSpawner.ComputeWorldBoundsFromColliders(rigidbody.gameObject);
				bounds2.Expand(this.noOverlapPadding * 2f);
				list.Add(bounds2);
				if (!this.spawnedBodies.Contains(rigidbody.gameObject))
				{
					this.spawnedBodies.Add(rigidbody.gameObject);
				}
			}
		}
		float num = Mathf.Max(vector.x * 2f, vector.z * 2f);
		List<Vector2> list2 = this.GeneratePoissonSamples(bounds, num, this.col, list, this.bodiesCount);
		Debug.Log(string.Format("Poisson sampling generated {0} valid positions", list2.Count));
		for (int j = 0; j < Mathf.Min(list2.Count, this.bodiesCount); j++)
		{
			Vector3 vector2 = new Vector3(list2[j].x, this.baseY + global::UnityEngine.Random.Range(-this.yRange, 0f), list2[j].y);
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.bodyPrefab, vector2, Quaternion.identity);
			gameObject.transform.SetParent(this.bodiesParent.transform, true);
			gameObject.gameObject.SetActive(true);
			BodyTrailer bodyTrailer;
			if (gameObject.TryGetComponent<BodyTrailer>(out bodyTrailer))
			{
				bodyTrailer.RollAndSetTexture();
			}
			this.spawnedBodies.Add(gameObject);
			if (!gameObject.GetComponent<Rigidbody>())
			{
				gameObject.AddComponent<Rigidbody>().useGravity = false;
			}
			Bounds bounds3 = AreaBodiesSpawner.ComputeWorldBoundsFromColliders(gameObject);
			bounds3.Expand(this.noOverlapPadding * 2f);
			list.Add(bounds3);
		}
		Debug.Log(string.Format("Spawn completed: {0} bodies total ({1} preset + {2} spawned)", this.spawnedBodies.Count, this.preSetBodies.Count, this.spawnedBodies.Count - this.preSetBodies.Count));
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x000F7864 File Offset: 0x000F5A64
	private List<Vector2> GeneratePoissonSamples(Bounds areaBounds, float minDistance, Collider collider, List<Bounds> obstacles, int maxSamples)
	{
		List<Vector2> list = new List<Vector2>();
		List<Vector2> list2 = new List<Vector2>();
		float num = minDistance / Mathf.Sqrt(2f);
		int num2 = Mathf.CeilToInt((areaBounds.max.x - areaBounds.min.x) / num);
		int num3 = Mathf.CeilToInt((areaBounds.max.z - areaBounds.min.z) / num);
		List<Vector2>[,] array = new List<Vector2>[num2, num3];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num3; j++)
			{
				array[i, j] = new List<Vector2>();
			}
		}
		foreach (Bounds bounds in obstacles)
		{
			int num4 = Mathf.FloorToInt((bounds.min.x - areaBounds.min.x) / num);
			int num5 = Mathf.CeilToInt((bounds.max.x - areaBounds.min.x) / num);
			int num6 = Mathf.FloorToInt((bounds.min.z - areaBounds.min.z) / num);
			int num7 = Mathf.CeilToInt((bounds.max.z - areaBounds.min.z) / num);
			for (int k = Mathf.Max(0, num4); k < Mathf.Min(num2, num5); k++)
			{
				for (int l = Mathf.Max(0, num6); l < Mathf.Min(num3, num7); l++)
				{
					array[k, l].Add(new Vector2(float.MinValue, float.MinValue));
				}
			}
		}
		int num8 = 0;
		Vector2? vector = null;
		while (num8 < 30 && vector == null)
		{
			float num9 = global::UnityEngine.Random.Range(areaBounds.min.x, areaBounds.max.x);
			float num10 = global::UnityEngine.Random.Range(areaBounds.min.z, areaBounds.max.z);
			Vector3 vector2 = new Vector3(num9, this.baseY, num10);
			if (this.IsPointInsideCollider(vector2, collider) && !this.IsNearObstacle(vector2, obstacles, minDistance))
			{
				vector = new Vector2?(new Vector2(num9, num10));
			}
			num8++;
		}
		if (vector == null)
		{
			Debug.LogWarning("Could not find initial sample for Poisson disk sampling");
			return list;
		}
		list.Add(vector.Value);
		list2.Add(vector.Value);
		int num11 = Mathf.FloorToInt((vector.Value.x - areaBounds.min.x) / num);
		int num12 = Mathf.FloorToInt((vector.Value.y - areaBounds.min.z) / num);
		if (num11 >= 0 && num11 < num2 && num12 >= 0 && num12 < num3)
		{
			array[num11, num12].Add(vector.Value);
		}
		while (list2.Count > 0 && list.Count < maxSamples)
		{
			int num13 = global::UnityEngine.Random.Range(0, list2.Count);
			Vector2 vector3 = list2[num13];
			bool flag = false;
			for (int m = 0; m < 30; m++)
			{
				float num14 = global::UnityEngine.Random.value * 2f * 3.1415927f;
				float num15 = global::UnityEngine.Random.Range(minDistance, 2f * minDistance);
				float num16 = vector3.x + num15 * Mathf.Cos(num14);
				float num17 = vector3.y + num15 * Mathf.Sin(num14);
				if (num16 >= areaBounds.min.x && num16 <= areaBounds.max.x && num17 >= areaBounds.min.z && num17 <= areaBounds.max.z)
				{
					Vector3 vector4 = new Vector3(num16, this.baseY, num17);
					if (this.IsPointInsideCollider(vector4, collider) && !this.IsNearObstacle(vector4, obstacles, minDistance) && this.IsValidSample(new Vector2(num16, num17), array, areaBounds, num, minDistance))
					{
						list.Add(new Vector2(num16, num17));
						list2.Add(new Vector2(num16, num17));
						int num18 = Mathf.FloorToInt((num16 - areaBounds.min.x) / num);
						int num19 = Mathf.FloorToInt((num17 - areaBounds.min.z) / num);
						if (num18 >= 0 && num18 < num2 && num19 >= 0 && num19 < num3)
						{
							array[num18, num19].Add(new Vector2(num16, num17));
						}
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				list2.RemoveAt(num13);
			}
		}
		return list;
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x000F7D3C File Offset: 0x000F5F3C
	private bool IsNearObstacle(Vector3 position, List<Bounds> obstacles, float minDistance)
	{
		foreach (Bounds bounds in obstacles)
		{
			if (bounds.Contains(position) || bounds.SqrDistance(position) < minDistance * minDistance)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x000F7DA4 File Offset: 0x000F5FA4
	private bool IsValidSample(Vector2 candidate, List<Vector2>[,] grid, Bounds areaBounds, float cellSize, float minDistance)
	{
		int num = Mathf.FloorToInt((candidate.x - areaBounds.min.x) / cellSize);
		int num2 = Mathf.FloorToInt((candidate.y - areaBounds.min.z) / cellSize);
		int num3 = Mathf.Max(0, num - 2);
		int num4 = Mathf.Min(grid.GetLength(0), num + 3);
		int num5 = Mathf.Max(0, num2 - 2);
		int num6 = Mathf.Min(grid.GetLength(1), num2 + 3);
		for (int i = num3; i < num4; i++)
		{
			for (int j = num5; j < num6; j++)
			{
				foreach (Vector2 vector in grid[i, j])
				{
					if (vector.x != -3.4028235E+38f && (candidate - vector).sqrMagnitude < minDistance * minDistance)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x000F7EB4 File Offset: 0x000F60B4
	public void AddBody(GameObject body)
	{
		this.spawnedBodies.Add(body);
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x000F7EC4 File Offset: 0x000F60C4
	public void ResetBodiesPos(bool? isKinematic = null)
	{
		for (int i = 0; i < Mathf.Min(this.spawnedBodies.Count, this.bodiesPositionsOnStart.Count); i++)
		{
			this.spawnedBodies[i].transform.position = this.bodiesPositionsOnStart[i];
			Rigidbody rigidbody;
			if (isKinematic != null && this.spawnedBodies[i].TryGetComponent<Rigidbody>(out rigidbody))
			{
				rigidbody.isKinematic = isKinematic.Value;
			}
		}
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x000F7F44 File Offset: 0x000F6144
	public void StartFlow()
	{
		if (this.flowActive)
		{
			return;
		}
		this.flowActive = true;
		this.objToDisableOnFlowStart.ForEach(delegate(GameObject go)
		{
			if (go != null)
			{
				go.SetActive(false);
			}
		});
		this.preSetBodies.ForEach(delegate(Rigidbody rb)
		{
			rb.isKinematic = false;
		});
		this.InitBodiesForFlow();
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x000F7FBB File Offset: 0x000F61BB
	public void StopFlow()
	{
		this.preSetBodies.ForEach(delegate(Rigidbody rb)
		{
			rb.isKinematic = true;
		});
		this.flowActive = false;
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x000F7FF0 File Offset: 0x000F61F0
	public void StartFlowTransform()
	{
		if (this.transformFlowActive)
		{
			return;
		}
		this.transformFlowActive = true;
		this.objToDisableOnFlowStart.ForEach(delegate(GameObject go)
		{
			if (go != null)
			{
				go.SetActive(false);
			}
		});
		for (int i = 0; i < this.spawnedBodies.Count; i++)
		{
			GameObject gameObject = this.spawnedBodies[i];
			if (gameObject)
			{
				Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
				if (!rigidbody)
				{
					rigidbody = gameObject.AddComponent<Rigidbody>();
				}
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}
		float num = float.PositiveInfinity;
		float num2 = float.NegativeInfinity;
		for (int j = 0; j < this.spawnedBodies.Count; j++)
		{
			Transform transform = (this.spawnedBodies[j] ? this.spawnedBodies[j].transform : null);
			if (transform)
			{
				float x = transform.position.x;
				if (x < num)
				{
					num = x;
				}
				if (x > num2)
				{
					num2 = x;
				}
			}
		}
		float num3 = Mathf.Max(0.0001f, num2 - num);
		for (int k = 0; k < this.spawnedBodies.Count; k++)
		{
			GameObject gameObject2 = this.spawnedBodies[k];
			if (gameObject2)
			{
				Rigidbody component = gameObject2.GetComponent<Rigidbody>();
				if (component)
				{
					AreaBodiesSpawner.BodyFlowData bodyFlowData;
					if (!this.bodyFlowData.TryGetValue(component, out bodyFlowData))
					{
						bodyFlowData = this.CreateFlowDataFor(gameObject2);
					}
					float x2 = gameObject2.transform.position.x;
					float num4 = (num2 - x2) / num3;
					bodyFlowData.speedJitter = Mathf.Clamp01(num4) * Mathf.Max(0f, this.transformFlowMaxDelay);
					this.bodyFlowData[component] = bodyFlowData;
				}
			}
		}
		this.transformFlowStartTime = Time.time;
	}

	// Token: 0x06003355 RID: 13141 RVA: 0x000F81D0 File Offset: 0x000F63D0
	public void StopFlowTransform()
	{
		this.transformFlowActive = false;
		for (int i = 0; i < this.spawnedBodies.Count; i++)
		{
			GameObject gameObject = this.spawnedBodies[i];
			if (gameObject)
			{
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (component)
				{
					component.isKinematic = true;
				}
			}
		}
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x000F8228 File Offset: 0x000F6428
	public void ClearBodies()
	{
		foreach (GameObject gameObject in this.spawnedBodies)
		{
			if (gameObject)
			{
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (!this.preSetBodies.Contains(component))
				{
					if (Application.isPlaying)
					{
						global::UnityEngine.Object.Destroy(gameObject);
					}
					else
					{
						global::UnityEngine.Object.DestroyImmediate(gameObject);
					}
				}
			}
		}
		this.spawnedBodies.Clear();
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x000F82B4 File Offset: 0x000F64B4
	public void RemoveBody(GameObject body)
	{
		this.spawnedBodies.Remove(body);
		if (Application.isPlaying)
		{
			global::UnityEngine.Object.Destroy(body);
			return;
		}
		global::UnityEngine.Object.DestroyImmediate(body);
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x000F82D8 File Offset: 0x000F64D8
	private void Start()
	{
		GameObject gameObject = this.bodyPrefab;
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		foreach (Rigidbody rigidbody in this.preSetBodies)
		{
			this.presetBodiesPositions.Add(rigidbody.transform.position);
		}
		foreach (GameObject gameObject2 in this.spawnedBodies)
		{
			this.bodiesPositionsOnStart.Add(gameObject2.transform.position);
		}
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x000F83A0 File Offset: 0x000F65A0
	private void FixedUpdate()
	{
		if (this.transformFlowActive || !this.flowActive || this.spawnedBodies.Count == 0)
		{
			return;
		}
		for (int i = 0; i < this.spawnedBodies.Count; i++)
		{
			GameObject gameObject = this.spawnedBodies[i];
			if (gameObject)
			{
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (component)
				{
					AreaBodiesSpawner.BodyFlowData bodyFlowData;
					if (!this.bodyFlowData.TryGetValue(component, out bodyFlowData))
					{
						bodyFlowData = this.CreateFlowDataFor(gameObject);
						this.bodyFlowData[component] = bodyFlowData;
						this.ConfigureRigidbody(component);
					}
					float num = 1f;
					Vector3 vector2;
					if (this.flowSpline != null && this.flowSpline.Spline != null && this.flowSpline.Spline.Count > 1)
					{
						Vector3 position = gameObject.transform.position;
						Vector3 vector = this.flowSpline.transform.InverseTransformPoint(position);
						float3 @float;
						float num2;
						SplineUtility.GetNearestPoint<Spline>(this.flowSpline.Spline, vector, out @float, out num2, 4, 2);
						float3 float2 = this.flowSpline.Spline.EvaluateTangent(num2);
						vector2 = this.flowSpline.transform.TransformDirection(float2).normalized;
						Vector3 vector3 = this.flowSpline.transform.TransformPoint(@float);
						float num3 = Vector3.Distance(position, vector3);
						if (this.flowSplineMaxDistance > 0.001f)
						{
							num = 1f - Mathf.Clamp01(num3 / this.flowSplineMaxDistance);
							num *= num;
						}
					}
					else
					{
						vector2 = Vector3.right;
					}
					float num4 = 1f + bodyFlowData.speedJitter * this.speedRandomness;
					Vector3 vector4 = (vector2 * (this.flowSpeed * num4) - AreaBodiesSpawner.GetLinearVelocity(component)) * Mathf.Max(0f, this.accelerationGain);
					component.AddForce(vector4 * num, ForceMode.Acceleration);
				}
			}
		}
	}

	// Token: 0x0600335A RID: 13146 RVA: 0x000F85A0 File Offset: 0x000F67A0
	private void Update()
	{
		if (!this.transformFlowActive || this.spawnedBodies.Count == 0)
		{
			return;
		}
		if (MainGame.IsGamePaused)
		{
			return;
		}
		Vector3 right = Vector3.right;
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < this.spawnedBodies.Count; i++)
		{
			GameObject gameObject = this.spawnedBodies[i];
			if (gameObject)
			{
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (component)
				{
					AreaBodiesSpawner.BodyFlowData bodyFlowData;
					if (!this.bodyFlowData.TryGetValue(component, out bodyFlowData))
					{
						bodyFlowData = this.CreateFlowDataFor(gameObject);
						this.bodyFlowData[component] = bodyFlowData;
					}
					float num = Mathf.Max(0f, bodyFlowData.speedJitter);
					if (Time.time - this.transformFlowStartTime < num)
					{
						goto IL_00CE;
					}
				}
				gameObject.transform.position += right * this.flowSpeed * deltaTime;
			}
			IL_00CE:;
		}
	}

	// Token: 0x0600335B RID: 13147 RVA: 0x000F8690 File Offset: 0x000F6890
	private void InitBodiesForFlow()
	{
		this.bodyFlowData.Clear();
		for (int i = 0; i < this.spawnedBodies.Count; i++)
		{
			GameObject gameObject = this.spawnedBodies[i];
			if (gameObject)
			{
				Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
				if (!rigidbody)
				{
					rigidbody = gameObject.AddComponent<Rigidbody>();
				}
				rigidbody.useGravity = false;
				rigidbody.isKinematic = false;
				this.ConfigureRigidbody(rigidbody);
				this.bodyFlowData[rigidbody] = this.CreateFlowDataFor(gameObject);
			}
		}
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x000F8714 File Offset: 0x000F6914
	private void EnsureFlowData()
	{
		for (int i = 0; i < this.spawnedBodies.Count; i++)
		{
			GameObject gameObject = this.spawnedBodies[i];
			if (gameObject)
			{
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (component && !this.bodyFlowData.ContainsKey(component))
				{
					this.bodyFlowData[component] = this.CreateFlowDataFor(gameObject);
				}
			}
		}
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x000F877C File Offset: 0x000F697C
	private void ConfigureRigidbody(Rigidbody rb)
	{
		rb.linearDamping = Mathf.Max(0f, this.linearDrag);
		rb.angularDamping = Mathf.Max(0f, this.angularDrag);
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x000F87AC File Offset: 0x000F69AC
	private AreaBodiesSpawner.BodyFlowData CreateFlowDataFor(GameObject go)
	{
		float num = AreaBodiesSpawner.HashTo01(go.GetInstanceID() * 123457 + 76543);
		return new AreaBodiesSpawner.BodyFlowData
		{
			speedJitter = num * 2f - 1f
		};
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x000F87EE File Offset: 0x000F69EE
	private static float HashTo01(int v)
	{
		int num = (v ^ (int)((uint)v >> 17)) * -312814405;
		int num2 = (num ^ (int)((uint)num >> 11)) * -1404298415;
		int num3 = (num2 ^ (int)((uint)num2 >> 15)) * 830770091;
		return ((num3 ^ (int)((uint)num3 >> 14)) & 16777215) / 16777216f;
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x000F8828 File Offset: 0x000F6A28
	private Vector3 GetPrefabHalfExtentsWorld()
	{
		if (!this.bodyPrefab)
		{
			return Vector3.one * 0.5f;
		}
		return AreaBodiesSpawner.ComputeWorldBoundsFromColliders(this.bodyPrefab).extents;
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x000F8868 File Offset: 0x000F6A68
	private static Bounds ComputeWorldBoundsFromColliders(GameObject go)
	{
		Collider[] componentsInChildren = go.GetComponentsInChildren<Collider>(true);
		Bounds? bounds = null;
		foreach (Collider collider in componentsInChildren)
		{
			if (collider)
			{
				Bounds bounds2 = collider.bounds;
				if (bounds == null)
				{
					bounds = new Bounds?(bounds2);
				}
				else
				{
					Bounds value = bounds.Value;
					value.Encapsulate(bounds2.min);
					value.Encapsulate(bounds2.max);
					bounds = new Bounds?(value);
				}
			}
		}
		Bounds? bounds3 = bounds;
		if (bounds3 == null)
		{
			return new Bounds(go.transform.position, Vector3.zero);
		}
		return bounds3.GetValueOrDefault();
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x000F8912 File Offset: 0x000F6B12
	private static Vector3 GetLinearVelocity(Rigidbody rb)
	{
		return rb.linearVelocity;
	}

	// Token: 0x040028FE RID: 10494
	public Collider col;

	// Token: 0x040028FF RID: 10495
	public float baseY;

	// Token: 0x04002900 RID: 10496
	[Range(0f, 1f)]
	public float yRange = 0.2f;

	// Token: 0x04002901 RID: 10497
	public int bodiesCount = 50;

	// Token: 0x04002902 RID: 10498
	public GameObject bodyPrefab;

	// Token: 0x04002903 RID: 10499
	public GameObject bodiesParent;

	// Token: 0x04002904 RID: 10500
	public List<Rigidbody> preSetBodies = new List<Rigidbody>();

	// Token: 0x04002905 RID: 10501
	public SplineContainer flowSpline;

	// Token: 0x04002906 RID: 10502
	[SerializeField]
	private List<GameObject> spawnedBodies = new List<GameObject>();

	// Token: 0x04002907 RID: 10503
	[Header("River Flow Settings")]
	public bool flowActive;

	// Token: 0x04002908 RID: 10504
	public float flowSpeed = 1.5f;

	// Token: 0x04002909 RID: 10505
	[Min(0f)]
	public float accelerationGain = 3f;

	// Token: 0x0400290A RID: 10506
	[Min(0f)]
	public float flowSplineMaxDistance = 10f;

	// Token: 0x0400290B RID: 10507
	[Range(0f, 1f)]
	public float speedRandomness = 0.25f;

	// Token: 0x0400290C RID: 10508
	[Header("Flow Mode")]
	public bool transformFlowActive;

	// Token: 0x0400290D RID: 10509
	[Min(0f)]
	public float transformFlowMaxDelay = 1f;

	// Token: 0x0400290E RID: 10510
	[Header("Rigidbodies")]
	[Min(0f)]
	public float linearDrag = 1f;

	// Token: 0x0400290F RID: 10511
	[Min(0f)]
	public float angularDrag = 0.5f;

	// Token: 0x04002910 RID: 10512
	[Header("Editor No-Overlap Spawn")]
	[Min(0f)]
	public float noOverlapPadding = 0.05f;

	// Token: 0x04002911 RID: 10513
	[Min(1f)]
	public int noOverlapMaxAttemptsPerBody = 200;

	// Token: 0x04002912 RID: 10514
	public List<GameObject> objToDisableOnFlowStart = new List<GameObject>();

	// Token: 0x04002913 RID: 10515
	private List<Vector3> presetBodiesPositions = new List<Vector3>();

	// Token: 0x04002914 RID: 10516
	private List<Vector3> bodiesPositionsOnStart = new List<Vector3>();

	// Token: 0x04002915 RID: 10517
	private readonly Dictionary<Rigidbody, AreaBodiesSpawner.BodyFlowData> bodyFlowData = new Dictionary<Rigidbody, AreaBodiesSpawner.BodyFlowData>();

	// Token: 0x04002916 RID: 10518
	private float transformFlowStartTime;

	// Token: 0x020007CD RID: 1997
	private struct BodyFlowData
	{
		// Token: 0x04002917 RID: 10519
		public float speedJitter;
	}
}
