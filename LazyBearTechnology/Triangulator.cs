using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200017D RID: 381
	public class Triangulator
	{
		// Token: 0x06000881 RID: 2177 RVA: 0x00029C1D File Offset: 0x00027E1D
		public Triangulator(Vector2[] points)
		{
			this.mPoints = new List<Vector2>(points);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00029C31 File Offset: 0x00027E31
		public Triangulator(List<Vector2> points)
		{
			this.mPoints = points;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00029C40 File Offset: 0x00027E40
		public int[] Triangulate()
		{
			List<int> list = new List<int>();
			int count = this.mPoints.Count;
			if (count < 3)
			{
				return list.ToArray();
			}
			int[] array = new int[count];
			if (this.Area() > 0f)
			{
				for (int i = 0; i < count; i++)
				{
					array[i] = i;
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					array[j] = count - 1 - j;
				}
			}
			int k = count;
			int num = 2 * k;
			int num2 = k - 1;
			while (k > 2)
			{
				if (num-- <= 0)
				{
					return list.ToArray();
				}
				int num3 = num2;
				if (k <= num3)
				{
					num3 = 0;
				}
				num2 = num3 + 1;
				if (k <= num2)
				{
					num2 = 0;
				}
				int num4 = num2 + 1;
				if (k <= num4)
				{
					num4 = 0;
				}
				if (this.Snip(num3, num2, num4, k, array))
				{
					int num5 = array[num3];
					int num6 = array[num2];
					int num7 = array[num4];
					list.Add(num5);
					list.Add(num6);
					list.Add(num7);
					int num8 = num2;
					for (int l = num2 + 1; l < k; l++)
					{
						array[num8] = array[l];
						num8++;
					}
					k--;
					num = 2 * k;
				}
			}
			list.Reverse();
			return list.ToArray();
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00029D78 File Offset: 0x00027F78
		private float Area()
		{
			int count = this.mPoints.Count;
			float num = 0f;
			int num2 = count - 1;
			int i = 0;
			while (i < count)
			{
				Vector2 vector = this.mPoints[num2];
				Vector2 vector2 = this.mPoints[i];
				num += vector.x * vector2.y - vector2.x * vector.y;
				num2 = i++;
			}
			return num * 0.5f;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00029DF0 File Offset: 0x00027FF0
		private bool Snip(int u, int v, int w, int n, int[] V)
		{
			Vector2 vector = this.mPoints[V[u]];
			Vector2 vector2 = this.mPoints[V[v]];
			Vector2 vector3 = this.mPoints[V[w]];
			if (Mathf.Epsilon > (vector2.x - vector.x) * (vector3.y - vector.y) - (vector2.y - vector.y) * (vector3.x - vector.x))
			{
				return false;
			}
			for (int i = 0; i < n; i++)
			{
				if (i != u && i != v && i != w)
				{
					Vector2 vector4 = this.mPoints[V[i]];
					if (this.InsideTriangle(vector, vector2, vector3, vector4))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00029EA8 File Offset: 0x000280A8
		private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
		{
			float num = C.x - B.x;
			float num2 = C.y - B.y;
			float num3 = A.x - C.x;
			float num4 = A.y - C.y;
			float num5 = B.x - A.x;
			float num6 = B.y - A.y;
			float num7 = P.x - A.x;
			float num8 = P.y - A.y;
			float num9 = P.x - B.x;
			float num10 = P.y - B.y;
			float num11 = P.x - C.x;
			float num12 = P.y - C.y;
			float num13 = num * num10 - num2 * num9;
			float num14 = num5 * num8 - num6 * num7;
			float num15 = num3 * num12 - num4 * num11;
			return num13 >= 0f && num15 >= 0f && num14 >= 0f;
		}

		// Token: 0x0400053B RID: 1339
		private readonly List<Vector2> mPoints;
	}
}
