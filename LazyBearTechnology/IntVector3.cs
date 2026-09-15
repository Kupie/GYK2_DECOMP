using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200017C RID: 380
	[Serializable]
	public struct IntVector3
	{
		// Token: 0x06000868 RID: 2152 RVA: 0x00029901 File Offset: 0x00027B01
		public IntVector3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00029918 File Offset: 0x00027B18
		public IntVector3(float x, float y, float z)
		{
			this.x = Mathf.RoundToInt(x);
			this.y = Mathf.RoundToInt(y);
			this.z = Mathf.RoundToInt(z);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x0002993E File Offset: 0x00027B3E
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x00029946 File Offset: 0x00027B46
		public int v1
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0002994F File Offset: 0x00027B4F
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x00029957 File Offset: 0x00027B57
		public int v2
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00029960 File Offset: 0x00027B60
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x00029968 File Offset: 0x00027B68
		public int v3
		{
			get
			{
				return this.z;
			}
			set
			{
				this.z = value;
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00029971 File Offset: 0x00027B71
		public static IntVector3 operator +(IntVector3 a, IntVector3 b)
		{
			return new IntVector3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0002999F File Offset: 0x00027B9F
		public static IntVector3 operator -(IntVector3 a, IntVector3 b)
		{
			return new IntVector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000299CD File Offset: 0x00027BCD
		public static IntVector3 operator *(IntVector3 a, IntVector3 b)
		{
			return new IntVector3(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000299FB File Offset: 0x00027BFB
		public static IntVector3 operator /(IntVector3 a, IntVector3 b)
		{
			return new IntVector3(a.x / b.x, a.y / b.y, a.z / b.z);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00029A29 File Offset: 0x00027C29
		public static IntVector3 operator +(IntVector3 a, int b)
		{
			return new IntVector3(a.x + b, a.y + b, a.z + b);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00029A48 File Offset: 0x00027C48
		public static IntVector3 operator -(IntVector3 a, int b)
		{
			return new IntVector3(a.x - b, a.y - b, a.z - b);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00029A67 File Offset: 0x00027C67
		public static IntVector3 operator *(IntVector3 a, int b)
		{
			return new IntVector3(a.x * b, a.y * b, a.z * b);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00029A86 File Offset: 0x00027C86
		public static IntVector3 operator /(IntVector3 a, int b)
		{
			return new IntVector3(a.x / b, a.y / b, a.z / b);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00029AA5 File Offset: 0x00027CA5
		public static bool operator ==(IntVector3 a, IntVector3 b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00029AD3 File Offset: 0x00027CD3
		public static bool operator !=(IntVector3 a, IntVector3 b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00029B04 File Offset: 0x00027D04
		public bool Equals(IntVector3 o)
		{
			return this.x == o.x && this.y == o.y && this.z == o.z;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00029B34 File Offset: 0x00027D34
		public override bool Equals(object obj)
		{
			if (obj is IntVector3)
			{
				IntVector3 intVector = (IntVector3)obj;
				return this.Equals(intVector);
			}
			return false;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00029B59 File Offset: 0x00027D59
		public override string ToString()
		{
			return string.Format("({0},{1},{2})", this.x, this.y, this.z);
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x00029B86 File Offset: 0x00027D86
		public float magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.y * this.y + this.z * this.z));
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00029BB7 File Offset: 0x00027DB7
		public override int GetHashCode()
		{
			return (this.x * 397) ^ this.y ^ (this.z * 384723);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00029BD9 File Offset: 0x00027DD9
		public static implicit operator Vector3(IntVector3 v)
		{
			return new Vector3((float)v.x, (float)v.y, (float)v.z);
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00029BF5 File Offset: 0x00027DF5
		public static explicit operator IntVector3(Vector3 v)
		{
			return new IntVector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		}

		// Token: 0x04000538 RID: 1336
		public int x;

		// Token: 0x04000539 RID: 1337
		public int y;

		// Token: 0x0400053A RID: 1338
		public int z;
	}
}
