using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AF1 RID: 2801
[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class MirrorMeshCollider : MonoBehaviour
{
	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x06004AD7 RID: 19159 RVA: 0x00161420 File Offset: 0x0015F620
	private MeshCollider MeshCol
	{
		get
		{
			if (this.meshCol == null)
			{
				this.meshCol = base.GetComponent<MeshCollider>();
			}
			return this.meshCol;
		}
	}

	// Token: 0x06004AD8 RID: 19160 RVA: 0x00161442 File Offset: 0x0015F642
	private void OnEnable()
	{
		if (this.MeshCol != null && this.MeshCol.IsLossyScaleNegative())
		{
			this.MirrorCollider();
		}
	}

	// Token: 0x06004AD9 RID: 19161 RVA: 0x00161468 File Offset: 0x0015F668
	public void MirrorCollider()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (component == null || this.MeshCol == null)
		{
			return;
		}
		VHACD_ColliderDecomposer vhacd_ColliderDecomposer;
		if (base.TryGetComponent<VHACD_ColliderDecomposer>(out vhacd_ColliderDecomposer) && vhacd_ColliderDecomposer.IsValid)
		{
			return;
		}
		Mesh sharedMesh = component.sharedMesh;
		if (sharedMesh == null)
		{
			return;
		}
		base.transform.localScale = new Vector3(base.transform.localScale.x * this.mirrorScale.x, base.transform.localScale.y * this.mirrorScale.y, base.transform.localScale.z * this.mirrorScale.z);
		Mesh mesh = global::UnityEngine.Object.Instantiate<Mesh>(sharedMesh);
		List<Vector3> list = new List<Vector3>();
		foreach (Vector3 vector in mesh.vertices)
		{
			list.Add(Vector3.Scale(vector, this.mirrorScale));
		}
		mesh.vertices = list.ToArray();
		int[] triangles = mesh.triangles;
		for (int j = 0; j < triangles.Length; j += 3)
		{
			ref int ptr = ref triangles[j];
			int[] array = triangles;
			int num = j + 1;
			int i = triangles[j + 1];
			int num2 = triangles[j];
			ptr = i;
			array[num] = num2;
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		this.MeshCol.sharedMesh = mesh;
	}

	// Token: 0x04003C71 RID: 15473
	[SerializeField]
	private Vector3 mirrorScale = new Vector3(-1f, 1f, 1f);

	// Token: 0x04003C72 RID: 15474
	private MeshCollider meshCol;
}
