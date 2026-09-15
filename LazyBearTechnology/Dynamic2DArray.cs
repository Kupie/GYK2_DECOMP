using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LazyBearTechnology
{
	// Token: 0x02000179 RID: 377
	public class Dynamic2DArray<T> : IEnumerable<ValueTuple<int, int, T>>, IEnumerable
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x000291BC File Offset: 0x000273BC
		public int MinX
		{
			get
			{
				return this.minX;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x000291C4 File Offset: 0x000273C4
		public int MaxX
		{
			get
			{
				return this.maxX;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x000291CC File Offset: 0x000273CC
		public int MinY
		{
			get
			{
				return this.minY;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x000291D4 File Offset: 0x000273D4
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
		}

		// Token: 0x1700011C RID: 284
		public T this[int x, int y]
		{
			get
			{
				if (x < this.minX || y < this.minY || x > this.maxX || y > this.maxY)
				{
					return default(T);
				}
				Dictionary<int, T> dictionary;
				if (!this.data.TryGetValue(x, out dictionary))
				{
					return default(T);
				}
				T t;
				if (!dictionary.TryGetValue(y, out t))
				{
					return default(T);
				}
				return t;
			}
			set
			{
				Dictionary<int, T> dictionary;
				if (!this.data.TryGetValue(x, out dictionary))
				{
					dictionary = new Dictionary<int, T>();
					this.data.Add(x, dictionary);
				}
				if (!dictionary.TryAdd(y, value))
				{
					dictionary[y] = value;
				}
				if (x < this.minX)
				{
					this.minX = x;
				}
				if (x > this.maxX)
				{
					this.maxX = x;
				}
				if (y < this.minY)
				{
					this.minY = y;
				}
				if (y > this.maxY)
				{
					this.maxY = y;
				}
			}
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x000292CA File Offset: 0x000274CA
		[return: TupleElementNames(new string[] { "x", "y", "value" })]
		public IEnumerator<ValueTuple<int, int, T>> GetEnumerator()
		{
			int num;
			for (int x = this.MinX; x <= this.MaxX; x = num + 1)
			{
				Dictionary<int, T> xdata;
				if (this.data.TryGetValue(x, out xdata) && xdata != null)
				{
					for (int y = this.MinY; y <= this.MaxY; y = num + 1)
					{
						T t;
						if (xdata.TryGetValue(y, out t) && t != null)
						{
							yield return new ValueTuple<int, int, T>(x, y, t);
						}
						num = y;
					}
					xdata = null;
				}
				num = x;
			}
			yield break;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000292D9 File Offset: 0x000274D9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000292E1 File Offset: 0x000274E1
		public void Delete(int x, int y)
		{
			if (this[x, y] != null)
			{
				this.data[x].Remove(y);
			}
		}

		// Token: 0x0400052C RID: 1324
		private readonly Dictionary<int, Dictionary<int, T>> data = new Dictionary<int, Dictionary<int, T>>();

		// Token: 0x0400052D RID: 1325
		private int minX;

		// Token: 0x0400052E RID: 1326
		private int minY;

		// Token: 0x0400052F RID: 1327
		private int maxX;

		// Token: 0x04000530 RID: 1328
		private int maxY;
	}
}
