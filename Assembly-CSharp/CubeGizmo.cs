using System;
using UnityEngine;

// Token: 0x02000AAD RID: 2733
public class CubeGizmo : MonoBehaviour
{
	// Token: 0x060049E4 RID: 18916 RVA: 0x0015D0AC File Offset: 0x0015B2AC
	private void OnDrawGizmosSelected()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (!component || !component.sharedMesh)
		{
			return;
		}
		Gizmos.color = this.color;
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Vector3 size = component.sharedMesh.bounds.size;
		Gizmos.DrawWireCube(Vector3.zero, size);
		Gizmos.matrix = matrix;
	}

	// Token: 0x040039A5 RID: 14757
	public Color color = Color.green;
}
