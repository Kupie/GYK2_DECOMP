using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class PoissonDiskSampler3D
{
	// Token: 0x06000A86 RID: 2694 RVA: 0x0003511C File Offset: 0x0003331C
	public PoissonDiskSampler3D(float width, float height, float depth, float radius)
	{
		this.cube = new Vector3(width, height, depth);
		this.radius2 = radius * radius;
		this.cellSize = radius / Mathf.Sqrt(3f);
		this.grid = new Vector3[Mathf.CeilToInt(width / this.cellSize), Mathf.CeilToInt(height / this.cellSize), Mathf.CeilToInt(depth / this.cellSize)];
		Debug.Log(this.grid.GetLength(0));
		Debug.Log(this.grid.GetLength(1));
		Debug.Log(this.grid.GetLength(2));
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x000351DC File Offset: 0x000333DC
	public List<Vector3> Samples()
	{
		List<Vector3> list = new List<Vector3>();
		list.Add(this.AddSample(new Vector3(global::UnityEngine.Random.value * this.cube.x, global::UnityEngine.Random.value * this.cube.y, global::UnityEngine.Random.value * this.cube.z)));
		while (this.activeSamples.Count > 0)
		{
			int num = (int)global::UnityEngine.Random.value * this.activeSamples.Count;
			Vector3 vector = this.activeSamples[num];
			bool flag = false;
			for (int i = 0; i < 30; i++)
			{
				Vector3 vector2 = this.GenerateRandomPointAround(vector, global::UnityEngine.Random.value * 3f * this.radius2 + this.radius2);
				if (this.IsContains(vector2, this.cube) && this.IsFarEnough(vector2))
				{
					flag = true;
					list.Add(this.AddSample(vector2));
					break;
				}
			}
			if (!flag)
			{
				this.activeSamples[num] = this.activeSamples[this.activeSamples.Count - 1];
				this.activeSamples.RemoveAt(this.activeSamples.Count - 1);
			}
		}
		return list;
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0003530C File Offset: 0x0003350C
	private bool IsContains(Vector3 v, Vector3 area)
	{
		return v.x >= 0f && v.x < area.x && v.y >= 0f && v.y < area.y && v.z >= 0f && v.z < area.z;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00035370 File Offset: 0x00033570
	private Vector3 GenerateRandomPointAround(Vector3 point, float minDist)
	{
		float value = global::UnityEngine.Random.value;
		float value2 = global::UnityEngine.Random.value;
		float value3 = global::UnityEngine.Random.value;
		float num = minDist * (value + 1f);
		float num2 = 6.2831855f * value2;
		float num3 = 6.2831855f * value3;
		float num4 = point.x + num * Mathf.Cos(num2) * Mathf.Sin(num3);
		float num5 = point.y + num * Mathf.Sin(num2) * Mathf.Sin(num3);
		float num6 = point.z + num * Mathf.Cos(num3);
		return new Vector3(num4, num5, num6);
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x000353F8 File Offset: 0x000335F8
	private bool IsFarEnough(Vector3 sample)
	{
		PoissonDiskSampler3D.GridPos gridPos = new PoissonDiskSampler3D.GridPos(sample, this.cellSize);
		int num = Mathf.Max(gridPos.x - 2, 0);
		int num2 = Mathf.Max(gridPos.y - 2, 0);
		int num3 = Mathf.Max(gridPos.z - 2, 0);
		int num4 = Mathf.Min(gridPos.x + 2, this.grid.GetLength(0) - 1);
		int num5 = Mathf.Min(gridPos.y + 2, this.grid.GetLength(1) - 1);
		int num6 = Mathf.Min(gridPos.z + 2, this.grid.GetLength(2) - 1);
		for (int i = num3; i <= num6; i++)
		{
			for (int j = num2; j <= num5; j++)
			{
				for (int k = num; k <= num4; k++)
				{
					Vector3 vector = this.grid[k, j, i];
					if (vector != Vector3.zero)
					{
						Vector3 vector2 = vector - sample;
						if (vector2.x * vector2.x + vector2.y * vector2.y + vector2.z * vector2.z < this.radius2)
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x00035530 File Offset: 0x00033730
	private Vector3 AddSample(Vector3 sample)
	{
		this.activeSamples.Add(sample);
		PoissonDiskSampler3D.GridPos gridPos = new PoissonDiskSampler3D.GridPos(sample, this.cellSize);
		this.grid[gridPos.x, gridPos.y, gridPos.z] = sample;
		return sample;
	}

	// Token: 0x04000BF2 RID: 3058
	private const int k = 30;

	// Token: 0x04000BF3 RID: 3059
	private readonly Vector3 cube;

	// Token: 0x04000BF4 RID: 3060
	private readonly float radius2;

	// Token: 0x04000BF5 RID: 3061
	private readonly float cellSize;

	// Token: 0x04000BF6 RID: 3062
	private Vector3[,,] grid;

	// Token: 0x04000BF7 RID: 3063
	private List<Vector3> activeSamples = new List<Vector3>();

	// Token: 0x020001A0 RID: 416
	private struct GridPos
	{
		// Token: 0x06000A8C RID: 2700 RVA: 0x00035576 File Offset: 0x00033776
		public GridPos(Vector3 sample, float cellSize)
		{
			this.x = (int)(sample.x / cellSize);
			this.y = (int)(sample.y / cellSize);
			this.z = (int)(sample.z / cellSize);
		}

		// Token: 0x04000BF8 RID: 3064
		public int x;

		// Token: 0x04000BF9 RID: 3065
		public int y;

		// Token: 0x04000BFA RID: 3066
		public int z;
	}
}
