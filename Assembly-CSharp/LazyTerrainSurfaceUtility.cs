using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
public static class LazyTerrainSurfaceUtility
{
	// Token: 0x06002DE4 RID: 11748 RVA: 0x000DB6A4 File Offset: 0x000D98A4
	public static GameObject GenerateObjectWithColliderFromSprite(Sprite sprite, float height)
	{
		GameObject gameObject = new GameObject(sprite.name);
		List<Vector2> list = new List<Vector2>();
		sprite.GetPhysicsShape(0, list);
		List<Vector3> list2 = new List<Vector3>();
		List<int> list3 = new List<int>();
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(new Vector3(list[i].x, list[i].y, 0f));
			list2.Add(new Vector3(list[i].x, list[i].y, height));
		}
		for (int j = 0; j < list.Count; j++)
		{
			int num2 = num + j * 2;
			int num3 = num + (j + 1) % list.Count * 2;
			list3.Add(num2);
			list3.Add(num2 + 1);
			list3.Add(num3);
			list3.Add(num3);
			list3.Add(num2 + 1);
			list3.Add(num3 + 1);
		}
		int count = list2.Count;
		int num4 = count + 1;
		list2.Add(new Vector3(0f, 0f, 0f));
		list2.Add(new Vector3(0f, 0f, height));
		for (int k = 0; k < list.Count; k++)
		{
			int num5 = (k + 1) % list.Count;
			int num6 = k * 2;
			int num7 = num6 + 1;
			list3.Add(count);
			list3.Add(num6);
			list3.Add(num5 * 2);
			list3.Add(num7);
			list3.Add(num4);
			list3.Add(num5 * 2 + 1);
		}
		Mesh mesh = new Mesh();
		mesh.vertices = list2.ToArray();
		mesh.triangles = list3.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshFilter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
		meshCollider.convex = true;
		meshRenderer.enabled = false;
		meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
		return gameObject;
	}
}
