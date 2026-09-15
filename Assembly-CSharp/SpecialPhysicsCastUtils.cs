using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LinqTools;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

// Token: 0x02000B07 RID: 2823
public static class SpecialPhysicsCastUtils
{
	// Token: 0x06004B2F RID: 19247 RVA: 0x00162554 File Offset: 0x00160754
	public static void GetLinecastVisibility(Vector3 origin, IReadOnlyList<Vector3> destinations, int obstacleLayerMask, List<bool> visibilityResults, int maxHitsPerCommand = 30)
	{
		if (destinations == null)
		{
			throw new ArgumentNullException("destinations");
		}
		if (visibilityResults == null)
		{
			throw new ArgumentNullException("visibilityResults");
		}
		if (maxHitsPerCommand <= 0)
		{
			throw new ArgumentOutOfRangeException("maxHitsPerCommand", "maxHitsPerCommand must be greater than zero.");
		}
		visibilityResults.Clear();
		int count = destinations.Count;
		if (count == 0)
		{
			return;
		}
		NativeArray<RaycastCommand> nativeArray = new NativeArray<RaycastCommand>(count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		NativeArray<RaycastHit> nativeArray2 = new NativeArray<RaycastHit>(count * maxHitsPerCommand, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		NativeArray<byte> nativeArray3 = new NativeArray<byte>(count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		QueryParameters queryParameters = new QueryParameters(obstacleLayerMask, true, QueryTriggerInteraction.Ignore, false);
		for (int i = 0; i < count; i++)
		{
			Vector3 vector = destinations[i] - origin;
			float magnitude = vector.magnitude;
			if (magnitude <= Mathf.Epsilon)
			{
				nativeArray[i] = new RaycastCommand(Physics.defaultPhysicsScene, origin, Vector3.forward, queryParameters, 0f);
				nativeArray3[i] = 1;
			}
			else
			{
				Vector3 vector2 = vector / magnitude;
				nativeArray[i] = new RaycastCommand(Physics.defaultPhysicsScene, origin, vector2, queryParameters, magnitude);
			}
		}
		int num = Mathf.Clamp(count / 4, 1, count);
		RaycastCommand.ScheduleBatch(nativeArray, nativeArray2, num, maxHitsPerCommand, default(JobHandle)).Complete();
		for (int j = 0; j < count; j++)
		{
			if (nativeArray3[j] == 1)
			{
				visibilityResults.Add(true);
			}
			else
			{
				bool flag = false;
				int num2 = j * maxHitsPerCommand;
				for (int k = 0; k < maxHitsPerCommand; k++)
				{
					if (nativeArray2[num2 + k].collider)
					{
						flag = true;
						break;
					}
				}
				visibilityResults.Add(!flag);
			}
		}
		nativeArray2.Dispose();
		nativeArray.Dispose();
		nativeArray3.Dispose();
	}

	// Token: 0x06004B30 RID: 19248 RVA: 0x00162700 File Offset: 0x00160900
	public static bool GetSpawnPosForBigDropRadial(Vector3 searchPoint, float searchRadius, Vector3 size, out Vector3 foundDropPos)
	{
		foundDropPos = default(Vector3);
		float num = Mathf.Atan2(size.x / 2f, searchRadius);
		int num2 = Mathf.CeilToInt(6.2831855f / num);
		float num3 = 0.7853982f;
		float num4 = global::UnityEngine.Random.Range(-num3, num3);
		int num5 = 10;
		int num6 = num2 * num5;
		NativeArray<RaycastHit> nativeArray = new NativeArray<RaycastHit>(num6, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		NativeArray<BoxcastCommand> nativeArray2 = new NativeArray<BoxcastCommand>(num2, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		QueryParameters queryParameters = new QueryParameters(7424, false, QueryTriggerInteraction.Ignore, true);
		Vector3 vector = size / 2f;
		Vector3 down = Vector3.down;
		float num7 = 10f;
		for (int i = 0; i < num2; i++)
		{
			Vector3 vector2 = searchPoint + Vector3.Scale(new Vector3(searchRadius, 0f, searchRadius), new Vector3(Mathf.Cos(num * (float)i + num4), 0f, Mathf.Sin(num * (float)i + num4))) + Vector3.up * num7 / 2f;
			nativeArray2[i] = new BoxcastCommand(vector2, vector, Quaternion.identity, down, queryParameters, 5f);
			Debug.DrawLine(vector2 - Vector3.up * num7 / 2f, vector2 + Vector3.up, Color.cyan, 2f);
		}
		BoxcastCommand.ScheduleBatch(nativeArray2, nativeArray, 10, num5, default(JobHandle)).Complete();
		int j = 0;
		bool flag = false;
		Vector3 vector3 = default(Vector3);
		while (j < num2)
		{
			bool flag2 = false;
			bool flag3 = false;
			for (int k = 0; k < num5; k++)
			{
				RaycastHit raycastHit = nativeArray[j * num5 + k];
				if (!(raycastHit.collider == null))
				{
					int num8 = raycastHit.collider.gameObject.layer;
					if (num8 == 10 || num8 == 8)
					{
						flag3 = true;
						break;
					}
					if (!flag2)
					{
						num8 = raycastHit.collider.gameObject.layer;
						if (num8 == 11 || num8 == 12)
						{
							flag2 = true;
							vector3 = raycastHit.point;
						}
					}
				}
			}
			if (flag2 && !flag3)
			{
				flag = true;
				foundDropPos = vector3;
				break;
			}
			j++;
		}
		nativeArray.Dispose();
		nativeArray2.Dispose();
		return flag;
	}

	// Token: 0x06004B31 RID: 19249 RVA: 0x00162940 File Offset: 0x00160B40
	public static bool GetSpawnPosForBigDropRectangular(Vector3 searchPoint, Vector3 size, float xStepSize, int xStepsCount, float zStepSize, int zStepsCount, out Vector3 foundDropPos, IReadOnlyList<Vector3> occupiedPositions = null)
	{
		foundDropPos = default(Vector3);
		int num = xStepsCount * zStepsCount;
		int num2 = 10;
		int num3 = num * num2;
		float num4 = Mathf.Max(size.x, size.z);
		float num5 = num4 * num4;
		NativeArray<RaycastHit> nativeArray = new NativeArray<RaycastHit>(num3, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		NativeArray<BoxcastCommand> nativeArray2 = new NativeArray<BoxcastCommand>(num, Allocator.TempJob, NativeArrayOptions.ClearMemory);
		QueryParameters queryParameters = new QueryParameters(72960, false, QueryTriggerInteraction.Ignore, true);
		Vector3 vector = size / 2f;
		Vector3 down = Vector3.down;
		float num6 = 10f;
		float centerX = (float)(xStepsCount - 1) / 2f;
		float centerZ = (float)(zStepsCount - 1) / 2f;
		ValueTuple<int, int>[] array = new ValueTuple<int, int>[num];
		int num7 = 0;
		for (int i = 0; i < xStepsCount; i++)
		{
			for (int j = 0; j < zStepsCount; j++)
			{
				array[num7++] = new ValueTuple<int, int>(i, j);
			}
		}
		Array.Sort<ValueTuple<int, int>>(array, delegate([TupleElementNames(new string[] { "ix", "iz" })] ValueTuple<int, int> a, [TupleElementNames(new string[] { "ix", "iz" })] ValueTuple<int, int> b)
		{
			float num11 = ((float)a.Item1 - centerX) * ((float)a.Item1 - centerX) + ((float)a.Item2 - centerZ) * ((float)a.Item2 - centerZ);
			float num12 = ((float)b.Item1 - centerX) * ((float)b.Item1 - centerX) + ((float)b.Item2 - centerZ) * ((float)b.Item2 - centerZ);
			return num11.CompareTo(num12);
		});
		for (int k = 0; k < num; k++)
		{
			ValueTuple<int, int> valueTuple = array[k];
			int item = valueTuple.Item1;
			float item2 = (float)valueTuple.Item2;
			float num8 = ((float)item - centerX) * xStepSize;
			float num9 = (item2 - centerZ) * zStepSize;
			Vector3 vector2 = searchPoint + new Vector3(num8, num6 / 2f, num9);
			nativeArray2[k] = new BoxcastCommand(vector2, vector, Quaternion.identity, down, queryParameters, 5f);
		}
		BoxcastCommand.ScheduleBatch(nativeArray2, nativeArray, 10, num2, default(JobHandle)).Complete();
		int l = 0;
		bool flag = false;
		Vector3 vector3 = default(Vector3);
		while (l < num)
		{
			bool flag2 = false;
			bool flag3 = false;
			for (int m = 0; m < num2; m++)
			{
				RaycastHit raycastHit = nativeArray[l * num2 + m];
				if (!(raycastHit.collider == null))
				{
					int num10 = raycastHit.collider.gameObject.layer;
					if (num10 == 10 || num10 == 8 || num10 == 16)
					{
						Debug.DrawLine(raycastHit.point, raycastHit.point + Vector3.up * 0.5f, Color.red, 2f);
						flag3 = true;
						break;
					}
					if (!flag2)
					{
						num10 = raycastHit.collider.gameObject.layer;
						if (num10 == 11 || num10 == 12)
						{
							Debug.DrawLine(raycastHit.point, raycastHit.point + Vector3.up * 0.5f, Color.green, 2f);
							flag2 = true;
							vector3 = raycastHit.point;
						}
					}
				}
			}
			if (flag2 && !flag3 && SpecialPhysicsCastUtils.IsReachableDropElevation(searchPoint.y, vector3.y) && !SpecialPhysicsCastUtils.IsOccupiedByDropPosition(vector3, occupiedPositions, num5))
			{
				flag = true;
				foundDropPos = vector3;
				break;
			}
			l++;
		}
		nativeArray.Dispose();
		nativeArray2.Dispose();
		return flag;
	}

	// Token: 0x06004B32 RID: 19250 RVA: 0x00162C2A File Offset: 0x00160E2A
	public static bool IsReachableDropElevation(float anchorY, float foundGroundY)
	{
		return Mathf.Abs(anchorY - foundGroundY) <= 1.5f;
	}

	// Token: 0x06004B33 RID: 19251 RVA: 0x00162C40 File Offset: 0x00160E40
	private static bool IsOccupiedByDropPosition(Vector3 candidate, IReadOnlyList<Vector3> occupiedPositions, float occupyRadiusSq)
	{
		if (occupiedPositions == null || occupiedPositions.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < occupiedPositions.Count; i++)
		{
			Vector3 vector = occupiedPositions[i];
			float num = candidate.x - vector.x;
			float num2 = candidate.z - vector.z;
			if (num * num + num2 * num2 <= occupyRadiusSq)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004B34 RID: 19252 RVA: 0x00162C9C File Offset: 0x00160E9C
	public static bool GetPlayerDropPosition(Vector3 playerPos, Vector2 playerDir, out Vector3 foundDropPos)
	{
		foundDropPos = default(Vector3);
		float[] array = new float[]
		{
			0.75f, 0.85f, 0.95f, 1.05f, 1.15f, 1.25f, 1.35f, 1.45f, 1.55f, 1.65f,
			1.75f, 1.85f, 1.95f
		};
		float num = 15f;
		Vector3 normalized = new Vector3(playerDir.x, 0f, playerDir.y).normalized;
		foreach (float num2 in array)
		{
			int num3 = Mathf.CeilToInt(360f / num);
			int num4 = 20;
			int num5 = num3 * num4;
			NativeArray<RaycastHit> nativeArray = new NativeArray<RaycastHit>(num5, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<BoxcastCommand> nativeArray2 = new NativeArray<BoxcastCommand>(num3, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			QueryParameters queryParameters = new QueryParameters(7424, true, QueryTriggerInteraction.Ignore, true);
			Vector3 down = Vector3.down;
			Vector3 vector = LazyConsts.BIG_DROP_COLLIDER_SIZE / 2f;
			for (int j = 0; j < num3; j++)
			{
				float num6;
				if (j == 0)
				{
					num6 = 0f;
				}
				else if (j % 2 == 1)
				{
					num6 = (float)(-(float)((j + 1) / 2)) * num;
				}
				else
				{
					num6 = (float)(j / 2) * num;
				}
				Vector3 vector2 = playerPos + (Quaternion.Euler(0f, num6, 0f) * normalized).normalized * num2 + Vector3.up * 2f;
				nativeArray2[j] = new BoxcastCommand(vector2, vector, Quaternion.identity, down, queryParameters, 5f);
				Color color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 0.2f);
				Debug.DrawLine(vector2, vector2 + down * 2f, color, 5f);
			}
			BoxcastCommand.ScheduleBatch(nativeArray2, nativeArray, 10, num4, default(JobHandle)).Complete();
			for (int k = 0; k < num3; k++)
			{
				bool flag = false;
				bool flag2 = false;
				Vector3 zero = Vector3.zero;
				for (int l = 0; l < num4; l++)
				{
					RaycastHit raycastHit = nativeArray[k * num4 + l];
					if (!(raycastHit.collider == null))
					{
						WaterComponentTag waterComponentTag;
						if (raycastHit.collider.TryGetComponent<WaterComponentTag>(out waterComponentTag))
						{
							flag2 = true;
							break;
						}
						int layer = raycastHit.collider.gameObject.layer;
						if (layer == 11 || layer == 12)
						{
							if (SpecialPhysicsCastUtils.IsReachableDropElevation(playerPos.y, raycastHit.point.y))
							{
								flag = true;
								zero = new Vector3(nativeArray2[k].center.x, raycastHit.point.y, nativeArray2[k].center.z);
								break;
							}
						}
						else if (layer == 8)
						{
							flag2 = true;
							break;
						}
					}
				}
				if (flag && !flag2 && !SpecialPhysicsCastUtils.IsOverlappingSomething(zero, vector, Quaternion.identity))
				{
					Vector3 vector3 = Vector3.up * 0.6f;
					Vector3 vector4 = playerPos + vector3;
					Vector3 vector5 = zero + vector3;
					Vector3 normalized2 = (vector5 - vector4).normalized;
					float num7 = Vector3.Distance(vector4, vector5);
					RaycastHit raycastHit2;
					if (!Physics.Raycast(vector4, normalized2, out raycastHit2, num7, 3328, QueryTriggerInteraction.Ignore))
					{
						foundDropPos = zero;
						Vector3 vector6;
						if (SpecialPhysicsCastUtils.TrySnapDropPosToTopmostGround(foundDropPos, out vector6))
						{
							if (!SpecialPhysicsCastUtils.IsReachableDropElevation(playerPos.y, vector6.y))
							{
								goto IL_03BB;
							}
							foundDropPos = vector6;
						}
						Debug.DrawLine(foundDropPos, foundDropPos + Vector3.up * 2f, Color.green, 5f);
						Debug.DrawLine(vector4, vector5, Color.green, 5f);
						nativeArray.Dispose();
						nativeArray2.Dispose();
						return true;
					}
					Debug.DrawLine(vector4, vector5, Color.red, 5f);
				}
				IL_03BB:;
			}
			nativeArray.Dispose();
			nativeArray2.Dispose();
		}
		foundDropPos = playerPos + Vector3.up * 2f;
		Vector3 vector7;
		if (SpecialPhysicsCastUtils.TrySnapDropPosToTopmostGround(playerPos, out vector7))
		{
			foundDropPos = vector7;
		}
		return false;
	}

	// Token: 0x06004B35 RID: 19253 RVA: 0x001630C0 File Offset: 0x001612C0
	public static bool TrySnapDropPosToTopmostGround(Vector3 worldPos, out Vector3 snapped)
	{
		snapped = worldPos;
		RaycastHit raycastHit;
		if (!SpecialPhysicsCastUtils.TryGetTopmostGroundHit(worldPos, out raycastHit))
		{
			return false;
		}
		Vector3 vector = raycastHit.point + raycastHit.normal * 0.02f;
		snapped = new Vector3(worldPos.x, vector.y, worldPos.z);
		return true;
	}

	// Token: 0x06004B36 RID: 19254 RVA: 0x0016311C File Offset: 0x0016131C
	public static bool TryGetTopmostGroundHit(Vector3 worldPos, out RaycastHit bestHit)
	{
		bestHit = default(RaycastHit);
		RaycastHit[] array = Physics.RaycastAll(new Vector3(worldPos.x, worldPos.y + 8f, worldPos.z), Vector3.down, 16f, SpecialPhysicsCastUtils.DropGroundMask, QueryTriggerInteraction.Ignore);
		if (array == null || array.Length == 0)
		{
			return false;
		}
		float num = float.MaxValue;
		bool flag = false;
		foreach (RaycastHit raycastHit in array)
		{
			if (!(raycastHit.collider == null) && !(raycastHit.collider.GetComponentInParent<DropView>() != null) && raycastHit.distance < num)
			{
				num = raycastHit.distance;
				bestHit = raycastHit;
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x06004B37 RID: 19255 RVA: 0x001631CC File Offset: 0x001613CC
	public static bool GetLandPositionByCapsule(Vector3 centerPos, Vector2 preferredDir, float lookDistance, float capsuleRadius, float capsuleHeight, out Vector3 foundDropPos, float angleStep = 20f, int radialSteps = 4)
	{
		foundDropPos = default(Vector3);
		lookDistance = Mathf.Max(0.1f, lookDistance);
		capsuleRadius = Mathf.Max(0.05f, capsuleRadius);
		capsuleHeight = Mathf.Max(capsuleRadius * 2f + 0.05f, capsuleHeight);
		angleStep = Mathf.Clamp(angleStep, 5f, 90f);
		radialSteps = Mathf.Clamp(radialSteps, 1, 12);
		Vector3 forward = new Vector3(preferredDir.x, 0f, preferredDir.y);
		if (forward.sqrMagnitude < 0.0001f)
		{
			forward = Vector3.forward;
		}
		forward.Normalize();
		int num = Mathf.CeilToInt(360f / angleStep);
		float num2 = 2f;
		float num3 = 6f;
		int num4 = 7424;
		int num5 = 1281;
		for (int i = 0; i < radialSteps; i++)
		{
			float num6 = ((float)i + 1f) / (float)radialSteps;
			float num7 = lookDistance * num6;
			for (int j = 0; j < num; j++)
			{
				float num8;
				if (j == 0)
				{
					num8 = 0f;
				}
				else if (j % 2 == 1)
				{
					num8 = (float)(-(float)((j + 1) / 2)) * angleStep;
				}
				else
				{
					num8 = (float)(j / 2) * angleStep;
				}
				Vector3 vector = Quaternion.Euler(0f, num8, 0f) * forward;
				Vector3 vector2 = centerPos + vector * num7;
				RaycastHit raycastHit;
				WaterComponentTag waterComponentTag;
				if (Physics.Raycast(vector2 + Vector3.up * num2, Vector3.down, out raycastHit, num3, num4, QueryTriggerInteraction.Ignore) && (!(raycastHit.collider != null) || !raycastHit.collider.gameObject.TryGetComponent<WaterComponentTag>(out waterComponentTag)))
				{
					int num9 = ((raycastHit.collider != null) ? raycastHit.collider.gameObject.layer : (-1));
					if (num9 == 11 || num9 == 12)
					{
						Vector3 vector3 = new Vector3(vector2.x, raycastHit.point.y, vector2.z);
						float num10 = Mathf.Max(0.01f, capsuleHeight * 0.5f - capsuleRadius);
						Vector3 vector4 = vector3 + Vector3.up * capsuleRadius;
						Vector3 vector5 = vector4 + Vector3.up * (num10 * 2f);
						if (!Physics.CheckCapsule(vector4, vector5, capsuleRadius, 16843008, QueryTriggerInteraction.Ignore))
						{
							Vector3 vector6 = centerPos + Vector3.up * 0.5f;
							Vector3 vector7 = vector3 + Vector3.up * 0.5f - vector6;
							float magnitude = vector7.magnitude;
							if (magnitude <= 0.001f || !Physics.Raycast(vector6, vector7 / magnitude, magnitude, num5, QueryTriggerInteraction.Ignore))
							{
								foundDropPos = vector3;
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06004B38 RID: 19256 RVA: 0x00163484 File Offset: 0x00161684
	private static bool IsOverlappingSomething(Vector3 pos, Vector3 halfExtents, Quaternion orientation)
	{
		return Physics.OverlapBox(pos, halfExtents, orientation, 16843008).Length != 0;
	}

	// Token: 0x06004B39 RID: 19257 RVA: 0x00163498 File Offset: 0x00161698
	public static void GetSweepTriangleOverlap(BoxCollider boxCollider, Vector3 currentPosition, Quaternion currentRotation, Vector3 previousPosition, Quaternion previousRotation, Vector3 currentEndPoint, Vector3 previousEndPoint, int layerMask, HashSet<Collider> results, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
	{
		if (boxCollider == null)
		{
			throw new ArgumentNullException("boxCollider");
		}
		if (results == null)
		{
			throw new ArgumentNullException("results");
		}
		results.Clear();
		Vector3 vector = Vector3.Scale(boxCollider.size * 0.5f, boxCollider.transform.lossyScale);
		Collider[] array = new Collider[32];
		int num = Physics.OverlapBoxNonAlloc(currentPosition, vector, array, currentRotation, layerMask, queryTriggerInteraction);
		for (int i = 0; i < num; i++)
		{
			if (array[i] != boxCollider)
			{
				results.Add(array[i]);
			}
		}
		int num2 = Physics.OverlapBoxNonAlloc(previousPosition, vector, array, previousRotation, layerMask, queryTriggerInteraction);
		for (int j = 0; j < num2; j++)
		{
			if (array[j] != boxCollider)
			{
				results.Add(array[j]);
			}
		}
		Vector3 vector2 = currentEndPoint - previousEndPoint;
		float magnitude = vector2.magnitude;
		if (magnitude > 0.001f)
		{
			Vector3 vector3 = (previousEndPoint + currentEndPoint) * 0.5f;
			Quaternion quaternion = Quaternion.LookRotation(vector2.normalized);
			Vector3 vector4 = new Vector3(vector.x, vector.y, magnitude * 0.5f);
			int num3 = Physics.OverlapBoxNonAlloc(vector3, vector4, array, quaternion, layerMask, queryTriggerInteraction);
			for (int k = 0; k < num3; k++)
			{
				if (array[k] != boxCollider)
				{
					results.Add(array[k]);
				}
			}
		}
	}

	// Token: 0x06004B3A RID: 19258 RVA: 0x001635F8 File Offset: 0x001617F8
	public static void GetSweepTriangleOverlap(BoxCollider boxCollider, Transform endPointTransform, Vector3 previousBoxPosition, Quaternion previousBoxRotation, Vector3 previousEndPoint, int layerMask, HashSet<Collider> results, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
	{
		Vector3 vector = boxCollider.transform.TransformPoint(boxCollider.center);
		Quaternion rotation = boxCollider.transform.rotation;
		Vector3 position = endPointTransform.position;
		SpecialPhysicsCastUtils.GetSweepTriangleOverlap(boxCollider, vector, rotation, previousBoxPosition, previousBoxRotation, position, previousEndPoint, layerMask, results, queryTriggerInteraction);
	}

	// Token: 0x06004B3B RID: 19259 RVA: 0x00163640 File Offset: 0x00161840
	public static bool GetLineSweepHits(Vector3 previousPosition, Vector3 currentPosition, float radius, int layerMask, List<RaycastHit> results, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
	{
		if (results == null)
		{
			throw new ArgumentNullException("results");
		}
		results.Clear();
		Vector3 vector = currentPosition - previousPosition;
		float magnitude = vector.magnitude;
		if (magnitude < 0.001f)
		{
			return false;
		}
		Vector3 vector2 = vector / magnitude;
		RaycastHit[] array;
		if (radius > 0.001f)
		{
			array = Physics.SphereCastAll(previousPosition, radius, vector2, magnitude, layerMask, queryTriggerInteraction);
		}
		else
		{
			array = Physics.RaycastAll(previousPosition, vector2, magnitude, layerMask, queryTriggerInteraction);
		}
		if (array.Length == 0)
		{
			return false;
		}
		Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
		results.AddRange(array);
		return true;
	}

	// Token: 0x06004B3C RID: 19260 RVA: 0x001636DC File Offset: 0x001618DC
	public static bool GetBoxLineSweepHits(BoxCollider boxCollider, Vector3 previousPosition, Vector3 currentPosition, Quaternion orientation, int layerMask, List<RaycastHit> results, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
	{
		if (boxCollider == null)
		{
			throw new ArgumentNullException("boxCollider");
		}
		if (results == null)
		{
			throw new ArgumentNullException("results");
		}
		results.Clear();
		Vector3 vector = currentPosition - previousPosition;
		float magnitude = vector.magnitude;
		if (magnitude < 0.001f)
		{
			return false;
		}
		Vector3 vector2 = vector / magnitude;
		Vector3 vector3 = Vector3.Scale(boxCollider.size * 0.5f, boxCollider.transform.lossyScale);
		RaycastHit[] array = Physics.BoxCastAll(previousPosition, vector3, vector2, orientation, magnitude, layerMask, queryTriggerInteraction);
		if (array.Length == 0)
		{
			return false;
		}
		Array.Sort<RaycastHit>(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
		results.AddRange(array);
		return true;
	}

	// Token: 0x06004B3D RID: 19261 RVA: 0x001637A0 File Offset: 0x001619A0
	public static bool GetLineSweepFirstHit(Vector3 previousPosition, Vector3 currentPosition, float radius, int layerMask, out RaycastHit hit, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Collide)
	{
		hit = default(RaycastHit);
		Vector3 vector = currentPosition - previousPosition;
		float magnitude = vector.magnitude;
		if (magnitude < 0.001f)
		{
			return false;
		}
		Vector3 vector2 = vector / magnitude;
		bool flag;
		if (radius > 0.001f)
		{
			flag = Physics.SphereCast(previousPosition, radius, vector2, out hit, magnitude, layerMask, queryTriggerInteraction);
		}
		else
		{
			flag = Physics.Raycast(previousPosition, vector2, out hit, magnitude, layerMask, queryTriggerInteraction);
		}
		return flag;
	}

	// Token: 0x06004B3E RID: 19262 RVA: 0x00163800 File Offset: 0x00161A00
	public static bool TryGetWgosIntersectedByBuffCollider(Wgo wgo, bool isParentWorkbench, out HashSet<Wgo> intersectedWgos)
	{
		intersectedWgos = new HashSet<Wgo>();
		List<Collider> list = new List<Collider>();
		List<WGODef> list2 = new List<WGODef>();
		if (!isParentWorkbench)
		{
			GameBalance.Me.TryGetParentWorkbenchDefsForExtension(wgo.Id, out list2);
			list = (from col in wgo.GetComponentsInChildren<Collider>()
				where col.gameObject.layer == 28
				select col).ToList<Collider>();
		}
		else
		{
			WGODef workbenchExtensionLogicDef = GameBalance.Me.GetWorkbenchExtensionLogicDef(wgo.Data.id);
			List<WGODef> list3;
			if (workbenchExtensionLogicDef == null)
			{
				list3 = new List<WGODef>();
			}
			else
			{
				list3 = (from id in workbenchExtensionLogicDef.attachedWorkbenchExtensionIds
					select GameBalance.Me.GetData<WGODef>(id) into def
					where def != null
					select def).ToList<WGODef>();
			}
			list2 = list3;
			list = (from col in wgo.GetComponentsInChildren<Collider>()
				where col.gameObject.layer == 19
				select col).ToList<Collider>();
		}
		if (list.Count == 0)
		{
			return false;
		}
		int num = ((!isParentWorkbench) ? 524288 : 268435456);
		Collider[] array = new Collider[20];
		foreach (Collider collider in list)
		{
			int num2 = Physics.OverlapBoxNonAlloc(collider.bounds.center, collider.bounds.extents + Vector3.up * 0.5f - VisualConsts.XYZ_STEP, array, Quaternion.identity, num, QueryTriggerInteraction.Collide);
			for (int i = 0; i < num2; i++)
			{
				Collider collider2 = array[i];
				if (collider2 && !(collider2 == collider))
				{
					Wgo componentInParent = collider2.gameObject.GetComponentInParent<Wgo>();
					if (componentInParent && componentInParent.Data != null && componentInParent.Data.Definition != null && list2.Contains(componentInParent.Data.Definition))
					{
						intersectedWgos.Add(componentInParent);
						Debug.DrawLine(collider.bounds.center, collider2.bounds.center, Color.green, 5f);
					}
				}
			}
		}
		return intersectedWgos.Count > 0;
	}

	// Token: 0x04003CA0 RID: 15520
	private const int POSSIBLE_MASK = 16843008;

	// Token: 0x04003CA1 RID: 15521
	private static readonly int DropGroundMask = 6144;

	// Token: 0x04003CA2 RID: 15522
	private const float DropGroundRayUp = 8f;

	// Token: 0x04003CA3 RID: 15523
	private const float DropGroundRayDistance = 16f;

	// Token: 0x04003CA4 RID: 15524
	private const float DropGroundSnapEpsilon = 0.02f;
}
