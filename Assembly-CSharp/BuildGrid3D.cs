using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class BuildGrid3D : MonoBehaviour
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00024F7D File Offset: 0x0002317D
	public HashSet<PreSetModuleBuildView> PreSetModuleBuildViews
	{
		get
		{
			return this.preSetModuleBuildViews;
		}
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00024F88 File Offset: 0x00023188
	public void Init()
	{
		this.preSetModuleBuildViewPrefab.gameObject.SetActive(false);
		this.buffAreaPrefab.gameObject.SetActive(false);
		this.preSetModuleBuildViewPool = LazyPooler.CreatePool<PreSetModuleBuildView>(this.preSetModuleBuildViewPrefab, 5, Pool.PoolType.ImmediateActivation, false, false, null);
		this.buffAreaPool = LazyPooler.CreatePool<BuffArea>(this.buffAreaPrefab, 5, Pool.PoolType.ImmediateActivation, false, false, null);
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x00024FE3 File Offset: 0x000231E3
	public void SetAllowedExtensionWgoIds(HashSet<string> allowed)
	{
		this.allowedExtensionWgoIds = ((allowed != null) ? new HashSet<string>(allowed) : null);
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x00024FF7 File Offset: 0x000231F7
	public void SetElevationAreas(List<BuildElevationArea> areas)
	{
		this.elevationAreas = areas;
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00025000 File Offset: 0x00023200
	public void Draw(BuildCellData[,] gridData, BuildCellSelectionData[,] selectionData, BuildCellBuffUsageData[,] buffUsageData, BuildCellData.BuildMode buildMode, bool drawExtensions = true)
	{
		int length = gridData.GetLength(0);
		int length2 = gridData.GetLength(1);
		int num = gridData.GetLength(0) / 2;
		int num2 = gridData.GetLength(1) / 2;
		HashSet<BuildArea> hashSet = new HashSet<BuildArea>();
		foreach (BuffArea buffArea in this.activeBuffAreas)
		{
			this.buffAreaPool.ReleaseObject<BuffArea>(buffArea);
		}
		this.activeBuffAreas.Clear();
		this.buffAreaData.Clear();
		for (int i = -num; i < num; i++)
		{
			for (int j = -num2; j < num2; j++)
			{
				int num3 = i + num;
				int num4 = j + num2;
				BuildCellData buildCellData = gridData[num3, num4];
				float num5 = (float)((buildCellData.State >> 1) & 1) * 0.1f;
				int num6 = (buildCellData.State >> 2) & 1;
				if (((buildCellData.State >> 4) & 1) != 0)
				{
					num6 |= 2;
				}
				if (((buildCellData.State >> 5) & 1) != 0)
				{
					num6 |= 4;
				}
				float num7 = (float)num6 * 0.1f;
				int num8 = 0;
				num8 |= ((((buildCellData.State >> 3) & 1) != 0) ? 1 : 0);
				num8 |= ((((buildCellData.State >> 6) & 1) != 0) ? 2 : 0);
				float num9 = (float)num8 * 0.1f;
				if (num9 > 0f && drawExtensions)
				{
					foreach (SGuid sguid in buildCellData.ExtensionList)
					{
						if (this.allowedExtensionWgoIds != null)
						{
							Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(sguid);
							if (wgoViewGlobal == null || !this.allowedExtensionWgoIds.Contains(wgoViewGlobal.Data.id))
							{
								continue;
							}
						}
						BuildGrid3D.BuffAreaData buffAreaData;
						if (!this.buffAreaData.TryGetValue(sguid, out buffAreaData))
						{
							buffAreaData = new BuildGrid3D.BuffAreaData
							{
								holderId = sguid
							};
							this.buffAreaData[sguid] = buffAreaData;
						}
						buffAreaData.AddCell(buildCellData.Coords);
					}
				}
				int num10 = 512 - (-i + 256);
				int num11 = 512 - (-j + 256);
				this.gridColorData[num10 + num11 * 512] = new Color(num5, num7, num9, 1f);
				int num12 = 0;
				if (selectionData != null)
				{
					num12 |= selectionData[num3, num4].state & 1;
				}
				if (buffUsageData != null && buffUsageData.GetLength(0) == length && buffUsageData.GetLength(1) == length2)
				{
					num12 |= buffUsageData[num3, num4].state & 2;
				}
				else if ((num8 & 2) != 0)
				{
					num12 |= 2;
				}
				float num13 = (float)num12 * 0.1f;
				float num14 = ((buildCellData.BuildAreaWithCovering != null) ? 0.1f : 0f);
				this.selectionColorData[num10 + num11 * 512] = new Color(num13, num14, 0f, 1f);
				if (!this.wereModulesDrawn && buildCellData.BuildAreaWithCovering && hashSet.Add(buildCellData.BuildAreaWithCovering))
				{
					PreSetModuleBuildView orCreateObject = this.preSetModuleBuildViewPool.GetOrCreateObject<PreSetModuleBuildView>();
					orCreateObject.transform.SetParent(base.transform);
					orCreateObject.SetPositionAndScaleAs(buildCellData.BuildAreaWithCovering);
					this.preSetModuleBuildViews.Add(orCreateObject);
				}
			}
		}
		foreach (KeyValuePair<SGuid, BuildGrid3D.BuffAreaData> keyValuePair in this.buffAreaData)
		{
			ValueTuple<Texture2D, Vector3, Vector2> valueTuple = keyValuePair.Value.CreateTexture();
			Texture2D item = valueTuple.Item1;
			Vector3 item2 = valueTuple.Item2;
			Vector2 item3 = valueTuple.Item3;
			BuffArea orCreateObject2 = this.buffAreaPool.GetOrCreateObject<BuffArea>();
			orCreateObject2.transform.SetParent(base.transform);
			orCreateObject2.Draw(item, item2, item3);
			this.activeBuffAreas.Add(orCreateObject2);
		}
		this.buildMode = (int)buildMode;
		this.CreateMaterial();
		this.ApplyDataToMaterial();
		this.DrawElevationQuads(gridData);
		this.wereModulesDrawn = true;
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00025440 File Offset: 0x00023640
	public void UpdateSelection(BuildCellSelectionData[,] selectionData, BuildCellBuffUsageData[,] buffUsageData)
	{
		if (selectionData == null && buffUsageData == null)
		{
			return;
		}
		this.CreateMaterial();
		int num = ((selectionData != null) ? selectionData.GetLength(0) : buffUsageData.GetLength(0));
		int num2 = ((selectionData != null) ? selectionData.GetLength(1) : buffUsageData.GetLength(1));
		int num3 = num / 2;
		int num4 = num2 / 2;
		for (int i = -num3; i < num3; i++)
		{
			for (int j = -num4; j < num4; j++)
			{
				int num5 = i + num3;
				int num6 = j + num4;
				int num7 = 512 - (-i + 256);
				int num8 = 512 - (-j + 256);
				int num9 = 0;
				if (selectionData != null)
				{
					num9 |= selectionData[num5, num6].state & 1;
				}
				if (buffUsageData != null)
				{
					num9 |= buffUsageData[num5, num6].state & 2;
				}
				float num10 = (float)num9 * 0.1f;
				int num11 = num7 + num8 * 512;
				float g = this.selectionColorData[num11].g;
				this.selectionColorData[num11] = new Color(num10, g, 0f, 1f);
			}
		}
		this.selectionTexture.SetPixels(this.selectionColorData);
		this.selectionTexture.Apply();
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00025578 File Offset: 0x00023778
	public void Clear()
	{
		foreach (PreSetModuleBuildView preSetModuleBuildView in this.preSetModuleBuildViews)
		{
			this.preSetModuleBuildViewPool.ReleaseObject<PreSetModuleBuildView>(preSetModuleBuildView);
		}
		foreach (BuffArea buffArea in this.activeBuffAreas)
		{
			this.buffAreaPool.ReleaseObject<BuffArea>(buffArea);
		}
		this.activeBuffAreas.Clear();
		this.buffAreaData.Clear();
		this.preSetModuleBuildViews.Clear();
		for (int i = 0; i < this.elevationQuads.Count; i++)
		{
			this.elevationQuads[i].Hide();
		}
		this.wereModulesDrawn = false;
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0002566C File Offset: 0x0002386C
	private void CreateMaterial()
	{
		if (this.materialInstance == null)
		{
			if (this.meshRenderer == null)
			{
				return;
			}
			this.materialInstance = new Material(this.material);
			this.texture = new Texture2D(512, 512, TextureFormat.RGBA32, true)
			{
				filterMode = FilterMode.Point
			};
			this.selectionTexture = new Texture2D(512, 512, TextureFormat.RGBA32, true)
			{
				filterMode = FilterMode.Point
			};
		}
		this.meshRenderer.material = this.materialInstance;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x000256F4 File Offset: 0x000238F4
	private void DrawElevationQuads(BuildCellData[,] gridData)
	{
		for (int i = 0; i < this.elevationQuads.Count; i++)
		{
			this.elevationQuads[i].Hide();
		}
		if (this.elevationAreas == null || this.elevationAreas.Count == 0 || gridData == null)
		{
			return;
		}
		int length = gridData.GetLength(0);
		int length2 = gridData.GetLength(1);
		int num = length / 2;
		int num2 = length2 / 2;
		Vector2 cell_SIZE = BuildConsts.CELL_SIZE;
		int num3 = 0;
		for (int j = 0; j < this.elevationAreas.Count; j++)
		{
			Rect groundRect = this.elevationAreas[j].GroundRect;
			int num4 = int.MaxValue;
			int num5 = int.MaxValue;
			int num6 = int.MinValue;
			int num7 = int.MinValue;
			float num8 = float.MaxValue;
			float num9 = float.MaxValue;
			for (int k = 0; k < length; k++)
			{
				for (int l = 0; l < length2; l++)
				{
					if (gridData[k, l].State != 0)
					{
						Vector3 coords = gridData[k, l].Coords;
						if (groundRect.Contains(new Vector2(coords.x, coords.z)))
						{
							if (k < num4)
							{
								num4 = k;
							}
							if (l < num5)
							{
								num5 = l;
							}
							if (k > num6)
							{
								num6 = k;
							}
							if (l > num7)
							{
								num7 = l;
							}
							if (coords.x < num8)
							{
								num8 = coords.x;
							}
							if (coords.z < num9)
							{
								num9 = coords.z;
							}
						}
					}
				}
			}
			if (num6 < num4)
			{
				Debug.LogWarning(string.Format("[ElevationQuad] area#{0} rect={1} -> NO scanned cells inside footprint (quad skipped)", j, groundRect));
			}
			else
			{
				int num10 = num6 - num4 + 1;
				int num11 = num7 - num5 + 1;
				int num12 = Mathf.Max(num10, num11) + 2;
				Color[] array = new Color[num12 * num12];
				Color[] array2 = new Color[num12 * num12];
				for (int m = num4; m <= num6; m++)
				{
					for (int n = num5; n <= num7; n++)
					{
						if (gridData[m, n].State != 0)
						{
							Vector3 coords2 = gridData[m, n].Coords;
							if (groundRect.Contains(new Vector2(coords2.x, coords2.z)))
							{
								int num13 = 256 + m - num + (256 + n - num2) * 512;
								int num14 = m - num4 + 1 + (n - num5 + 1) * num12;
								array[num14] = this.gridColorData[num13];
								array2[num14] = this.selectionColorData[num13];
							}
						}
					}
				}
				float num15 = num8 + ((float)num12 * 0.5f - 1.5f) * cell_SIZE.x;
				float num16 = num9 + ((float)num12 * 0.5f - 1.5f) * cell_SIZE.y;
				Vector3 vector = VisualConsts.ProjectGroundPointToElevation(new Vector3(num15, this.elevationAreas[j].GroundY, num16), this.elevationAreas[j].ElevationY) + VisualConsts.GetLayerOffset(2);
				vector.z += cell_SIZE.y;
				vector.y += 0.005f;
				Vector2 vector2 = new Vector2((float)num12 * cell_SIZE.x, (float)num12 * cell_SIZE.y);
				this.GetOrCreateElevationQuad(num3).Draw(array, array2, num12, vector, vector2, this.buildMode);
				num3++;
			}
		}
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00025A84 File Offset: 0x00023C84
	private ElevationGridQuad GetOrCreateElevationQuad(int index)
	{
		if (index < this.elevationQuads.Count)
		{
			return this.elevationQuads[index];
		}
		Transform transform = ((base.transform.parent != null) ? base.transform.parent : base.transform);
		GameObject gameObject = new GameObject("ElevationGridQuad");
		gameObject.transform.SetParent(transform, false);
		ElevationGridQuad elevationGridQuad = gameObject.AddComponent<ElevationGridQuad>();
		elevationGridQuad.Setup(this.material);
		this.elevationQuads.Add(elevationGridQuad);
		return elevationGridQuad;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00025B0C File Offset: 0x00023D0C
	private void ApplyDataToMaterial()
	{
		this.texture.SetPixels(this.gridColorData);
		this.texture.Apply();
		this.selectionTexture.SetPixels(this.selectionColorData);
		this.selectionTexture.Apply();
		this.materialInstance.SetTexture(BuildGrid3D.shaderIdDataTexture, this.texture);
		this.materialInstance.SetTexture(BuildGrid3D.shaderIdSelectionTexture, this.selectionTexture);
		this.materialInstance.SetInt(BuildGrid3D.shaderIdBuildMode, this.buildMode);
		this.materialInstance.SetVector(BuildGrid3D.shaderIdObjectScale, base.transform.lossyScale);
	}

	// Token: 0x04000998 RID: 2456
	private const int TEXTURE_SIZE = 512;

	// Token: 0x04000999 RID: 2457
	private const int HALF_TEXTURE_SIZE = 256;

	// Token: 0x0400099A RID: 2458
	[SerializeField]
	private MeshRenderer meshRenderer;

	// Token: 0x0400099B RID: 2459
	[SerializeField]
	private Material material;

	// Token: 0x0400099C RID: 2460
	[SerializeField]
	private PreSetModuleBuildView preSetModuleBuildViewPrefab;

	// Token: 0x0400099D RID: 2461
	private HashSet<PreSetModuleBuildView> preSetModuleBuildViews = new HashSet<PreSetModuleBuildView>();

	// Token: 0x0400099E RID: 2462
	private Pool preSetModuleBuildViewPool;

	// Token: 0x0400099F RID: 2463
	[SerializeField]
	private BuffArea buffAreaPrefab;

	// Token: 0x040009A0 RID: 2464
	private List<BuffArea> activeBuffAreas = new List<BuffArea>();

	// Token: 0x040009A1 RID: 2465
	private Pool buffAreaPool;

	// Token: 0x040009A2 RID: 2466
	private readonly List<ElevationGridQuad> elevationQuads = new List<ElevationGridQuad>();

	// Token: 0x040009A3 RID: 2467
	private List<BuildElevationArea> elevationAreas;

	// Token: 0x040009A4 RID: 2468
	private bool wereModulesDrawn;

	// Token: 0x040009A5 RID: 2469
	private readonly Color[] gridColorData = new Color[262144];

	// Token: 0x040009A6 RID: 2470
	private readonly Color[] selectionColorData = new Color[262144];

	// Token: 0x040009A7 RID: 2471
	private int buildMode;

	// Token: 0x040009A8 RID: 2472
	private Texture2D texture;

	// Token: 0x040009A9 RID: 2473
	private Texture2D selectionTexture;

	// Token: 0x040009AA RID: 2474
	private Material materialInstance;

	// Token: 0x040009AB RID: 2475
	private Dictionary<SGuid, BuildGrid3D.BuffAreaData> buffAreaData = new Dictionary<SGuid, BuildGrid3D.BuffAreaData>();

	// Token: 0x040009AC RID: 2476
	private HashSet<string> allowedExtensionWgoIds;

	// Token: 0x040009AD RID: 2477
	private static readonly int shaderIdDataTexture = Shader.PropertyToID("_DataTex");

	// Token: 0x040009AE RID: 2478
	private static readonly int shaderIdSelectionTexture = Shader.PropertyToID("_SelectionTex");

	// Token: 0x040009AF RID: 2479
	private static readonly int shaderIdObjectScale = Shader.PropertyToID("_ObjectScale");

	// Token: 0x040009B0 RID: 2480
	private static readonly int shaderIdBuildMode = Shader.PropertyToID("_BuildMode");

	// Token: 0x02000144 RID: 324
	private class BuffAreaData
	{
		// Token: 0x060007BD RID: 1981 RVA: 0x00025C51 File Offset: 0x00023E51
		public void AddCell(Vector3 coords)
		{
			this.cellCoords.Add(coords);
			this.min = Vector3.Min(this.min, coords);
			this.max = Vector3.Max(this.max, coords);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00025C84 File Offset: 0x00023E84
		[return: TupleElementNames(new string[] { "texture", "position", "size" })]
		public ValueTuple<Texture2D, Vector3, Vector2> CreateTexture()
		{
			Vector2 cell_SIZE = BuildConsts.CELL_SIZE;
			int num = Mathf.RoundToInt((this.max.x - this.min.x) / cell_SIZE.x) + 1;
			int num2 = Mathf.RoundToInt((this.max.z - this.min.z) / cell_SIZE.y) + 1;
			int num3 = num + 2;
			int num4 = num2 + 2;
			Debug.Log(string.Format("base size: {0}x{1}, expanded: {2}x{3}", new object[] { num, num2, num3, num4 }));
			Texture2D texture2D = new Texture2D(num3, num4, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Clamp
			};
			Color32[] array = new Color32[num3 * num4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Color32(0, 0, 0, 0);
			}
			foreach (Vector3 vector in this.cellCoords)
			{
				int num5 = Mathf.RoundToInt((vector.x - this.min.x) / cell_SIZE.x) + 1;
				int num6 = Mathf.RoundToInt((vector.z - this.min.z) / cell_SIZE.y) + 1;
				if (num5 >= 0 && num5 < num3 && num6 >= 0 && num6 < num4)
				{
					array[num5 + num6 * num3] = new Color(0.1f, 0f, 0f, 1f);
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			Vector3 vector2 = (this.min + this.max) * 0.5f;
			Vector2 vector3 = new Vector2((float)num3 * cell_SIZE.x, (float)num4 * cell_SIZE.y);
			return new ValueTuple<Texture2D, Vector3, Vector2>(texture2D, vector2, vector3);
		}

		// Token: 0x040009B1 RID: 2481
		public SGuid holderId;

		// Token: 0x040009B2 RID: 2482
		public List<Vector3> cellCoords = new List<Vector3>();

		// Token: 0x040009B3 RID: 2483
		public Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

		// Token: 0x040009B4 RID: 2484
		public Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
	}
}
