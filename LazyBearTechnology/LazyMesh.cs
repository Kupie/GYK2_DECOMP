using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000180 RID: 384
	public static class LazyMesh
	{
		// Token: 0x0600088B RID: 2187 RVA: 0x0002A04F File Offset: 0x0002824F
		public static Mesh CloneMesh(Mesh mesh)
		{
			Mesh mesh2 = new Mesh();
			LazyMesh.CloneMeshInto(mesh2, mesh);
			mesh2.name = mesh.name;
			return mesh2;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0002A06C File Offset: 0x0002826C
		public static void CloneMeshInto(Mesh dest, Mesh src)
		{
			dest.colors = src.colors;
			dest.vertices = src.vertices;
			dest.normals = src.normals;
			dest.triangles = src.triangles;
			dest.tangents = src.tangents;
			dest.uv = src.uv;
			dest.uv2 = src.uv2;
			dest.uv3 = src.uv3;
			dest.uv4 = src.uv4;
			dest.uv5 = src.uv5;
			dest.uv6 = src.uv6;
			dest.uv7 = src.uv7;
			dest.uv8 = src.uv8;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0002A118 File Offset: 0x00028318
		private static T[] MergeArrays<T>(T[] array1, T[] array2)
		{
			T[] array3 = new T[array1.Length + array2.Length];
			Array.Copy(array1, array3, array1.Length);
			Array.Copy(array2, 0, array3, array1.Length, array2.Length);
			return array3;
		}
	}
}
