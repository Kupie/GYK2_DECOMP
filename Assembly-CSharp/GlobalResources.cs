using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200053E RID: 1342
[CreateAssetMenu(menuName = "GK2/Global Resources")]
public class GlobalResources : LazySingletonSO<GlobalResources>
{
	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x0600227B RID: 8827 RVA: 0x000A1B77 File Offset: 0x0009FD77
	public Material TransparentMaterial
	{
		get
		{
			return this.matsObject3DTransparent[4];
		}
	}

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x0600227C RID: 8828 RVA: 0x000A1B81 File Offset: 0x0009FD81
	public Material TransparentMaterialPlus1
	{
		get
		{
			return this.matsObject3DTransparent[5];
		}
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x000A1B8B File Offset: 0x0009FD8B
	private void OnEnable()
	{
		GlobalResources.aliveInstance = this;
		this.ClearTransparentMaterialAllocations();
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x000A1B99 File Offset: 0x0009FD99
	private void OnDisable()
	{
		if (GlobalResources.aliveInstance == this)
		{
			GlobalResources.aliveInstance = null;
		}
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x000A1BA9 File Offset: 0x0009FDA9
	public static void TryReleaseTransparentMaterialFor(Object3DMesh mesh)
	{
		if (GlobalResources.aliveInstance == null)
		{
			return;
		}
		GlobalResources.aliveInstance.ReleaseTransparentMaterialFor(mesh);
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x000A1BC4 File Offset: 0x0009FDC4
	private void ClearTransparentMaterialAllocations()
	{
		this.object3DMeshes.Clear();
		this.obj3DMeshWithZPos = new ValueTuple<float, Object3DMesh>[9];
		this.lastOccupiedIdx = -1;
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x000A1BE8 File Offset: 0x0009FDE8
	private void PurgeStaleTransparentMaterialAllocations()
	{
		if (this.lastOccupiedIdx < 0)
		{
			return;
		}
		ValueTuple<float, Object3DMesh>[] array = new ValueTuple<float, Object3DMesh>[9];
		int num = 0;
		bool flag = false;
		for (int i = 0; i <= this.lastOccupiedIdx; i++)
		{
			Object3DMesh item = this.obj3DMeshWithZPos[i].Item2;
			if (item == null)
			{
				flag = true;
			}
			else
			{
				array[num] = this.obj3DMeshWithZPos[i];
				this.object3DMeshes[item] = num;
				num++;
			}
		}
		if (!flag)
		{
			return;
		}
		List<Object3DMesh> list = null;
		foreach (KeyValuePair<Object3DMesh, int> keyValuePair in this.object3DMeshes)
		{
			if (keyValuePair.Key == null)
			{
				if (list == null)
				{
					list = new List<Object3DMesh>();
				}
				list.Add(keyValuePair.Key);
			}
		}
		if (list != null)
		{
			for (int j = 0; j < list.Count; j++)
			{
				this.object3DMeshes.Remove(list[j]);
			}
		}
		this.obj3DMeshWithZPos = array;
		this.lastOccupiedIdx = num - 1;
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000A1D10 File Offset: 0x0009FF10
	public bool GetTransparentMaterialFor(Object3DMesh mesh, Material defaultIfNotGet, out Material material, float zOffset = 0f)
	{
		material = defaultIfNotGet;
		if (mesh == null)
		{
			return false;
		}
		int num;
		if (this.object3DMeshes.TryGetValue(mesh, out num))
		{
			material = this.matsObject3DTransparent[num];
			return true;
		}
		this.PurgeStaleTransparentMaterialAllocations();
		if (this.lastOccupiedIdx + 1 >= 9)
		{
			return false;
		}
		for (int i = 0; i < 9; i++)
		{
			float num2 = mesh.transform.position.z + zOffset;
			if (i > this.lastOccupiedIdx || (this.obj3DMeshWithZPos[i].Item1 - num2).EqualsOrMore(0f, 1E-05f))
			{
				this.lastOccupiedIdx++;
				ValueTuple<float, Object3DMesh>[] array = new ValueTuple<float, Object3DMesh>[9];
				for (int j = 0; j < i; j++)
				{
					array[j] = this.obj3DMeshWithZPos[j];
				}
				array[i] = new ValueTuple<float, Object3DMesh>(num2, mesh);
				this.object3DMeshes.Add(mesh, i);
				for (int k = i; k < Mathf.Min(8, this.lastOccupiedIdx); k++)
				{
					int num3 = k + 1;
					array[num3] = this.obj3DMeshWithZPos[k];
					Object3DMesh item = this.obj3DMeshWithZPos[k].Item2;
					if (!(item == null))
					{
						this.object3DMeshes[item] = num3;
						item.ApplyMaterials();
					}
				}
				this.obj3DMeshWithZPos = array;
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x000A1E7C File Offset: 0x000A007C
	public void ReleaseTransparentMaterialFor(Object3DMesh mesh)
	{
		if (mesh == null)
		{
			return;
		}
		int num;
		if (!this.object3DMeshes.Remove(mesh, out num))
		{
			return;
		}
		ValueTuple<float, Object3DMesh>[] array = new ValueTuple<float, Object3DMesh>[9];
		for (int i = 0; i < num; i++)
		{
			array[i] = this.obj3DMeshWithZPos[i];
		}
		for (int j = num; j < Mathf.Min(8, this.lastOccupiedIdx); j++)
		{
			ValueTuple<float, Object3DMesh> valueTuple = this.obj3DMeshWithZPos[j + 1];
			array[j] = valueTuple;
			if (valueTuple.Item2 != null)
			{
				this.object3DMeshes[valueTuple.Item2] = j;
			}
		}
		this.obj3DMeshWithZPos = array;
		int num2 = this.lastOccupiedIdx - 1;
		this.lastOccupiedIdx = num2;
		this.lastOccupiedIdx = Mathf.Clamp(num2, -1, 8);
	}

	// Token: 0x04001F02 RID: 7938
	private const int DEFAULT_TRANSPARENT_MATERIAL_IDX = 4;

	// Token: 0x04001F03 RID: 7939
	private const int TRANSPARENT_MATERIALS_COUNT = 9;

	// Token: 0x04001F04 RID: 7940
	[Header("Materials")]
	public Material matObject3D;

	// Token: 0x04001F05 RID: 7941
	public Material matObject3DDeforming;

	// Token: 0x04001F06 RID: 7942
	public Material matObject3DLUT;

	// Token: 0x04001F07 RID: 7943
	public Material[] matsObject3DTransparent = new Material[9];

	// Token: 0x04001F08 RID: 7944
	public Material matBlackout;

	// Token: 0x04001F09 RID: 7945
	public Material matHorizontalSprite;

	// Token: 0x04001F0A RID: 7946
	public Material matHorizontalSpriteVertLight;

	// Token: 0x04001F0B RID: 7947
	public Material matVerticalSprite;

	// Token: 0x04001F0C RID: 7948
	public Material matVerticalSpriteShadowCaster;

	// Token: 0x04001F0D RID: 7949
	public Material matGround;

	// Token: 0x04001F0E RID: 7950
	public Material shadowMaterial;

	// Token: 0x04001F0F RID: 7951
	public Material cloudsMaterial;

	// Token: 0x04001F10 RID: 7952
	public Material windClothMaterial;

	// Token: 0x04001F11 RID: 7953
	public Material matDeformingGrass;

	// Token: 0x04001F12 RID: 7954
	public Material fullTransparent;

	// Token: 0x04001F13 RID: 7955
	public Material fakeLightMaterial;

	// Token: 0x04001F14 RID: 7956
	[Header("Shaders")]
	public Shader smartRasterizerShader;

	// Token: 0x04001F15 RID: 7957
	[Header("Meshes")]
	public Mesh humanoidSizedPlaneMesh;

	// Token: 0x04001F16 RID: 7958
	[Header("FX")]
	public FXSettings fxSettings;

	// Token: 0x04001F17 RID: 7959
	[Header("Fighting")]
	public FightingGlobal fighting;

	// Token: 0x04001F18 RID: 7960
	private ValueTuple<float, Object3DMesh>[] obj3DMeshWithZPos = new ValueTuple<float, Object3DMesh>[9];

	// Token: 0x04001F19 RID: 7961
	private Dictionary<Object3DMesh, int> object3DMeshes = new Dictionary<Object3DMesh, int>();

	// Token: 0x04001F1A RID: 7962
	private int lastOccupiedIdx = -1;

	// Token: 0x04001F1B RID: 7963
	private static GlobalResources aliveInstance;
}
