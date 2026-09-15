using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200017A RID: 378
	public class FastPixelsArray
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00029318 File Offset: 0x00027518
		public int width { get; }

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x00029320 File Offset: 0x00027520
		public int height { get; }

		// Token: 0x06000844 RID: 2116 RVA: 0x00029328 File Offset: 0x00027528
		public FastPixelsArray(Texture2D texture)
		{
			this.pixels = texture.GetPixels();
			this.width = texture.width;
			this.height = texture.height;
			this.textureFormat = texture.format;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00029360 File Offset: 0x00027560
		public FastPixelsArray(int width, int height)
			: this(width, height, new Color(0f, 0f, 0f, 0f), TextureFormat.RGBA32)
		{
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00029384 File Offset: 0x00027584
		public FastPixelsArray(int width, int height, Color fillColor, TextureFormat textureFormat = TextureFormat.RGBA32)
		{
			this.pixels = new Color[width * height];
			for (int i = 0; i < this.pixels.Length; i++)
			{
				this.pixels[i] = fillColor;
			}
			this.width = width;
			this.height = height;
			this.textureFormat = textureFormat;
		}

		// Token: 0x1700011F RID: 287
		public Color this[int x, int y]
		{
			get
			{
				return this.pixels[this.InBoundsX(x) + this.InBoundsY(y) * this.width];
			}
			set
			{
				this.pixels[this.InBoundsX(x) + this.InBoundsY(y) * this.width] = value;
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00029422 File Offset: 0x00027622
		public bool IsTransparent(int x, int y)
		{
			return this[x, y].a > 0.5f;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00029438 File Offset: 0x00027638
		public bool IsTransparent(IntVector2 pos)
		{
			return this.IsTransparent(pos.x, pos.y);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0002944C File Offset: 0x0002764C
		public Texture2D GetTexture()
		{
			if (this.tex == null)
			{
				this.tex = new Texture2D(this.width, this.height, this.textureFormat, false);
			}
			this.tex.SetPixels(this.pixels);
			this.tex.Apply();
			return this.tex;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000294A7 File Offset: 0x000276A7
		private int InBoundsX(int x)
		{
			return Mathf.Max(0, Mathf.Min(this.width - 1, x));
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000294BD File Offset: 0x000276BD
		private int InBoundsY(int y)
		{
			return Mathf.Max(0, Mathf.Min(this.height - 1, y));
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000294D4 File Offset: 0x000276D4
		public void DrawFilledTriangle(IntVector2 p1, IntVector2 p2, IntVector2 p3, Color color)
		{
			int x = p1.x;
			int y = p1.y;
			int x2 = p2.x;
			int y2 = p2.y;
			int x3 = p3.x;
			int y3 = p3.y;
			if (y2 > y3)
			{
				FastPixelsArray.SwapInts(ref x2, ref x3);
				FastPixelsArray.SwapInts(ref y2, ref y3);
			}
			if (y > y2)
			{
				FastPixelsArray.SwapInts(ref x, ref x2);
				FastPixelsArray.SwapInts(ref y, ref y2);
			}
			if (y2 > y3)
			{
				FastPixelsArray.SwapInts(ref x2, ref x3);
				FastPixelsArray.SwapInts(ref y2, ref y3);
			}
			float num = (float)(x3 - x) / (float)(y3 - y + 1);
			float num2 = (float)(x2 - x) / (float)(y2 - y + 1);
			float num3 = (float)(x3 - x2) / (float)(y3 - y2 + 1);
			float num4 = (float)x;
			float num5 = (float)x + num2;
			for (int i = y; i <= Mathf.Min(y3, this.height - 1); i++)
			{
				if (i >= 0)
				{
					int num6 = Mathf.Max(0, (int)num4);
					while ((float)num6 <= Mathf.Min((float)(this.width - 1), num5))
					{
						this[num6, i] = color;
						num6++;
					}
					int num7 = Mathf.Min((int)num4, this.width - 1);
					while ((float)num7 >= Mathf.Max(0f, num5))
					{
						this[num7, i] = color;
						num7--;
					}
				}
				num4 += num;
				if (i < y2)
				{
					num5 += num2;
				}
				else
				{
					num5 += num3;
				}
			}
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00029638 File Offset: 0x00027838
		private static void SwapInts(ref int a, ref int b)
		{
			int num = a;
			a = b;
			b = num;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00029650 File Offset: 0x00027850
		public void DrawFilledCircle(IntVector2 center, int radius, Color color)
		{
			int num = radius * radius;
			for (int i = -radius; i < radius; i++)
			{
				int num2 = i + center.x;
				if (num2 >= 0 && num2 < this.width)
				{
					for (int j = -radius; j < radius; j++)
					{
						int num3 = j + center.y;
						if (num3 >= 0 && num3 < this.height && i * i + j * j <= num)
						{
							this[num2, num3] = color;
						}
					}
				}
			}
		}

		// Token: 0x04000531 RID: 1329
		private readonly Color[] pixels;

		// Token: 0x04000532 RID: 1330
		private Texture2D tex;

		// Token: 0x04000533 RID: 1331
		private TextureFormat textureFormat;
	}
}
