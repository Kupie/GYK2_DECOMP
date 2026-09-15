using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200017B RID: 379
	[Serializable]
	public struct IntVector2
	{
		// Token: 0x06000851 RID: 2129 RVA: 0x000296BE File Offset: 0x000278BE
		public IntVector2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x000296CE File Offset: 0x000278CE
		public IntVector2(float x, float y)
		{
			this.x = Mathf.RoundToInt(x);
			this.y = Mathf.RoundToInt(y);
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x000296E8 File Offset: 0x000278E8
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x000296F0 File Offset: 0x000278F0
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

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x000296F9 File Offset: 0x000278F9
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x00029701 File Offset: 0x00027901
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

		// Token: 0x06000857 RID: 2135 RVA: 0x0002970A File Offset: 0x0002790A
		public static IntVector2 operator +(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x + b.x, a.y + b.y);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0002972B File Offset: 0x0002792B
		public static IntVector2 operator -(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x - b.x, a.y - b.y);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002974C File Offset: 0x0002794C
		public static IntVector2 operator *(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x * b.x, a.y * b.y);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0002976D File Offset: 0x0002796D
		public static IntVector2 operator /(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x / b.x, a.y / b.y);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0002978E File Offset: 0x0002798E
		public static IntVector2 operator +(IntVector2 a, int b)
		{
			return new IntVector2(a.x + b, a.y + b);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000297A5 File Offset: 0x000279A5
		public static IntVector2 operator -(IntVector2 a, int b)
		{
			return new IntVector2(a.x - b, a.y - b);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000297BC File Offset: 0x000279BC
		public static IntVector2 operator *(IntVector2 a, int b)
		{
			return new IntVector2(a.x * b, a.y * b);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000297D3 File Offset: 0x000279D3
		public static IntVector2 operator /(IntVector2 a, int b)
		{
			return new IntVector2(a.x / b, a.y / b);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000297EA File Offset: 0x000279EA
		public static bool operator ==(IntVector2 a, IntVector2 b)
		{
			return a.x == b.x && a.y == b.y;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0002980A File Offset: 0x00027A0A
		public static bool operator !=(IntVector2 a, IntVector2 b)
		{
			return a.x != b.x || a.y != b.y;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0002982D File Offset: 0x00027A2D
		public bool Equals(IntVector2 o)
		{
			return this.x == o.x && this.y == o.y;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00029850 File Offset: 0x00027A50
		public override bool Equals(object obj)
		{
			if (obj is IntVector2)
			{
				IntVector2 intVector = (IntVector2)obj;
				return this.Equals(intVector);
			}
			return false;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00029875 File Offset: 0x00027A75
		public override string ToString()
		{
			return string.Format("({0},{1})", this.x, this.y);
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00029897 File Offset: 0x00027A97
		public float magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.y * this.y));
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x000298BA File Offset: 0x00027ABA
		public override int GetHashCode()
		{
			return (this.x * 397) ^ this.y;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x000298CF File Offset: 0x00027ACF
		public static implicit operator Vector2(IntVector2 v)
		{
			return new Vector2((float)v.x, (float)v.y);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x000298E4 File Offset: 0x00027AE4
		public static explicit operator IntVector2(Vector2 v)
		{
			return new IntVector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		// Token: 0x04000536 RID: 1334
		public int x;

		// Token: 0x04000537 RID: 1335
		public int y;
	}
}
